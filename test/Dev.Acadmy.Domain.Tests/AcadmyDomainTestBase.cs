using Volo.Abp.Modularity;

namespace Dev.Acadmy;

/* Inherit from this class for your domain layer tests. */
public abstract class AcadmyDomainTestBase<TStartupModule> : AcadmyTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
