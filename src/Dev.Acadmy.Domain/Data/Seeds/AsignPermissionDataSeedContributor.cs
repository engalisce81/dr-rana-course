using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Course.Data.Seeds
{
    public class AsignPermissionDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        //public AsignPermissionDataSeedContributor
        public async Task SeedAsync(DataSeedContext context)
        {
        }
    }
}
