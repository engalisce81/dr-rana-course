using Volo.Abp.Settings;

namespace Dev.Acadmy.Settings;

public class AcadmySettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(AcadmySettings.MySetting1));
    }
}
