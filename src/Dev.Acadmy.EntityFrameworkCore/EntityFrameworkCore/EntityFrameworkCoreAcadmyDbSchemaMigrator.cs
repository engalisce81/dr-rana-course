using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Dev.Acadmy.Data;
using Volo.Abp.DependencyInjection;

namespace Dev.Acadmy.EntityFrameworkCore;

public class EntityFrameworkCoreAcadmyDbSchemaMigrator
    : IAcadmyDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreAcadmyDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the AcadmyDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<AcadmyDbContext>()
            .Database
            .MigrateAsync();
    }
}
