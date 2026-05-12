using System.Text.Json.Serialization;

namespace Hospitaly.Common.Presentation;

public sealed class ApiError
{
    public string Code { get; }
    public string Message { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, string[]>? ValidationErrors { get; }

    public ApiError(string code, string message, Dictionary<string, string[]>? validationErrors = null)
    {
        Code = code;
        Message = message;
        ValidationErrors = validationErrors;
    }
}
