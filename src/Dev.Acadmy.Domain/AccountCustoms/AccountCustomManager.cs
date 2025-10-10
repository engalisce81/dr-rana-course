
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
            if(input.GradeLevelId !=null)  await _gradeLevelRepository.GetAsync((Guid)input.GradeLevelId);
        }


        public async Task<ResponseApi<LookupDto>> UpdateAsync(Guid userId, RegistercustomDto input)
        {
            // 🟢 1. التحقق من صحة البيانات
            await CheckEntity(input);

            // 🟢 2. الحصول على المستخدم الحالي
            var user = await _userRepository.FindAsync(userId);
            if (user == null)
                throw new UserFriendlyException("User Not Found");

            // 🟢 3. التحقق من عدم وجود بريد إلكتروني أو اسم مستخدم مكرر (لبقية المستخدمين)
            var existingUser = await _userRepository.FindByNormalizedEmailAsync(input.UserName.ToUpper());
            if (existingUser != null && existingUser.Id != userId)
                throw new UserFriendlyException("The Email or User Name Already Exist");

            // 🟢 4. التحقق من نوع الحساب
            var accountType = await _accountTypeRepository.FirstOrDefaultAsync(x => x.Key == input.AccountTypeKey);
            if (accountType == null)
                throw new UserFriendlyException("Account Type Not Found");

            // 🟢 5. تحديث الخصائص الأساسية
            await _userManager.SetUserNameAsync(user, input.UserName);
            await _userManager.SetEmailAsync(user, input.UserName);
            user.Name = input.FullName;
            user.SetProperty(SetPropConsts.AccountTypeId, accountType.Id);
            user.SetProperty(SetPropConsts.CollegeId, input.CollegeId);
            user.SetProperty(SetPropConsts.Gender, input.Gender);
            user.SetProperty(SetPropConsts.UniversityId, input.UniversityId);
            // 🟢 6. تحديث خصائص إضافية حسب نوع الحساب
            if (accountType.Key == (int)AccountTypeKey.Student)
            {
                user.SetProperty(SetPropConsts.GradeLevelId, input.GradeLevelId);
                user.SetProperty(SetPropConsts.StudentMobileIP, input.StudentMobileIP);
                var currentTerm = await _termRepository.FirstOrDefaultAsync(x => x.IsActive);
                if (currentTerm != null)  user.SetProperty(SetPropConsts.TermId, currentTerm.Id);
            }
            // 🟢 7. تحديث حالة المستخدم
            user.SetIsActive(true);

            // 🟢 8. تحديث المستخدم في قاعدة البيانات
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                throw new UserFriendlyException(updateResult.Errors.FirstOrDefault()?.Description ?? "Failed To Update User");

            // 🟢 9. التحقق من الـ Role الحالي وتحديثه لو تغيّر نوع الحساب
            var userRoles = await _userManager.GetRolesAsync(user);
            var currentRoleName = userRoles.FirstOrDefault();

            var newRole = await GetRole(accountType.Id);
            if (newRole == null)
                throw new UserFriendlyException("Role Not Found");

            if (currentRoleName != newRole.Name)
            {
                if (currentRoleName != null)
                    await _userManager.RemoveFromRoleAsync(user, currentRoleName);
                await _userManager.AddToRoleAsync(user, newRole.Name);
            }

            // 🟢 10. إرجاع النتيجة
            var lookupDto = new LookupDto { Id = user.Id, Name = user.Name };
            return new ResponseApi<LookupDto> { Data = lookupDto, Success = true, Message = "User Updated Successfully" };
        }

        public async Task<ResponseApi<RegistercustomDto>> GetAsync(Guid userId)
        {
            // 🟢 1. الحصول على المستخدم
            var user = await _userRepository.FindAsync(userId);
            if (user == null)
                throw new UserFriendlyException("User Not Found");

            // 🟢 2. قراءة نوع الحساب
            var accountTypeId = user.GetProperty<Guid?>(SetPropConsts.AccountTypeId);
            var accountType = accountTypeId.HasValue
                ? await _accountTypeRepository.FindAsync(accountTypeId.Value)
                : null;

            // 🟢 3. تعبئة البيانات في DTO
            var dto = new RegistercustomDto
            {
                Id = user.Id,
                FullName = user.Name,
                UserName = user.UserName,
                AccountTypeKey = accountType?.Key ?? 0,
                CollegeId = user.GetProperty<Guid>(SetPropConsts.CollegeId),
                UniversityId = user.GetProperty<Guid>(SetPropConsts.UniversityId),
                Gender = user.GetProperty<bool>(SetPropConsts.Gender),
                GradeLevelId = user.GetProperty<Guid>(SetPropConsts.GradeLevelId),
                StudentMobileIP = user.GetProperty<string>(SetPropConsts.StudentMobileIP)
            };

            // 🟢 4. إرجاع النتيجة
            return new ResponseApi<RegistercustomDto>
            {
                Data = dto,
                Success = true,
                Message = "User Retrieved Successfully"
            };
        }

        public async Task<ResponseApi<List<RegistercustomDto>>> GetStudentListAsync()
        {
            // 🟢 1. جلب جميع المستخدمين
            var users = await _userRepository.GetListAsync();

            var resultList = new List<RegistercustomDto>();

            foreach (var user in users)
            {
                // 🟢 2. جلب نوع الحساب من ExtraProperties
                var accountTypeId = user.GetProperty<Guid?>(SetPropConsts.AccountTypeId);
                if (!accountTypeId.HasValue)
                    continue;

                var accountType = await _accountTypeRepository.FindAsync(accountTypeId.Value);
                if (accountType == null)
                    continue;

                // 🟢 3. فلترة الطلاب فقط
                if (accountType.Key != (int)AccountTypeKey.Student)
                    continue;

                // 🟢 4. بناء الـ DTO
                var dto = new RegistercustomDto
                {
                    Id = user.Id,
                    FullName = user.Name,
                    UserName = user.UserName,
                    AccountTypeKey = accountType.Key,
                    CollegeId = user.GetProperty<Guid>(SetPropConsts.CollegeId),
                    UniversityId = user.GetProperty<Guid>(SetPropConsts.UniversityId),
                    Gender = user.GetProperty<bool>(SetPropConsts.Gender),
                    GradeLevelId = user.GetProperty<Guid?>(SetPropConsts.GradeLevelId),
                    StudentMobileIP = user.GetProperty<string>(SetPropConsts.StudentMobileIP)
                };

                resultList.Add(dto);
            }

            // 🟢 5. إرجاع النتيجة
            return new ResponseApi<List<RegistercustomDto>>
            {
                Data = resultList,
                Success = true,
                Message = "Student Accounts Retrieved Successfully"
            };
        }


    }
}
