using Hospitaly.Modules.Clinic.Application.Abstractions.Data;
using Hospitaly.Modules.Clinic.Domain.Clinic;
using Hospitaly.Modules.Clinic.Domain.Doctor;
using Hospitaly.Modules.Clinic.Infrastructure.Database;
using Hospitaly.Modules.Clinic.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hospitaly.Modules.Clinic.Infrastructure;

public static class ClinicModule
{
    public static IServiceCollection AddClinicModule(this IServiceCollection services , IConfiguration configuration)
    {
        services.AddClinicDbContext(configuration);
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IClinicRepository, ClinicRepository>();

        return services;
    }

    private static void AddClinicDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ClinikDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("Database"),
                b => b.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Clinic)
                );
        });
        
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ClinikDbContext>());
    }
}