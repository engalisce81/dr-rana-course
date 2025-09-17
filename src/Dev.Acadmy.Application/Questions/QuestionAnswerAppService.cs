using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Dev.Acadmy.Questions;
using Dev.Acadmy.Permissions;
using Dev.Acadmy.Response;

namespace QuestionAnswer.Questions
{
    public class QuestionAnswerAppService :ApplicationService
    {
        private readonly QuestionAnswerManager _QuestionAnswerManager;
        public QuestionAnswerAppService(QuestionAnswerManager QuestionAnswerManager)
        {
            _QuestionAnswerManager = QuestionAnswerManager;
        }
        [Authorize(AcadmyPermissions.QuestionAnswers.View)]
        public async Task<ResponseApi<QuestionAnswerDto>> GetAsync(Guid id) => await _QuestionAnswerManager.GetAsync(id);
        [Authorize(AcadmyPermissions.QuestionAnswers.View)]
        public async Task<PagedResultDto<QuestionAnswerDto>> GetListAsync(int pageNumber, int pageSize, string? search) => await _QuestionAnswerManager.GetListAsync(pageNumber, pageSize, search);
        [Authorize(AcadmyPermissions.QuestionAnswers.Create)]
        public async Task<ResponseApi<QuestionAnswerDto>> CreateAsync(CreateUpdateQuestionAnswerDto input) => await _QuestionAnswerManager.CreateAsync(input);
        [Authorize(AcadmyPermissions.QuestionAnswers.Edit)]
        public async Task<ResponseApi<QuestionAnswerDto>> UpdateAsync(Guid id, CreateUpdateQuestionAnswerDto input) => await _QuestionAnswerManager.UpdateAsync(id, input);
        [Authorize(AcadmyPermissions.QuestionAnswers.Delete)]
        public async Task DeleteAsync(Guid id) => await _QuestionAnswerManager.DeleteAsync(id);

    }
}
