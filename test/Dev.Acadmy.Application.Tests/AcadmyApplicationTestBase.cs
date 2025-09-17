using Volo.Abp.Modularity;

namespace Dev.Acadmy;

public abstract class AcadmyApplicationTestBase<TStartupModule> : AcadmyTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
