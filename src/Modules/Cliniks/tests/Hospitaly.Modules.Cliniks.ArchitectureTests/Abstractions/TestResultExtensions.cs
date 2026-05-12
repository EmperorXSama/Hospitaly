using FluentAssertions;
using NetArchTest.Rules;

namespace Hospitaly.Modules.Cliniks.ArchitectureTests.Abstractions;

internal static class TestResultExtensions
{
    public static void ShouldBeSuccessful(this TestResult testResult)
    {
        testResult.FailingTypes?.Should().BeEmpty();
    }
}