using Dev.Acadmy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace Course.Data.Seeds
{
    public class RoleDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IIdentityRoleRepository _roleManager;
        public RoleDataSeedContributor(IIdentityRoleRepository roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task SeedAsync(DataSeedContext context)
        {
            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole(Guid.NewGuid(), RoleConsts.Teacher){IsDefault=false,IsStatic=true},
                new IdentityRole(Guid.NewGuid(), RoleConsts.Student){IsDefault=false,IsStatic=true},
            };
            foreach (var role in roles)
            {
                var rolesDb = await _roleManager.GetListAsync();
                if (!rolesDb.Any(r => r.Name == role.Name))
                    await _roleManager.InsertAsync(role);
            }
        }
    }
}
