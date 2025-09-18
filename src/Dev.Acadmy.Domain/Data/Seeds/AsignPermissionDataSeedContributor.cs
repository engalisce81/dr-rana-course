using Dev.Acadmy;
using Dev.Acadmy.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;

namespace Course.Data.Seeds
{
    public class AsignPermissionDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IIdentityRoleRepository _roleRepository;
        private readonly IIdentityUserRepository _userRepository;
        private readonly IdentityRoleManager _roleManager;
        private readonly IPermissionManager _permissionManager;
        public AsignPermissionDataSeedContributor(IIdentityRoleRepository roleRepository, IdentityRoleManager roleManager, IPermissionManager permissionManager)
        {
            _roleRepository = roleRepository;
            _roleManager = roleManager;
            _permissionManager = permissionManager;
        }
        public async Task SeedAsync(DataSeedContext context)
        {
        }

        public async Task StudentCoursePermission()
        {
            await _permissionManager.SetForRoleAsync(RoleConsts.Student, AcadmyPermissions.Courses.Create, true);
            
            await _permissionManager.SetForRoleAsync(RoleConsts.Teacher, AcadmyPermissions.Courses.Edit, true);
            await _permissionManager.SetForRoleAsync(RoleConsts.Teacher, AcadmyPermissions.Courses.View, true);
            await _permissionManager.SetForRoleAsync(RoleConsts.Teacher, AcadmyPermissions.Courses.Delete, true);
            await _permissionManager.SetForRoleAsync(RoleConsts.Teacher , AcadmyPermissions.Courses.Default, true);
        }

    }
}
