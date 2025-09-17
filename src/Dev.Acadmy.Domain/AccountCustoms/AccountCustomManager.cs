
using Dev.Acadmy.AccountTypes;
using Dev.Acadmy.LookUp;
using Dev.Acadmy.Response;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;

namespace Dev.Acadmy.AccountCustoms
{
    public class AccountCustomManager:DomainService
    {
        private readonly IIdentityUserRepository _userRepository;
        private readonly IdentityUserManager _userManager;
        private readonly IRepository<AccountType, Guid> _accountTypeRepository;
        private readonly IIdentityRoleRepository _roleRepository;
        public AccountCustomManager(IIdentityRoleRepository roleRepository, IIdentityUserRepository userRepository , IRepository<AccountType, Guid> accountTypeRepository , IdentityUserManager userManager) 
        {
            _roleRepository = roleRepository;
            _userManager = userManager;
            _accountTypeRepository = accountTypeRepository;
            _userRepository = userRepository;
        }

        public async Task<PagedResultDto<LookupAccountDto>> GetAccountTypes()
        {
            var dtos = (await _accountTypeRepository.GetQueryableAsync()).Where(x=>x.Key !=(int) AccountTypeKey.Admin).Select(x=> new LookupAccountDto { Id = x.Id ,Key =x.Key,Name = x.Name}).ToList();
            var totalCount = await _accountTypeRepository.GetCountAsync();
            return new PagedResultDto<LookupAccountDto>(totalCount, dtos);
        }
        
        public async Task<ResponseApi<LookupDto>> RegisterAsync(RegistercustomDto input)
        {
            if (await _userRepository.FindByNormalizedEmailAsync(input.UserName.ToUpper()) != null ) return new ResponseApi<LookupDto> { Data= null ,Success=false ,Message= "The Email or User Name Already Exist" };
            var user = new IdentityUser(Guid.NewGuid(), input.UserName, input.UserName);
            var role = await GetRole(input.AccountTypeId);
            user.SetProperty(SetPropConsts.AccountTypeId , input.AccountTypeId);
            user.Name = input.FullName;
            user.SetProperty(SetPropConsts.CollegeId, input.CollegeId);
            user.SetProperty(SetPropConsts.Gender,input.Gender);
            user.SetIsActive(true);
            var result = await _userManager.CreateAsync(user, input.Password);
            if (result.Succeeded)
            {
                if (role != null)
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                    if (!result.Succeeded) return new ResponseApi<LookupDto> { Data = null, Success = false, Message = result.Errors.FirstOrDefault()?.Description ?? "" };
                    else
                    {
                        var lookupDto = new LookupDto { Id = user.Id, Name = input.FullName };
                        return new ResponseApi<LookupDto> { Data = lookupDto, Success = true, Message = "Register Success" };
                    }
                }
                else return new ResponseApi<LookupDto> { Data = null, Success = false, Message = "Role Not Found" };
            }
            else  return new ResponseApi<LookupDto> { Data = null, Success = false, Message = "Can't Create This Account" };
        }
        private async Task<IdentityRole> GetRole(Guid accountTypeId)
        {
            var accountType = await _accountTypeRepository.GetAsync(accountTypeId);
            if (accountType == null) new UserFriendlyException($"Not Found Account Type With Id{accountTypeId}");
            if(accountType.Key ==(int) AccountTypeKey.Teacher) return await _roleRepository.FindByNormalizedNameAsync(RoleConsts.Teacher.ToUpperInvariant());
            else return await _roleRepository.FindByNormalizedNameAsync(RoleConsts.Student.ToUpperInvariant());
        }

    }
}
