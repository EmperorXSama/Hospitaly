using Hospitaly.Common.Application.Authorization;
using Hospitaly.Modules.Users.Application.Abstractions.Data;
using Hospitaly.Modules.Users.Application.Abstractions.Identity;
using Hospitaly.Modules.Users.Domain.Users;
using Hospitaly.Modules.Users.Infrastructure.Authorization;
using Hospitaly.Modules.Users.Infrastructure.Database;
using Hospitaly.Modules.Users.Infrastructure.Identity;
using Hospitaly.Modules.Users.Infrastructure.PublicApi;
using Hospitaly.Modules.Users.Infrastructure.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PublicApi;

namespace Hospitaly.Modules.Users.Infrastructure;

public static class UsersModule
{

    public static IServiceCollection AddUserInfrastructure(this IServiceCollection services,
        IConfiguration configuration) =>
        services
            .AddUserDbContext(configuration)
            .AddHttpClients(configuration)
            .AddInfrastructureService(configuration);

    private static IServiceCollection AddUserDbContext(this IServiceCollection service, IConfiguration configuration)
    {
        
        service.AddDbContext<UserDbContext>(options => options.UseNpgsql(
            configuration.GetConnectionString("Database")
            , builder => builder.MigrationsHistoryTable(HistoryRepository.DefaultTableName,Schemas.Users)
            ));
        service.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<UserDbContext>());
        service.AddScoped<IUserRepository, UserRepository>();
        return service;
    }

    private static IServiceCollection AddHttpClients(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddScoped<IPermissionsService, PermissionService>();
        service.AddTransient<KeycloakAuthDelegatingHandler>();
        service.AddTransient<IIdentityProviderService, IdentityProviderService>();
        service.Configure<KeycloakOptions>(configuration.GetSection("User:Keycloak"));
        service.AddHttpClient<KeyCloakClient>((sp, client) =>
        {
            var keycloakOptions = sp.GetRequiredService<IOptions<KeycloakOptions>>().Value;
            client.BaseAddress = new Uri(keycloakOptions.AdminUrl);
        }).AddHttpMessageHandler<KeycloakAuthDelegatingHandler>(); 
        
        service.AddHttpClient<KeycloakUserRequests>((sp, client) =>
        {
            client.BaseAddress = new Uri("http://hospitaly.identity:8080/realms/hospitaly/");
        });
        
        service.AddScoped<IUserKeycloakRequests>(sp => sp.GetRequiredService<KeycloakUserRequests>());
        return service;
    }
    private static IServiceCollection AddInfrastructureService(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddScoped<IUserApi, UserApi>();
        return service;
    }
}