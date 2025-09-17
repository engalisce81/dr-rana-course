using System.Threading.Tasks;

namespace Dev.Acadmy.Data;

public interface IAcadmyDbSchemaMigrator
{
    Task MigrateAsync();
}
