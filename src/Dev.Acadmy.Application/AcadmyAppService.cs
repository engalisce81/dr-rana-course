using System;
using System.Collections.Generic;
using System.Text;
using Dev.Acadmy.Localization;
using Volo.Abp.Application.Services;

namespace Dev.Acadmy;

/* Inherit your application services from this class.
 */
public abstract class AcadmyAppService : ApplicationService
{
    protected AcadmyAppService()
    {
        LocalizationResource = typeof(AcadmyResource);
    }
}
