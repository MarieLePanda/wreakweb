using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_http_server.src.Middleware
{
    using global::codecrafters_http_server.src.RequestHandling;
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    namespace codecrafters_http_server.src.Middleware
    {
        public class LoggerMiddleware
        {
            private readonly Func<HTTPContext, Task> _next;
            public LoggerMiddleware(Func<HTTPContext, Task> next)
            {
                _next = next;
            }

            public async Task InvokeAsync(HTTPContext context)
            {
                Console.WriteLine("Logging Middleware");

                // Log incoming request
                LogRequest(context.Request);

                // Call the next middleware in the pipeline
                await _next(context);

                // Log outgoing response
                LogResponse(context.Response);

            }

            private void LogRequest(Request request)
            {
                Console.WriteLine("Log Request");
                // Log request details
                string logMessage = $"[{DateTime.UtcNow}] {request.Verb} {request.Path}";
                Console.WriteLine(logMessage);
            }

            private void LogResponse(HttpResponse response)
            {
                Console.WriteLine("Log Response");
                // Log response details
                string logMessage = $"[{DateTime.UtcNow}] Response status code: {response.StatusCode}";
                Console.WriteLine(logMessage);
            }
        }
    }

}
