using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;
using Volo.Abp.Users;
using Microsoft.EntityFrameworkCore;
using Dev.Acadmy.Response;
using Dev.Acadmy.MediaItems;
using static Dev.Acadmy.Permissions.AcadmyPermissions;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;
using Dev.Acadmy.Quizzes;
using Dev.Acadmy.Lectures;


namespace Dev.Acadmy.Courses
{
    public class CourseStudentManager :DomainService
    {
        private readonly IRepository<CourseStudent> _coursestudentRepository;
        private readonly IMapper _mapper;
        private readonly IIdentityUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;
        private readonly MediaItemManager _mediaItemManager;
        private readonly IRepository<QuizStudent ,Guid> _quizStudentRepository;
        private readonly IRepository<LectureTry, Guid> _lectureTryRepository;
        public CourseStudentManager(IRepository<LectureTry, Guid> lectureTryRepository, IRepository<QuizStudent, Guid> quizStudentRepository, MediaItemManager mediaItemManager, ICurrentUser currentUser, IIdentityUserRepository userRepository, IMapper mapper, IRepository<CourseStudent> coursestudentRepository)
        {
            _lectureTryRepository = lectureTryRepository;
            _quizStudentRepository = quizStudentRepository;
            _mediaItemManager = mediaItemManager;
            _currentUser = currentUser;
            _userRepository =userRepository;
            _coursestudentRepository = coursestudentRepository;
            _mapper = mapper;
        }

        public async Task<ResponseApi<CourseStudentDto>> GetAsync(Guid id)
        {
            var coursestudent = await _coursestudentRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (coursestudent == null) return new ResponseApi<CourseStudentDto> { Data = null, Success = false, Message = "Not found coursestudent" };
            var user = await _userRepository.GetAsync(coursestudent.UserId);
            var dto = _mapper.Map<CourseStudentDto>(coursestudent);
            dto.Name = user.Name;
            var medaiItem = await _mediaItemManager.GetAsync(coursestudent.UserId);
            dto.LogoUrl = medaiItem?.Url??"";
            return new ResponseApi<CourseStudentDto> { Data = dto, Success = true, Message = "find succeess" };
        }

        public async Task<PagedResultDto<CourseStudentDto>> GetListAsync(int pageNumber, int pageSize,bool isSubscribe,Guid courseId ,string? search )
        {
            var roles = await _userRepository.GetRolesAsync(_currentUser.GetId());
            var queryable = await _coursestudentRepository.GetQueryableAsync();
            if (!string.IsNullOrWhiteSpace(search)) queryable = queryable.Include(x=>x.Course).Include(x=>x.User).Where(c => c.Course.Name.Contains(search) || c.User.Name.Contains(search));
            var totalCount = await AsyncExecuter.CountAsync(queryable.Where(x=>x.IsSubscibe == isSubscribe && x.CourseId == courseId));
            var coursestudents = new List<CourseStudent>();
            if (roles.Any(x => x.Name.ToUpper() == RoleConsts.Admin.ToUpper())) coursestudents = await AsyncExecuter.ToListAsync(queryable.Include(x => x.Course).Include(x => x.User).Where(x=>x.IsSubscibe == isSubscribe && x.CourseId == courseId).OrderByDescending(c => c.CreationTime).Skip((pageNumber - 1) * pageSize).Take(pageSize));
            else coursestudents = await AsyncExecuter.ToListAsync(queryable.Include(x => x.Course).Include(x => x.User).Where(c => c.Course.UserId == _currentUser.GetId() && c.IsSubscibe == isSubscribe && c.CourseId ==courseId).OrderByDescending(c => c.CreationTime).Skip((pageNumber - 1) * pageSize).Take(pageSize));
            var coursestudentDtos = _mapper.Map<List<CourseStudentDto>>(coursestudents);
            foreach(var dto in coursestudentDtos)
            {
                var user = await _userRepository.GetAsync(dto.UserId);
                dto.Name = user.Name;
                var medaiItem = await _mediaItemManager.GetAsync(user.Id);
                dto.LogoUrl = medaiItem?.Url ?? "";
            }
            return new PagedResultDto<CourseStudentDto>(totalCount, coursestudentDtos);
        }

