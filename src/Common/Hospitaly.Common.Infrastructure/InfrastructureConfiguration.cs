using Hospitaly.Common.Application.Data;
using Hospitaly.Common.Infrastructure.Authentication;
using Hospitaly.Common.Infrastructure.Authorization;
using Hospitaly.Common.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;

namespace Hospitaly.Common.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string databaseConnectionString)
    {
        services.AddAuthenticationInternal();
        services.AddAuthorizationInternal();
        services.AddDbFactory(databaseConnectionString);
        return services;
    }

    public static void AddDbFactory(this IServiceCollection services,string databaseConnectionString)
    {
        NpgsqlDataSource npgsqlDataSource = new NpgsqlDataSourceBuilder(databaseConnectionString).Build();
        services.TryAddSingleton(npgsqlDataSource);
        services.TryAddScoped<IDbConnectionFactory, DbConnectionFactory>();
    }
}