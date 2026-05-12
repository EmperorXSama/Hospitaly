using ErrorOr;

namespace Hospitaly.Common.Presentation;

public static class ErrorOrExtensions
{
    public static ApiResponse<T> ToApiResponse<T>(this ErrorOr<T> result)
    {
        return result.Match(
            ApiResponse<T>.Success,
            errors => ApiResponse<T>.Failure(errors.ToApiError()));
    }

    public static ApiResponse<Success> ToApiResponse(this ErrorOr<Success> result)
    {
        return result.Match(
            _ => ApiResponse<Success>.Success(new Success()),
            errors => ApiResponse<Success>.Failure(errors.ToApiError()));
    }

    private static ApiError ToApiError(this List<Error> errors)
    {
        var first = errors[0];

        Dictionary<string, string[]>? validationErrors = null;
        var grouped = errors
            .Where(static e => e.Type == ErrorType.Validation || e.Type == ErrorType.Failure)
            .GroupBy(static e => e.Code)
            .ToDictionary(static g => g.Key, static g => g.Select(static e => e.Description).ToArray());

        if (grouped.Count > 0)
            validationErrors = grouped;

        return new ApiError(first.Code, first.Description, validationErrors);
    }
}
