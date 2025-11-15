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
using Dev.Acadmy.Response;

namespace Dev.Acadmy.Questions
{
    public class QuestionAnswerManager:DomainService
    {
        private readonly IRepository<QuestionAnswer> _QuestionAnswerRepository;
        private readonly IMapper _mapper;
        private readonly IIdentityUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;
        public QuestionAnswerManager(ICurrentUser currentUser, IIdentityUserRepository userRepository, IMapper mapper, IRepository<QuestionAnswer> QuestionAnswerRepository)
        {
            _currentUser = currentUser;
            _userRepository = userRepository;
            _QuestionAnswerRepository = QuestionAnswerRepository;
            _mapper = mapper;
        }

        public async Task<ResponseApi<QuestionAnswerDto>> GetAsync(Guid id)
        {
            var QuestionAnswer = await _QuestionAnswerRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (QuestionAnswer == null) return new ResponseApi<QuestionAnswerDto> { Data = null, Success = false, Message = "Not found QuestionAnswer" };
            var dto = _mapper.Map<QuestionAnswerDto>(QuestionAnswer);
            return new ResponseApi<QuestionAnswerDto> { Data = dto, Success = true, Message = "find succeess" };
        }

        public async Task<PagedResultDto<QuestionAnswerDto>> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            var roles = await _userRepository.GetRolesAsync(_currentUser.GetId());
            var queryable = await _QuestionAnswerRepository.GetQueryableAsync();
            if (!string.IsNullOrWhiteSpace(search)) queryable = queryable.Where(c => c.Answer.Contains(search));
            var totalCount = await AsyncExecuter.CountAsync(queryable);
            var questionAnswers = new List<QuestionAnswer>();    
            if(roles.Any(x=>x.Name.ToUpper()==RoleConsts.Admin.ToUpper() ) ) questionAnswers = await AsyncExecuter.ToListAsync(queryable.Include(x => x.Question).OrderByDescending(c => c.CreationTime).Skip((pageNumber - 1) * pageSize).Take(pageSize));
            else questionAnswers = await AsyncExecuter.ToListAsync(queryable.Include(x=>x.Question).Where(x=>x.CreatorId == _currentUser.GetId()).OrderByDescending(c => c.CreationTime).Skip((pageNumber - 1) * pageSize).Take(pageSize));
            var questionAnswerDtos = _mapper.Map<List<QuestionAnswerDto>>(questionAnswers);
            return new PagedResultDto<QuestionAnswerDto>(totalCount, questionAnswerDtos);
        }

        public async Task<ResponseApi<QuestionAnswerDto>> CreateAsync(CreateUpdateQuestionAnswerDto input)
        {
            var QuestionAnswer = _mapper.Map<QuestionAnswer>(input);
            var result = await _QuestionAnswerRepository.InsertAsync(QuestionAnswer,autoSave:true);
            var dto = _mapper.Map<QuestionAnswerDto>(result);
            return new ResponseApi<QuestionAnswerDto> { Data = dto, Success = true, Message = "save succeess" };
        }

        public async Task<ResponseApi<QuestionAnswerDto>> UpdateAsync(Guid id, CreateUpdateQuestionAnswerDto input)
        {
            var QuestionAnswerDB = await _QuestionAnswerRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (QuestionAnswerDB == null) return new ResponseApi<QuestionAnswerDto> { Data = null, Success = false, Message = "Not found QuestionAnswer" };
            var QuestionAnswer = _mapper.Map(input, QuestionAnswerDB);
            var result = await _QuestionAnswerRepository.UpdateAsync(QuestionAnswer);
            var dto = _mapper.Map<QuestionAnswerDto>(result);
            return new ResponseApi<QuestionAnswerDto> { Data = dto, Success = true, Message = "update succeess" };
        }

        public async Task<ResponseApi<bool>> DeleteAsync(Guid id)
        {
            var QuestionAnswer = await _QuestionAnswerRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (QuestionAnswer == null) return new ResponseApi<bool> { Data = false, Success = false, Message = "Not found QuestionAnswer" };
            await _QuestionAnswerRepository.DeleteAsync(QuestionAnswer);
            return new ResponseApi<bool> { Data = true, Success = true, Message = "delete succeess" };
        }

        public async Task DeleteByQuestionId(Guid questionId)
        {
            var answers = await (await _QuestionAnswerRepository.GetQueryableAsync()).Where(x => x.QuestionId == questionId).ToListAsync();
            await _QuestionAnswerRepository.DeleteManyAsync(answers);
        }
    }
}
