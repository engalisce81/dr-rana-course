using Dev.Acadmy.Samples;
using Xunit;

namespace Dev.Acadmy.EntityFrameworkCore.Domains;

[Collection(AcadmyTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<AcadmyEntityFrameworkCoreTestModule>
{

}
