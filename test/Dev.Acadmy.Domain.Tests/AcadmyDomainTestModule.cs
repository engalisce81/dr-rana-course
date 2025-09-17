using Volo.Abp.Modularity;

namespace Dev.Acadmy;

[DependsOn(
    typeof(AcadmyDomainModule),
    typeof(AcadmyTestBaseModule)
)]
public class AcadmyDomainTestModule : AbpModule
{

}
