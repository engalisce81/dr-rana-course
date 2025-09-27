using AutoMapper;
using Dev.Acadmy.Lectures;
using Dev.Acadmy.LookUp;
using Dev.Acadmy.MediaItems;
using Dev.Acadmy.Quizzes;
using Dev.Acadmy.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace Dev.Acadmy.Chapters
{
    public class ChapterManager:DomainService
    {
        private readonly IRepository<Chapter> _chapterRepository;
        private readonly IMapper _mapper;
        private readonly IIdentityUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IRepository<QuizStudent, Guid> _quizStudentRepository;
        private readonly MediaItemManager _mediaItemManager;
        public ChapterManager(MediaItemManager mediaItemManager, IRepository<QuizStudent, Guid> quizStudentRepository, ICurrentUser currentUser, IIdentityUserRepository userRepository, IMapper mapper, IRepository<Chapter> chapterRepository)
        {
            _mediaItemManager = mediaItemManager;
            _quizStudentRepository = quizStudentRepository;
            _currentUser = currentUser;
            _userRepository = userRepository;
            _chapterRepository = chapterRepository;
            _mapper = mapper;
        }

        public async Task<ResponseApi<ChapterDto>> GetAsync(Guid id)
        {
            var chapter = await _chapterRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (chapter == null) return new ResponseApi<ChapterDto> { Data = null, Success = false, Message = "Not found chapter" };
            var dto = _mapper.Map<ChapterDto>(chapter);
            return new ResponseApi<ChapterDto> { Data = dto, Success = true, Message = "find succeess" };
        }

        public async Task<PagedResultDto<ChapterDto>> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            var roles = await _userRepository.GetRolesAsync(_currentUser.GetId());
            var queryable = (await _chapterRepository.GetQueryableAsync());
            if (!string.IsNullOrWhiteSpace(search)) queryable = queryable.Include(x=>x.Course).Where(c => c.Name.Contains(search));
            var totalCount = await AsyncExecuter.CountAsync(queryable);
            var chapters = new List<Chapter>();  
            if (roles.Any(x=>x.Name.ToUpper()==RoleConsts.Admin.ToUpper())) chapters = await AsyncExecuter.ToListAsync(queryable.Include(x => x.Course).OrderByDescending(c => c.CreationTime).Skip((pageNumber - 1) * pageSize).Take(pageSize));
            else chapters = await AsyncExecuter.ToListAsync(queryable.Include(x => x.Course).Where(c => c.CreatorId == _currentUser.GetId()).OrderByDescending(c => c.CreationTime).Skip((pageNumber - 1) * pageSize).Take(pageSize));
            var chapterDtos = _mapper.Map<List<ChapterDto>>(chapters);
            return new PagedResultDto<ChapterDto>(totalCount, chapterDtos);
        }

        public async Task<ResponseApi<ChapterDto>> CreateAsync(CreateUpdateChapterDto input)
        {
            var chapter = _mapper.Map<Chapter>(input);
            var result = await _chapterRepository.InsertAsync(chapter);
            var dto = _mapper.Map<ChapterDto>(result);
            return new ResponseApi<ChapterDto> { Data = dto, Success = true, Message = "save succeess" };
        }

        public async Task<ResponseApi<ChapterDto>> UpdateAsync(Guid id, CreateUpdateChapterDto input)
        {
            var chapterDB = await _chapterRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (chapterDB == null) return new ResponseApi<ChapterDto> { Data = null, Success = false, Message = "Not found chapter" };
            var chapter = _mapper.Map(input, chapterDB);
            var result = await _chapterRepository.UpdateAsync(chapter);
            var dto = _mapper.Map<ChapterDto>(result);
            return new ResponseApi<ChapterDto> { Data = dto, Success = true, Message = "update succeess" };
        }

        public async Task<ResponseApi<bool>> DeleteAsync(Guid id)
        {
            var chapter = await _chapterRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (chapter == null) return new ResponseApi<bool> { Data = false, Success = false, Message = "Not found chapter" };
            await _chapterRepository.DeleteAsync(chapter);
            return new ResponseApi<bool> { Data = true, Success = true, Message = "delete succeess" };
        }

        public async Task<PagedResultDto<LookupDto>> GetListChaptersAsync()
        {
            var roles = await _userRepository.GetRolesAsync(_currentUser.GetId());
            var queryable = await _chapterRepository.GetQueryableAsync();
            var chapters = new List<Chapter>();
            if (roles.Any(x => x.Name.ToUpper() == RoleConsts.Admin.ToUpper())) chapters = await AsyncExecuter.ToListAsync(queryable.OrderByDescending(c => c.CreationTime).Take(100));
            else chapters = await AsyncExecuter.ToListAsync(queryable.Where(c => c.CreatorId == _currentUser.GetId()).OrderByDescending(c => c.CreationTime).Take(100));
            var chapterDtos = _mapper.Map<List<LookupDto>>(chapters);
            return new PagedResultDto<LookupDto>(chapterDtos.Count, chapterDtos);
        }

        public async Task<PagedResultDto<CourseChaptersDto>> GetCourseChaptersAsync(Guid courseId, int pageNumber, int pageSize)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;
            var currentUser = await _userRepository.GetAsync(_currentUser.GetId());
            // نجيب كل الكويزات اللي الطالب جاوبها قبل كده
            var answeredQuizIds = await (await _quizStudentRepository.GetQueryableAsync())
                .Where(qs => qs.UserId == currentUser.Id)
                .Select(qs => qs.QuizId)
                .ToListAsync();

            var queryable = await _chapterRepository.GetQueryableAsync();

            var query = queryable.Include(x => x.Course)
                .Include(c => c.Lectures)
                    .ThenInclude(l => l.Quiz)
                        .ThenInclude(q => q.Questions)
                .Where(c => c.CourseId == courseId);

            var totalCount = await query.CountAsync();

            var chapters = await query
                .OrderBy(c => c.CreationTime)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // بناء DTOs بطريقة Async
            var chapterInfoDtos = new List<CourseChaptersDto>();

            foreach (var c in chapters)
            {
                var lectureDtos = new List<LectureInfoDto>();

                foreach (var l in c.Lectures.Where(x => x.IsVisible))
                {
                    var media = await _mediaItemManager.GetAsync(l.Id, true);

                    lectureDtos.Add(new LectureInfoDto
                    {
                        LectureId = l.Id,
                        Title = l.Title,
                        Content = l.Content,
                        VideoUrl = l.VideoUrl,
                        PdfUrl = media?.Url??"", // بدل .Result
                        Quiz = new QuizInfoDto
                        {
                            QuizId = l.Quiz.Id,
                            Title = l.Quiz.Title,
                            QuestionsCount = l.Quiz.Questions.Count,
                            AlreadyAnswer = answeredQuizIds.Contains(l.Quiz.Id)
                        }
                    });
                }

                chapterInfoDtos.Add(new CourseChaptersDto
                {
                    CourseId = c.CourseId,
                    CourseName = c.Course.Name,
                    ChapterId = c.Id,
                    ChapterName = c.Name,
                    LectureCount = lectureDtos.Count,
                    Lectures = lectureDtos
                });
            }

            return new PagedResultDto<CourseChaptersDto>(totalCount, chapterInfoDtos);
        }


    }
}
