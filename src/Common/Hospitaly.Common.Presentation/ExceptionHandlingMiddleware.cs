using System.Net;
using Hospitaly.Common.Application.Exceptions;

namespace Hospitaly.Common.Presentation;

internal sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (HospitalyException ex)
        {
            _logger.LogError(ex, "HospitalyException occurred: {RequestName}", ex.RequestName);

            var apiError = ex.Error.HasValue
                ? new ApiError(ex.Error.Value.Code, ex.Error.Value.Description)
                : new ApiError("InternalServer", "An unexpected error occurred.");

            context.Response.StatusCode = (int)HttpStatusCode.OK;
            await context.Response.WriteAsJsonAsync(ApiResponse.Failure(apiError));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");

            var apiError = new ApiError("InternalServer", "An unexpected error occurred.");

            context.Response.StatusCode = (int)HttpStatusCode.OK;
            await context.Response.WriteAsJsonAsync(ApiResponse.Failure(apiError));
        }
    }
}
