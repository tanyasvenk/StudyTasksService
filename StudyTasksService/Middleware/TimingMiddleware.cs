using System.Diagnostics;

namespace StudyTasksService.Middleware
{
    public class TimingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TimingMiddleware> _logger;

        public TimingMiddleware(RequestDelegate next, ILogger<TimingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var sw = Stopwatch.StartNew();

            await _next(context);

            sw.Stop();

            var requestId = context.Items["RequestId"];

            _logger.LogInformation($"Request {requestId} took {sw.ElapsedMilliseconds} ms");
        }
    }
}