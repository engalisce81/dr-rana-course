using Microsoft.Extensions.Localization;
using Dev.Acadmy.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Dev.Acadmy;

[Dependency(ReplaceServices = true)]
public class AcadmyBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<AcadmyResource> _localizer;

    public AcadmyBrandingProvider(IStringLocalizer<AcadmyResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