        public async Task<ResponseApi<CourseStudentDto>> CreateAsync(CreateUpdateCourseStudentDto input)
        {
            var coursestudent = _mapper.Map<CourseStudent>(input);
            coursestudent.IsSubscibe = false; 
            var result = await _coursestudentRepository.InsertAsync(coursestudent);
            var dto = _mapper.Map<CourseStudentDto>(result);
            return new ResponseApi<CourseStudentDto> { Data = dto, Success = true, Message = "save succeess" };
        }

        public async Task<ResponseApi<CourseStudentDto>> UpdateAsync(Guid id, CreateUpdateCourseStudentDto input)
        {
            var coursestudentDB = await _coursestudentRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (coursestudentDB == null) return new ResponseApi<CourseStudentDto> { Data = null, Success = false, Message = "Not found coursestudent" };
            var coursestudent = _mapper.Map(input, coursestudentDB);
            if (coursestudent.IsSubscibe) 
            { 
                var result = await _coursestudentRepository.UpdateAsync(coursestudent);
                var dto = _mapper.Map<CourseStudentDto>(result);
                return new ResponseApi<CourseStudentDto> { Data = dto, Success = true, Message = "update succeess" };
            }
            else 
            {
                await _coursestudentRepository.DeleteAsync(coursestudent);
                return new ResponseApi<CourseStudentDto> { Data = null, Success = true, Message = "delete succeess" };
            }
        }
        

        public async Task<ResponseApi<bool>> DeleteAsync(Guid id)
        {
            var coursestudent = await _coursestudentRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (coursestudent == null) return new ResponseApi<bool> { Data = false, Success = false, Message = "Not found coursestudent" };
            await _coursestudentRepository.DeleteAsync(coursestudent);
            return new ResponseApi<bool> { Data = true, Success = true, Message = "delete succeess" };
        }

        public async Task<PagedResultDto<StudentDegreeByCourseDto>> GetStudentDegreByCourseAsync(int pageNumber, int pageSize, Guid courseId, Guid userId)
        {
            // 🟢 1. الاستعلام الأساسي
            var query = await _quizStudentRepository.GetQueryableAsync();

            var studentQuizzesQuery = query
                .Include(x => x.Quiz)
                    .ThenInclude(x => x.Lecture)
                        .ThenInclude(x => x.Chapter)
                            .ThenInclude(x => x.Course)
                .Where(x => x.UserId == userId && x.Quiz.Lecture.Chapter.CourseId == courseId);

            // 🟢 2. إجمالي السجلات
            var totalCount = await studentQuizzesQuery.CountAsync();

            // 🟢 3. تطبيق Pagination
            var studentQuizzes = await studentQuizzesQuery
                .OrderByDescending(x => x.CreationTime)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // 🟢 4. تجهيز البيانات
            var user = await _userRepository.GetAsync(userId);
            var mediaItem = await _mediaItemManager.GetAsync(userId);

            var result = new StudentDegreeByCourseDto
            {
                UserId = user.Id,
                Name = user.Name,
                LogoUrl = mediaItem?.Url ?? ""
            };

            foreach (var studentQuiz in studentQuizzes)
            {
                var lectureTry = await _lectureTryRepository.FirstOrDefaultAsync(x =>
                    x.LectureId == studentQuiz.Quiz.LectureId && x.UserId == userId);

                result.Quizzes.Add(new StudentQuizDto
                {
                    QuizName = studentQuiz.Quiz?.Title ?? string.Empty,
                    LectureName = studentQuiz.Quiz?.Lecture?.Title ?? string.Empty,
                    QuizScore = studentQuiz.Score,
                    TryCount = lectureTry?.MyTryCount ?? 0
                });
            }

            // 🟢 5. إرجاع النتيجة مع pagination
            return new PagedResultDto<StudentDegreeByCourseDto>(
                totalCount,
                new List<StudentDegreeByCourseDto> { result }
            );
        }



    }
}
