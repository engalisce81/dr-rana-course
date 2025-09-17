using Volo.Abp.Modularity;

namespace Dev.Acadmy;

[DependsOn(
    typeof(AcadmyApplicationModule),
    typeof(AcadmyDomainTestModule)
)]
public class AcadmyApplicationTestModule : AbpModule
{

}
