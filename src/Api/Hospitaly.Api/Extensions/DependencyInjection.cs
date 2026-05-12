using Hospitaly.Common.Application;
using Hospitaly.Modules.Clinic.Infrastructure;
using Hospitaly.Modules.Users.Infrastructure;

namespace Hospitaly.Api.Extensions;

internal static class DependencyInjection
{
    internal static IServiceCollection AddModules(
        this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddUserInfrastructure(configuration);
        serviceCollection.AddClinicModule(configuration);
        serviceCollection.AddApplicationServices(
            [
                Hospitaly.Modules.Users.Application.AssemblyReference.Assembly,
                Hospitaly.Modules.Clinic.Application.AssemblyReference.assembly
            ]);
        return serviceCollection;
    }
}