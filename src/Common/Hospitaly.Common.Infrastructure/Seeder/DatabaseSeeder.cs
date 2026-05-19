using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Hospitaly.Common.Infrastructure.Seeder;

public class DatabaseSeeder(IServiceProvider serviceProvider , ILogger<DatabaseSeeder> logger)
{
    public async Task SeedAllAsync(CancellationToken cancellationToken = default)
    {
        await using AsyncServiceScope scope = serviceProvider.CreateAsyncScope();
        IEnumerable<ISeeder> seeders = scope.ServiceProvider.GetServices<ISeeder>()
            .OrderBy(o => o.Order);
        foreach (var seeder in seeders)
        {
            try
            {
                string seederName = seeder.GetType().Name;
                logger.LogInformation($"Seeding {seederName}");
                await seeder.SeedAsync(cancellationToken);
            }
            catch (Exception e)
            {
                logger.LogError(e, "something went wrong seeding data in DataSeeder");
                throw;
            }
        }
    }

    public async Task ValidateRequiredSeedDataAsync(CancellationToken cancellationToken = default)
    {
        await using AsyncServiceScope scope = serviceProvider.CreateAsyncScope();
        IEnumerable<ISeeder> seeders = scope.ServiceProvider.GetServices<ISeeder>()
            .Where(s => !s.IsOptional)
            .OrderBy(o => o.Order);
        
        var missingData = new List<string>();
        foreach (var seeder in seeders)
        {
            try
            {
                await seeder.ValidateAsync(cancellationToken);
            }
            catch (Exception e)
            {
                missingData.Add(e.Message);
            }
        }

        if (missingData.Any())
        {
            string details = string.Join(Environment.NewLine, missingData);
            throw new InvalidOperationException(
                $"Application cannot start — required seed data is missing:{Environment.NewLine}{details}");
        }
    }
}