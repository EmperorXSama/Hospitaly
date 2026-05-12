namespace Hospitaly.Api.Extensions;

internal static class ConfigurationExtension
{
    internal static void AddModuleConfiguration(this IConfigurationBuilder builder, string[] modules)
    {
        foreach (var module in modules)
        {
            builder.AddJsonFile($"appSettings/{module}/modules.{module}.json", false, true);
            builder.AddJsonFile($"appSettings/{module}/modules.{module}.Development.json", true, true);
        }
    }
}