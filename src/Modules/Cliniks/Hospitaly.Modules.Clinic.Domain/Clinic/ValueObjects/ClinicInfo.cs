using ErrorOr;

namespace Hospitaly.Modules.Clinic.Domain.Clinic.ValueObjects;

public record ClinicInfo
{
    public string Name { get; init; }
    public string TradingName { get; init; }
    public string Description { get; private set; }
    public string? LogoUrl { get; set; }
    
    private ClinicInfo() { }
    private ClinicInfo(string name, string? tradingName, string description, string? logoUrl)
    {
        Name = name;
        TradingName = tradingName?? name;
        Description = description;
        LogoUrl = logoUrl;
    }

    public static ErrorOr<ClinicInfo> Create(string name, string? tradingName, string description, string? logoUrl)
    {
        if (string.IsNullOrEmpty(name))
        {
            return Error.Validation(
                code: "ClinicInfo.InvalidName",
                description: "Clinic name cannot be null or empty.",
                metadata: new Dictionary<string, object>
                {
                    ["name"] = name
                });
        }

        if (string.IsNullOrEmpty(description))
        {
            return Error.Validation(code:"ClinicInfo.InvalidDescription",description: "Clinic description cannot be null or empty.",
                metadata: new Dictionary<string, object>
                {
                    ["description"] = description
                });
            
        }

        return new ClinicInfo(name, tradingName, description, logoUrl);
        
    }
    
}