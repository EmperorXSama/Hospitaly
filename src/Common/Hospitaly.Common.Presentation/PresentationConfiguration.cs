namespace Hospitaly.Common.Presentation;

public static class PresentationConfiguration
{
    public static IApplicationBuilder UsePresentation(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        return app;
    }
}
