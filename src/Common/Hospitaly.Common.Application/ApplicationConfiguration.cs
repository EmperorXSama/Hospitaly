using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Hospitaly.Common.Application;

public static  class ApplicationConfiguration
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection service,
        Assembly[] assemblies
        )
    {
        service.AddMediatR(cnf =>
        {
            cnf.RegisterServicesFromAssemblies(assemblies);
        });
        service.AddValidatorsFromAssemblies(assemblies);
        return service;
    }
}