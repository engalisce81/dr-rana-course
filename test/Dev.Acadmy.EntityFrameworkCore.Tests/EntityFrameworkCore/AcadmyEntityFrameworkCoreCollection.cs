using Xunit;

namespace Dev.Acadmy.EntityFrameworkCore;

[CollectionDefinition(AcadmyTestConsts.CollectionDefinitionName)]
public class AcadmyEntityFrameworkCoreCollection : ICollectionFixture<AcadmyEntityFrameworkCoreFixture>
{

}
