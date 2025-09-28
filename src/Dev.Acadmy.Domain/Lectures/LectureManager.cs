using AutoMapper;
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
using Microsoft.EntityFrameworkCore;
using Dev.Acadmy.Quizzes;
using Dev.Acadmy.Response;
using Dev.Acadmy.Questions;
using Volo.Abp;
using Dev.Acadmy.MediaItems;
using static Dev.Acadmy.Permissions.AcadmyPermissions;

namespace Dev.Acadmy.Lectures
{
    public class LectureManager:DomainService
    {
        private readonly IRepository<Lecture,Guid> _lectureRepository;
        private readonly IMapper _mapper;
        private readonly IIdentityUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;
        private readonly QuizManager _quizManager;
        private readonly IRepository<Quiz ,Guid> _quizRepository;
        private readonly MediaItemManager _mediaItemManager;
        private readonly IRepository<LectureStudent ,Guid> _lectureStudentRepository;
        public LectureManager(IRepository<LectureStudent, Guid> lectureStudentRepository, MediaItemManager mediaItemManager, IRepository<Quiz,Guid> quizRepository, QuizManager quizManager, ICurrentUser currentUser, IIdentityUserRepository userRepository, IMapper mapper, IRepository<Lecture,Guid> lectureRepository)
        {
            _lectureStudentRepository = lectureStudentRepository;
            _mediaItemManager = mediaItemManager;
            _quizRepository = quizRepository;
            _quizManager = quizManager;
            _currentUser = currentUser;
            _userRepository = userRepository;
            _lectureRepository = lectureRepository;
            _mapper = mapper;
        }

        public async Task<ResponseApi<LectureDto>> GetAsync(Guid id)
        {
            var lecture = await _lectureRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (lecture == null) return new ResponseApi<LectureDto> { Data = null, Success = false, Message = "Not found lecture" };
            var dto = _mapper.Map<LectureDto>(lecture);
            return new ResponseApi<LectureDto> { Data = dto, Success = true, Message = "find succeess" };
        }

        public async Task<PagedResultDto<LectureDto>> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            var roles = await _userRepository.GetRolesAsync(_currentUser.GetId());
            var queryable = await _lectureRepository.GetQueryableAsync();
            if (!string.IsNullOrWhiteSpace(search)) queryable = queryable.Include(x=>x.Chapter).Where(c => c.Content.ToUpper().Contains(search.ToUpper()));
            var totalCount = await AsyncExecuter.CountAsync(queryable);
            var lectures = new List<Lecture>();
            if (roles.Any(x=>x.Name.ToUpper() == RoleConsts.Admin.ToUpper())) lectures = await AsyncExecuter.ToListAsync(queryable.OrderBy(c => c.CreationTime).Skip((pageNumber - 1) * pageSize).Take(pageSize));
            else lectures = await AsyncExecuter.ToListAsync(queryable.Where(c => c.CreatorId == _currentUser.GetId()).OrderBy(c => c.CreationTime).Skip((pageNumber - 1) * pageSize).Take(pageSize));
            var lectureDtos = _mapper.Map<List<LectureDto>>(lectures);
            return new PagedResultDto<LectureDto>(totalCount, lectureDtos);
        }

        public async Task<ResponseApi<LectureDto>> CreateAsync(CreateUpdateLectureDto input)
        {
            var lecture = _mapper.Map<Lecture>(input);
            var result = await _lectureRepository.InsertAsync(lecture);
            await CreateQuizes(input, result.Id);
            var mediaItem = await _mediaItemManager.CreateAsync(new CreateUpdateMediaItemDto {IsImage=false ,RefId=result.Id,Url=input.PdfUrl });
            var dto = _mapper.Map<LectureDto>(result);
            if(mediaItem != null ) dto.PdfUrl = mediaItem.Url;
            return new ResponseApi<LectureDto> { Data = dto, Success = true, Message = "save succeess" };
        }

        public async Task<ResponseApi<LectureDto>> UpdateAsync(Guid id, CreateUpdateLectureDto input)
        {
            var lectureDB = await _lectureRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (lectureDB == null) return new ResponseApi<LectureDto> { Data = null, Success = false, Message = "Not found lecture" };
            var lecture = _mapper.Map(input, lectureDB);
            await _quizManager.DeletQuizesByLectureId(id);
            await CreateQuizes(input, id);
            var result = await _lectureRepository.UpdateAsync(lecture);
            var mediaItemDB = await _mediaItemManager.UpdateAsync(id ,new CreateUpdateMediaItemDto { IsImage=false , RefId = result.Id ,Url = input.PdfUrl});
            var dto = _mapper.Map<LectureDto>(result);
            return new ResponseApi<LectureDto> { Data = dto, Success = true, Message = "update succeess" };
        }

        public async Task<ResponseApi<bool>> DeleteAsync(Guid id)
        {
            var lecture = await _lectureRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (lecture == null) return new ResponseApi<bool> { Data = false, Success = false, Message = "Not found lecture" };
            await _mediaItemManager.DeleteAsync(id);
            await _quizManager.DeletQuizesByLectureId(id);
            await _lectureStudentRepository.DeleteManyAsync( await (await _lectureStudentRepository.GetQueryableAsync()).Where(x => x.LectureId == id).ToListAsync());
            await _lectureRepository.DeleteAsync(lecture);
            return new ResponseApi<bool> { Data = true, Success = true, Message = "delete succeess" };
        }

        public async Task CreateQuizes(CreateUpdateLectureDto input , Guid lectureId)
        {
            for (var i = 0; i < input.QuizCount; i++) await _quizManager.CreateAsync(new CreateUpdateQuizDto { CreaterId = _currentUser.GetId(), QuizTime = input.QuizTime, Title = input.Title + "Quiz " + i, Description = input.Content });
        }
        public async Task<ResponseApi<QuizDetailsDto>> GetQuizDetailsAsync(Guid quizId)
        {
            var queryable = await _quizRepository.GetQueryableAsync();

            var quiz = await queryable
                .Include(q => q.Questions)
                    .ThenInclude(q => q.QuestionAnswers)
                .Include(q => q.Questions)
                    .ThenInclude(q => q.QuestionType)
                .FirstOrDefaultAsync(q => q.Id == quizId);

            if (quiz == null)
                throw new UserFriendlyException("Quiz not found");
            var dto = new QuizDetailsDto
            {
                QuizId = quiz.Id,
                Title = quiz.Title,
                QuizTime = quiz.QuizTime,
                QuizTryCount = quiz.QuizTryCount,
                Questions = quiz.Questions.Select(q => new QuestionDetailesDto
                {
                    QuestionId = q.Id,
                    Title = q.Title,
                    Score = q.Score,
                    QuestionType = q.QuestionType?.Name ?? "",
                    QuestionTypeKey = q.QuestionType?.Key ?? 0, // إضافة الـ Key

                    Answers = q.QuestionAnswers.Select(a => new QuestionAnswerDetailesDto
                    {
                        AnswerId = a.Id,
                        Answer = a.Answer,
                        IsCorrect = a.IsCorrect
                    }).ToList()
                }).ToList()
            };
            return new ResponseApi <QuizDetailsDto> { Data = dto, Success = true, Message = "find success" };
        }
        

    }
}
