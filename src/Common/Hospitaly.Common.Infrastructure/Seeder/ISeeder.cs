namespace Hospitaly.Common.Infrastructure.Seeder;

public interface ISeeder
{
    int Order { get; }
    bool IsOptional{get;}
    Task SeedAsync(CancellationToken cancellationToken = default);
    Task ValidateAsync(CancellationToken cancellationToken = default);
}