using System.Reflection;
using FluentAssertions;
using Hospitaly.Common.Domain;
using Hospitaly.Modules.Cliniks.ArchitectureTests.Abstractions;
using NetArchTest.Rules;

namespace Hospitaly.Modules.Cliniks.ArchitectureTests.Domain;

public class DomainTests : BaseTest
{

    [Fact]
    public void DomainEvents_Should_BeSealed()
    {
        Types.InAssembly(DomainAssembly)
            .That()
            .ImplementInterface(typeof(IDomainEvent))
            .Should()
            .BeSealed()
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void DomainEvents_Should_Have_DomainEventPostFix()
    {
        Types.InAssembly(DomainAssembly)
            .That()
            .ImplementInterface(typeof(IDomainEvent))
            .Should()
            .HaveNameEndingWith("DomainEvent")
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void Entities_ShouldHave_PrivateParameterlessConstructor()
    {
        IEnumerable<Type> entityTypes = Types.InAssembly(DomainAssembly)
            .That().Inherit(typeof(Entity)).GetTypes();

        var failingTypes = new List<Type>();
        foreach (var entityType in entityTypes)
        {
            ConstructorInfo[] constructorInfos =
                entityType.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);

            if (!constructorInfos.Any(c => c.IsPrivate && c.GetParameters().Length == 0))
            {
                failingTypes.Add(entityType);
            }
        }

        failingTypes.Should().BeEmpty();
    }

    [Fact]
    public void Entities_ShouldOnlyHave_PrivateConstructor()
    {
        IEnumerable<Type> entityTypes = Types.InAssembly(DomainAssembly)
            .That().Inherit(typeof(Entity)).GetTypes();

        var failingTypes = new List<Type>();

        foreach (var entityType in entityTypes)
        {
            ConstructorInfo[] constructorInfos =
                entityType.GetConstructors(BindingFlags.Instance | BindingFlags.Public);

            if (constructorInfos.Any())
            {
                failingTypes.Add(entityType);
            }
        }

        failingTypes.Should().BeEmpty();
    }
    
}











