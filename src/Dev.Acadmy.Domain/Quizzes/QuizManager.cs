using AutoMapper;
using Dev.Acadmy.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Dev.Acadmy.Quizzes
{
    public class QuizManager :DomainService
    {
        private readonly IRepository<Quiz> _quizRepository;
        private readonly IMapper _mapper;
        public QuizManager(IMapper mapper, IRepository<Quiz> quizRepository)
        {
            _quizRepository = quizRepository;
            _mapper = mapper;
        }

        public async Task<ResponseApi<QuizDto>> GetAsync(Guid id)
        {
            var quiz = await _quizRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (quiz == null) return new ResponseApi<QuizDto> { Data = null, Success = false, Message = "Not found quiz" };
            var dto = _mapper.Map<QuizDto>(quiz);
            return new ResponseApi<QuizDto> { Data = dto, Success = true, Message = "find succeess" };
        }

        public async Task<PagedResultDto<QuizDto>> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            var queryable = await _quizRepository.GetQueryableAsync();
            if (!string.IsNullOrWhiteSpace(search)) queryable = queryable.Where(c => c.Description.Contains(search));
            var totalCount = await AsyncExecuter.CountAsync(queryable);
            var quizs = await AsyncExecuter.ToListAsync(queryable.OrderBy(c => c.CreationTime).Skip((pageNumber - 1) * pageSize).Take(pageSize));
            var quizDtos = _mapper.Map<List<QuizDto>>(quizs);
            return new PagedResultDto<QuizDto>(totalCount, quizDtos);
        }

        public async Task<ResponseApi<QuizDto>> CreateAsync(CreateUpdateQuizDto input)
        {
            var quiz = _mapper.Map<Quiz>(input);
            var result = await _quizRepository.InsertAsync(quiz);
            var dto = _mapper.Map<QuizDto>(result);
            return new ResponseApi<QuizDto> { Data = dto, Success = true, Message = "save succeess" };
        }

        public async Task<ResponseApi<QuizDto>> UpdateAsync(Guid id, CreateUpdateQuizDto input)
        {
            var quizDB = await _quizRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (quizDB == null) return new ResponseApi<QuizDto> { Data = null, Success = false, Message = "Not found quiz" };
            var quiz = _mapper.Map(input, quizDB);
            var result = await _quizRepository.UpdateAsync(quiz);
            var dto = _mapper.Map<QuizDto>(result);
            return new ResponseApi<QuizDto> { Data = dto, Success = true, Message = "update succeess" };
        }

        public async Task<ResponseApi<bool>> DeleteAsync(Guid id)
        {
            var quiz = await _quizRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (quiz == null) return new ResponseApi<bool> { Data = false, Success = false, Message = "Not found quiz" };
            await _quizRepository.DeleteAsync(quiz);
            return new ResponseApi<bool> { Data = true, Success = true, Message = "delete succeess" };
        }
    }
}
