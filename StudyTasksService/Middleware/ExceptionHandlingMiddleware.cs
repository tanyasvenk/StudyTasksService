using System.Text.Json;
using StudyTasksService.Models;

namespace StudyTasksService.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var requestId = context.Items["RequestId"]?.ToString();

                var error = new ErrorResponse
                {
                    ErrorCode = "SERVER_ERROR",
                    Message = ex.Message,
                    RequestId = requestId
                };

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(JsonSerializer.Serialize(error));
            }
        }
    }
}