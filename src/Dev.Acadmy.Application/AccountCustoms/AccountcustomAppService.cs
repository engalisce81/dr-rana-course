using Dev.Acadmy.LookUp;
using Dev.Acadmy.Response;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dev.Acadmy.AccountCustoms
{
    public class AccountcustomAppService : ApplicationService
    {
        private readonly AccountCustomManager _accountCustomManager;
        public AccountcustomAppService(AccountCustomManager accountCustomManager) 
        {
            _accountCustomManager = accountCustomManager;
        }
        [AllowAnonymous]
        public async Task<ResponseApi<LookupDto>> RegisterAsync(RegistercustomDto input) => await _accountCustomManager.RegisterAsync(input);
        [AllowAnonymous]
        public async Task<PagedResultDto<LookupAccountDto>> GetAccountTypes() => await _accountCustomManager.GetAccountTypes();

    }
}
