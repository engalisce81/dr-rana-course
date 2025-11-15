using AutoMapper;
using Dev.Acadmy.LookUp;
using Dev.Acadmy.MediaItems;
using Dev.Acadmy.Quizzes;
using Dev.Acadmy.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace Dev.Acadmy.Questions
{
    public class QuestionManager:DomainService
    {
        private readonly IRepository<Question> _questionRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<QuestionType> _questionTypeRepository;
        private readonly IRepository<QuestionBank> _questionbankRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IIdentityUserRepository _userRepository;
        private readonly IRepository<Quiz,Guid> _quizRepository;
        private readonly QuestionAnswerManager _quetionAnswerManager;
        private readonly MediaItemManager _mediaItemManager;    
        
        public QuestionManager(MediaItemManager mediaItemManager, QuestionAnswerManager quetionAnswerManager, IRepository<Quiz, Guid> quizRepository, IIdentityUserRepository userRepository, ICurrentUser currentUser, IRepository<QuestionBank> questionbankRepository, IRepository<QuestionType> questionTypeRepository, IMapper mapper, IRepository<Question> questionRepository)
        {
            _mediaItemManager = mediaItemManager;
            _quetionAnswerManager = quetionAnswerManager;
            _quizRepository=quizRepository;
            _userRepository = userRepository;
            _currentUser = currentUser;
            _questionbankRepository = questionbankRepository;   
            _questionTypeRepository=questionTypeRepository;
            _questionRepository = questionRepository;
            _mapper = mapper;
        }

        public async Task<ResponseApi<QuestionDto>> GetAsync(Guid id)
        {
            var question =await ( await _questionRepository.GetQueryableAsync()).Include(x=>x.QuestionAnswers).FirstOrDefaultAsync(x => x.Id == id);
            if (question == null) return new ResponseApi<QuestionDto> { Data = null, Success = false, Message = "Not found Question" };
            var dto = _mapper.Map<QuestionDto>(question);
            var mediatItem = await _mediaItemManager.GetAsync(id);
            var questionAnswerDtos = _mapper.Map<List<QuestionAnswerDto>>(question.QuestionAnswers);
            dto.Answers = questionAnswerDtos;
            dto.LogoUrl = mediatItem?.Url?? string.Empty;
            return new ResponseApi<QuestionDto> { Data = dto, Success = true, Message = "find succeess" };
        }

        public async Task<PagedResultDto<QuestionDto>> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            var roles = await _userRepository.GetRolesAsync(_currentUser.GetId());
            var queryable = await _questionRepository.GetQueryableAsync();
            if (!string.IsNullOrWhiteSpace(search)) queryable = queryable.Include(x=>x.Quiz).Include(x=>x.QuestionAnswers).Include(x=>x.QuestionBank).Include(x=>x.QuestionType).Where(c => c.Title.Contains(search));
            var Questions = new List<Question>();
            if (roles.Any(x => x.Name.ToUpper() == RoleConsts.Admin.ToUpper()))    Questions = await AsyncExecuter.ToListAsync(queryable.Include(x => x.Quiz).Include(x => x.QuestionBank).Include(x => x.QuestionType).OrderByDescending(c => c.CreationTime).Skip((pageNumber - 1) * pageSize).Take(pageSize));
            else Questions = await AsyncExecuter.ToListAsync(queryable.Include(x => x.Quiz).Include(x => x.QuestionBank).Include(x => x.QuestionType).Where(x => x.CreatorId == _currentUser.GetId()).OrderByDescending(c => c.CreationTime).Skip((pageNumber - 1) * pageSize).Take(pageSize));
            var totalCount = await AsyncExecuter.CountAsync(queryable);
            var QuestionDtos = _mapper.Map<List<QuestionDto>>(Questions);
            return new PagedResultDto<QuestionDto>(totalCount, QuestionDtos);
        }

        public async Task<ResponseApi<QuestionDto>> CreateAsync(CreateUpdateQuestionDto input)
        {
            var Question = _mapper.Map<Question>(input);
            Question.QuizId = input.QuizId;  // ⭐ مهم جداً
            var result = await _questionRepository.InsertAsync(Question,autoSave:true);
            var mediaItem =  await _mediaItemManager.CreateAsync(new CreateUpdateMediaItemDto{RefId = result.Id,Url = input.LogoUrl,IsImage=true});
            foreach (var questionAnswer in input.Answers)
            {
                questionAnswer.QuestionId = result.Id;
                await _quetionAnswerManager.CreateAsync(questionAnswer);
            }
            var dto = _mapper.Map<QuestionDto>(result);
            dto.LogoUrl = input.LogoUrl;
            return new ResponseApi<QuestionDto> { Data = dto, Success = true, Message = "save succeess" };
        }

        public async Task<ResponseApi<QuestionDto>> UpdateAsync(Guid id, CreateUpdateQuestionDto input)
        {
            var QuestionDB = await _questionRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (QuestionDB == null) return new ResponseApi<QuestionDto> { Data = null, Success = false, Message = "Not found Question" };
            var Question = _mapper.Map(input, QuestionDB);
            var result = await _questionRepository.UpdateAsync(Question);
            var mediaItem = await _mediaItemManager.UpdateAsync(id,new CreateUpdateMediaItemDto { RefId = result.Id, Url = input.LogoUrl, IsImage = true });

            await _quetionAnswerManager.DeleteByQuestionId(id);
            foreach (var questionAnswer in input.Answers) 
            {
                questionAnswer.QuestionId = id;
                await _quetionAnswerManager.CreateAsync(questionAnswer); 
            }
            var dto = _mapper.Map<QuestionDto>(result);
            return new ResponseApi<QuestionDto> { Data = dto, Success = true, Message = "update succeess" };
        }

        public async Task<ResponseApi<bool>> DeleteAsync(Guid id)
        {
            var Question = await _questionRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (Question == null) return new ResponseApi<bool> { Data = false, Success = false, Message = "Not found Question" };
            await _questionRepository.DeleteAsync(Question);
            return new ResponseApi<bool> { Data = true, Success = true, Message = "delete succeess" };
        }

        public async Task<PagedResultDto<LookupDto>> GetListQuestionTypesAsync()
        {
            var questionTypes = await _questionTypeRepository.GetListAsync();
            var dtos = _mapper.Map<List<LookupDto>>(questionTypes);
            var totalCount = await _questionTypeRepository.GetCountAsync();
            return new PagedResultDto<LookupDto>(totalCount, dtos);
        }

        public async Task<PagedResultDto<LookupDto>> GetListQuestionesAsync()
        {
            var roles = await _userRepository.GetRolesAsync(_currentUser.GetId());
            var questiones = new List<Question>();
            if (roles.Any(x => x.Name.ToUpper() == RoleConsts.Admin.ToUpper())) questiones = await _questionRepository.GetListAsync();
            else questiones = (await _questionRepository.GetQueryableAsync()).Where(x => x.CreatorId == _currentUser.GetId()).ToList();
            var dtos = _mapper.Map<List<LookupDto>>(questiones);
            var totalCount = await _questionTypeRepository.GetCountAsync();
            return new PagedResultDto<LookupDto>(totalCount, dtos);
        }

        public async Task<PagedResultDto<LookupDto>> GetListQuestionBanksAsync()
        {
            var currentUser = await _userRepository.GetAsync(_currentUser.GetId());
            var questionBank = await (await _questionbankRepository.GetQueryableAsync()).Where(x => x.CreatorId == currentUser.Id).ToListAsync(); ;
            var dtos = _mapper.Map<List<LookupDto>>(questionBank);
            var totalCount = await _questionTypeRepository.GetCountAsync();
            return new PagedResultDto<LookupDto>(totalCount, dtos);
        }

        public async Task<PagedResultDto<LookupDto>> GetListQuizzesAsync(Guid lecId)
        {
            var quizzes = await (await _quizRepository.GetQueryableAsync()).Where(x => x.LectureId == lecId).ToListAsync();
            var dtos = _mapper.Map<List<LookupDto>>(quizzes);
            var totalCount = await _quizRepository.GetCountAsync();
            return new PagedResultDto<LookupDto>(totalCount, dtos);
        }
    }
}
