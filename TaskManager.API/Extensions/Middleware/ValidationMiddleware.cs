using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using TaskManager.Application.Commons.Bases;
using TaskManager.Application.Commons.Extensions;

namespace TaskManager.API.Extensions.Middleware
{
    [ExcludeFromCodeCoverage]
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (ValidationExceptionCustom ex)
            {
                context.Response.ContentType = "application/json";
                await JsonSerializer.SerializeAsync(context.Response.Body, new BaseResponse<object> { Message = "Validation Errors", Errors = ex.Errors });
            }
        }
    }
}
