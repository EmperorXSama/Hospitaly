using FluentAssertions;
using NetArchTest.Rules;

namespace ArchitectureTests.Abstractions;

internal static class TestResultExtensions
{
    internal static void ShouldBeSuccessful(this TestResult testResults)
    {
        testResults.FailingTypes?.Should().BeEmpty();
    }
}