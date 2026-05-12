using System.Reflection;

namespace Hospitaly.Modules.Cliniks.ArchitectureTests.Abstractions;

public abstract class BaseTest
{
    protected static readonly Assembly ApplicationAssembly =
        typeof(Hospitaly.Modules.Clinic.Application.AssemblyReference).Assembly;
    protected static readonly Assembly PresentationAssembly =
        typeof(Hospitaly.Modules.Clinic.Presentation.AssemblyReference).Assembly;
    protected static readonly Assembly DomainAssembly =
        typeof(Hospitaly.Modules.Clinic.Domain.AssemblyReference).Assembly;
    protected static readonly Assembly InfrastructureAssembly =
        typeof(Hospitaly.Modules.Clinic.Infrastructure.AssemblyReference).Assembly;
}