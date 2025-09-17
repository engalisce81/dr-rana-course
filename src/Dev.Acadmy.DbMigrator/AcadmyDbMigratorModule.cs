using Dev.Acadmy.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Dev.Acadmy.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AcadmyEntityFrameworkCoreModule),
    typeof(AcadmyApplicationContractsModule)
    )]
public class AcadmyDbMigratorModule : AbpModule
{
}
