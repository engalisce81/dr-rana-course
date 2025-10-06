using Dev.Acadmy.Quizzes;
using Dev.Acadmy.Response;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dev.Acadmy.Lectures
{
    public class QuizAppService : ApplicationService
    {
        private readonly QuizManager _quizManager;
        public QuizAppService(QuizManager quizManager)
        {
            _quizManager = quizManager;
        }
        public async Task<ResponseApi<QuizDto>> GetAsync(Guid id) => await _quizManager.GetAsync(id);
        public async Task<PagedResultDto<QuizDto>> GetListAsync(int pageNumber, int pageSize, string? search) => await _quizManager.GetListAsync(pageNumber, pageSize, search);
        public async Task<ResponseApi<QuizDto>> CreateAsync(CreateUpdateQuizDto input) => await _quizManager.CreateAsync(input);
        public async Task<ResponseApi<QuizDto>> UpdateAsync(Guid id, CreateUpdateQuizDto input) => await _quizManager.UpdateAsync(id, input);
        public async Task<ResponseApi<bool>> DeleteAsync(Guid id) => await _quizManager.DeleteAsync(id);
    }
}
