using Dev.Acadmy.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Dev.Acadmy.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class AcadmyController : AbpControllerBase
{
    protected AcadmyController()
    {
        LocalizationResource = typeof(AcadmyResource);
    }
}
