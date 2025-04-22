using System.Diagnostics.CodeAnalysis;

namespace TaskManager.API.Extensions.Middleware
{
    [ExcludeFromCodeCoverage]
    public static class MiddlewareExtension
    {
        public static IApplicationBuilder AddMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ValidationMiddleware>();
        }
    }
}
