using System.Text.Json.Serialization;

namespace Hospitaly.Common.Presentation;

public sealed record ApiResponse<T>
{
    public T? Data { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ApiError? Error { get; init; }

    public bool IsSuccess => Error is null;

    public static ApiResponse<T> Success(T data) => new() { Data = data };

    public static ApiResponse<T> Failure(ApiError error) => new() { Error = error };
}

public static class ApiResponse
{
    public static ApiResponse<T> Success<T>(T data) => ApiResponse<T>.Success(data);

    public static ApiResponse<object?> Failure(ApiError error) => new() { Error = error };
}
