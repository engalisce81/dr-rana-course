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
    public class QuestionBankAppService :ApplicationService
    {
        private readonly QuestionBankManager _questionbankManager;
        public QuestionBankAppService(QuestionBankManager questionbankManager)
        {
            _questionbankManager = questionbankManager;
        }

        [Authorize(AcadmyPermissions.QuestionBanks.View)]
        public async Task<ResponseApi<QuestionBankDto>> GetAsync(Guid id) => await _questionbankManager.GetAsync(id);
        [Authorize(AcadmyPermissions.QuestionBanks.View)]
        public async Task<PagedResultDto<QuestionBankDto>> GetListAsync(int pageNumber, int pageSize, string? search) => await _questionbankManager.GetListAsync(pageNumber, pageSize, search);
        [Authorize(AcadmyPermissions.QuestionBanks.Create)]
        public async Task<ResponseApi<QuestionBankDto>> CreateAsync(CreateUpdateQuestionBankDto input) => await _questionbankManager.CreateAsync(input);
        [Authorize(AcadmyPermissions.QuestionBanks.Edit)]
        public async Task<ResponseApi<QuestionBankDto>> UpdateAsync(Guid id, CreateUpdateQuestionBankDto input) => await _questionbankManager.UpdateAsync(id, input);
        [Authorize(AcadmyPermissions.QuestionBanks.Delete)]
        public async Task DeleteAsync(Guid id) => await _questionbankManager.DeleteAsync(id);
        [Authorize]
        public async Task<PagedResultDto<LookupDto>> GetListMyBankAsync() => await _questionbankManager.GetListMyBankAsync();

    }
}
