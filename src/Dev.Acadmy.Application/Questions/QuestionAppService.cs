using Dev.Acadmy.LookUp;
using Dev.Acadmy.Permissions;
using Dev.Acadmy.Response;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dev.Acadmy.Questions
{
    public class QuestionAppService : ApplicationService
    {
        private readonly QuestionManager _questionManager;
        public QuestionAppService(QuestionManager questionManager)
        {
            _questionManager = questionManager;
        }

        [Authorize(AcadmyPermissions.Questions.View)]
        public async Task<ResponseApi<QuestionDto>> GetAsync(Guid id) => await _questionManager.GetAsync(id);
        [Authorize(AcadmyPermissions.Questions.View)]
        public async Task<PagedResultDto<QuestionDto>> GetListAsync(int pageNumber, int pageSize, string? search) => await _questionManager.GetListAsync(pageNumber, pageSize, search);
        [Authorize(AcadmyPermissions.Questions.Create)]
        public async Task<ResponseApi<QuestionDto>> CreateAsync(CreateUpdateQuestionDto input) => await _questionManager.CreateAsync(input);
        [Authorize(AcadmyPermissions.Questions.Edit)]
        public async Task<ResponseApi<QuestionDto>> UpdateAsync(Guid id, CreateUpdateQuestionDto input) => await _questionManager.UpdateAsync(id, input);
        [Authorize(AcadmyPermissions.Questions.Delete)]
        public async Task DeleteAsync(Guid id) => await _questionManager.DeleteAsync(id);
        [AllowAnonymous]
        public async Task<PagedResultDto<LookupDto>> GetListQuestionTypesAsync() => await _questionManager.GetListQuestionTypesAsync();
        [Authorize]
        public async Task<PagedResultDto<LookupDto>> GetListQuestionesAsync() => await _questionManager.GetListQuestionesAsync();
        [Authorize]
        public async Task<PagedResultDto<LookupDto>> GetListQuestionBanksAsync() => await _questionManager.GetListQuestionBanksAsync();
        [Authorize]
        public async Task<PagedResultDto<LookupDto>> GetListQuizzesAsync(Guid lecId) => await _questionManager.GetListQuizzesAsync(lecId);
    }
}
