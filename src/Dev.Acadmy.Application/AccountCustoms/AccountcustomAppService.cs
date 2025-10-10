using Dev.Acadmy.Emails;
using Dev.Acadmy.LookUp;
using Dev.Acadmy.Response;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dev.Acadmy.AccountCustoms
{
    public class AccountcustomAppService : ApplicationService
    {
        private readonly AccountCustomManager _accountCustomManager;
        private readonly EmailManager _emailManager;
        public AccountcustomAppService(EmailManager emailManager, AccountCustomManager accountCustomManager) 
        {
            _emailManager = emailManager;
            _accountCustomManager = accountCustomManager;
        }
        [AllowAnonymous]
        public async Task<ResponseApi<LookupDto>> RegisterAsync(RegistercustomDto input) => await _accountCustomManager.RegisterAsync(input);
        [Authorize]
        public async Task<ResponseApi<LookupDto>> UpdateAsync(Guid userId, RegistercustomDto input) => await _accountCustomManager.UpdateAsync(userId, input);
        [Authorize]
        public async Task<ResponseApi<List<RegistercustomDto>>> GetStudentListAsync()=> await _accountCustomManager.GetStudentListAsync();
        [Authorize]
        public async Task<ResponseApi<RegistercustomDto>> GetAsync(Guid userId)=> await _accountCustomManager.GetAsync(userId);
        [AllowAnonymous]
        public async Task<PagedResultDto<LookupAccountDto>> GetAccountTypes() => await _accountCustomManager.GetAccountTypes();
        [AllowAnonymous]
        public async Task<ResponseApi<EmailDto>> SendNotificationToEmailAsync(CreateEmailDto input) => await _emailManager.SendNotificationToEmailAsync(input);
        [AllowAnonymous]
        public async Task<ResponseApi<EmailDto>> CheckCodeAsync(UpdateEmailDto input) => await _emailManager.CheckCodeAsync(input);
    }
}
