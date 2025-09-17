using Dev.Acadmy.Samples;
using Xunit;

namespace Dev.Acadmy.EntityFrameworkCore.Applications;

[Collection(AcadmyTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<AcadmyEntityFrameworkCoreTestModule>
{

}
