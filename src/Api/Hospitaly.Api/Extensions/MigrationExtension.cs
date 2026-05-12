using Hospitaly.Modules.Clinic.Infrastructure.Database;
using Hospitaly.Modules.Users.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Hospitaly.Api.Extensions;

internal static class MigrationExtension
{
    internal static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        
        ApplyMigration<UserDbContext>(scope);
        ApplyMigration<ClinikDbContext>(scope);
        
    }

    private static void ApplyMigration<TDbContext>(IServiceScope ss)
        where TDbContext : DbContext
    {
        using var context = ss.ServiceProvider.GetRequiredService<TDbContext>();
        context.Database.Migrate();
    }
}