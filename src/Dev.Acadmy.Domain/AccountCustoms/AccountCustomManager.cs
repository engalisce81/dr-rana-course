
using Dev.Acadmy.AccountTypes;
using Dev.Acadmy.LookUp;
using Dev.Acadmy.Response;
using Dev.Acadmy.Universites;
using System;
using System.Collections.Generic;
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
        private readonly IRepository<Subject, Guid> _subjectRepository;
        private readonly IRepository<College, Guid> _collegeRepository;
        private readonly IRepository<University ,Guid> _universityRepository;
        private readonly IRepository<GradeLevel , Guid> _gradeLevelRepository;
        private readonly IRepository<Term ,Guid > _termRepository;
        public AccountCustomManager(IRepository<Term, Guid> termRepository, IRepository<GradeLevel, Guid> gradeLevelRepository, IRepository<University, Guid> universityRepository, IRepository<College, Guid> collegeRepository, IRepository<Subject, Guid> subjectRepository, IIdentityRoleRepository roleRepository, IIdentityUserRepository userRepository , IRepository<AccountType, Guid> accountTypeRepository , IdentityUserManager userManager) 
        {
            _termRepository = termRepository;
            _gradeLevelRepository = gradeLevelRepository;
            _universityRepository = universityRepository;
            _collegeRepository = collegeRepository;
            _subjectRepository = subjectRepository;
            _roleRepository = roleRepository;
            _userManager = userManager;
            _accountTypeRepository = accountTypeRepository;
            _userRepository = userRepository;
        }

        public async Task ResetPasswordAsync(Guid userId, string newPassword)
        {
            var user = await _userManager.GetByIdAsync(userId);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (!result.Succeeded)
            {
                throw new UserFriendlyException(
                    string.Join(", ", result.Errors.Select(e => e.Description))
                );
            }
        }
        public async Task<PagedResultDto<LookupAccountDto>> GetAccountTypes()
        {
            var dtos = (await _accountTypeRepository.GetQueryableAsync()).Where(x=>x.Key !=(int) AccountTypeKey.Admin).Select(x=> new LookupAccountDto { Id = x.Id ,Key =x.Key,Name = x.Name}).ToList();
            var totalCount = await _accountTypeRepository.GetCountAsync();
            return new PagedResultDto<LookupAccountDto>(totalCount, dtos);
        }
        
        public async Task<ResponseApi<LookupDto>> RegisterAsync(RegistercustomDto input)
        {
            await CheckEntity(input);
            if (await _userRepository.FindByNormalizedEmailAsync(input.UserName.ToUpper()) != null) throw new UserFriendlyException("The Email or User Name Already Exist");
            var user = new IdentityUser(Guid.NewGuid(), input.UserName, input.UserName);
            var accountType = await _accountTypeRepository.FirstOrDefaultAsync(x=>x.Key == input.AccountTypeKey);   
            if(accountType == null)  throw new UserFriendlyException("Account Type Not Found"); 
            var role = await GetRole(accountType.Id);
            user.SetProperty(SetPropConsts.AccountTypeId , accountType.Id);
            user.Name = input.FullName;
            user.SetProperty(SetPropConsts.CollegeId, input.CollegeId);
            user.SetProperty(SetPropConsts.Gender,input.Gender);
            user.SetProperty(SetPropConsts.UniversityId, input.UniversityId);
            user.SetProperty(SetPropConsts.PhoneNumber, input.PhoneNumber);

            if (accountType.Key == (int)AccountTypeKey.Student)
            {
                user.SetProperty(SetPropConsts.GradeLevelId, input.GradeLevelId);
                user.SetProperty(SetPropConsts.StudentMobileIP, input.StudentMobileIP);
                var currentTerm = await _termRepository.FirstOrDefaultAsync(x => x.IsActive);
                if(currentTerm != null)user.SetProperty(SetPropConsts.TermId, currentTerm.Id);

            }
            else  accountType.Key = (int)AccountTypeKey.Teacher;
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
                else throw new UserFriendlyException("Role Not Found");
            }
            else throw new UserFriendlyException("Can't Create This Account");
        }
        private async Task<IdentityRole> GetRole(Guid accountTypeId)
        {
            var accountType = await _accountTypeRepository.GetAsync(accountTypeId);
            if (accountType == null) new UserFriendlyException($"Not Found Account Type With Id{accountTypeId}");
            if(accountType.Key ==(int) AccountTypeKey.Teacher) return await _roleRepository.FindByNormalizedNameAsync(RoleConsts.Teacher.ToUpperInvariant());
            else return await _roleRepository.FindByNormalizedNameAsync(RoleConsts.Student.ToUpperInvariant());
        }

        private async Task CheckEntity(RegistercustomDto input)
        {
            var university = await _universityRepository.GetAsync(input.UniversityId);
            var college = await _collegeRepository.GetAsync(input.CollegeId);
            if(input.GradeLevelId !=null )  await _gradeLevelRepository.GetAsync((Guid)input.GradeLevelId);
        }


        
    

       


    }
}
