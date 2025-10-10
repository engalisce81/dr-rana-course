using AutoMapper;
using Dev.Acadmy.Exams;
using Dev.Acadmy.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Dev.Acadmy.Questions
{
    public class QuestionBankManager:DomainService
    {
        private readonly IRepository<QuestionBank> _questionbankRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<Exam, Guid> _examRepositroy;
        public QuestionBankManager(IRepository<Exam, Guid> examRepositroy, IMapper mapper, IRepository<QuestionBank> questionbankRepository)
        {
            _examRepositroy = examRepositroy;
            _questionbankRepository = questionbankRepository;
            _mapper = mapper;
        }

        public async Task<QuestionBank?> GetByCourse(Guid courseId) => await _questionbankRepository.FirstOrDefaultAsync(x => x.CourseId == courseId);
        public async Task<ResponseApi<QuestionBankDto>> GetAsync(Guid id)
        {
            var questionbank = await _questionbankRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (questionbank == null) return new ResponseApi<QuestionBankDto> { Data = null, Success = false, Message = "Not found questionbank" };
            var dto = _mapper.Map<QuestionBankDto>(questionbank);
            return new ResponseApi<QuestionBankDto> { Data = dto, Success = true, Message = "find succeess" };
        }

        public async Task<PagedResultDto<QuestionBankDto>> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            var queryable = await _questionbankRepository.GetQueryableAsync();
            if (!string.IsNullOrWhiteSpace(search)) queryable = queryable.Where(c => c.Name.Contains(search));
            var questionbanks = await AsyncExecuter.ToListAsync(queryable.OrderBy(c => c.CreationTime).Skip((pageNumber - 1) * pageSize).Take(pageSize));
            var questionbankDtos = _mapper.Map<List<QuestionBankDto>>(questionbanks);
            var totalCount = await AsyncExecuter.CountAsync(queryable);

            return new PagedResultDto<QuestionBankDto>(totalCount, questionbankDtos);
        }

        public async Task<ResponseApi<QuestionBankDto>> CreateAsync(CreateUpdateQuestionBankDto input)
        {
            var questionbank = _mapper.Map<QuestionBank>(input);
            
            var result = await _questionbankRepository.InsertAsync(questionbank,autoSave:true);
            await  _examRepositroy.InsertAsync( new Exam { CourseId = input.CourseId ,Name = input.Name+" Exam" ,IsActive=false ,TimeExam = 0 } ,autoSave:true);
            var dto = _mapper.Map<QuestionBankDto>(result);
            return new ResponseApi<QuestionBankDto> { Data = dto, Success = true, Message = "save succeess" };
        }

        public async Task<ResponseApi<QuestionBankDto>> UpdateAsync(Guid id, CreateUpdateQuestionBankDto input)
        {
            var questionbankDB = await _questionbankRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (questionbankDB == null) return new ResponseApi<QuestionBankDto> { Data = null, Success = false, Message = "Not found questionbank" };
            var questionbank = _mapper.Map(input, questionbankDB);
            var result = await _questionbankRepository.UpdateAsync(questionbank);
            var dto = _mapper.Map<QuestionBankDto>(result);
            return new ResponseApi<QuestionBankDto> { Data = dto, Success = true, Message = "update succeess" };
        }

        public async Task<ResponseApi<bool>> DeleteAsync(Guid id)
        {
            var questionbank = await _questionbankRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (questionbank == null) return new ResponseApi<bool> { Data = false, Success = false, Message = "Not found questionbank" };
            await _questionbankRepository.DeleteAsync(questionbank);
            return new ResponseApi<bool> { Data = true, Success = true, Message = "delete succeess" };
        }
    }
}
