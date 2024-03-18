using codecrafters_http_server.src;
using codecrafters_http_server.src.RequestHandling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_http_server.src.Middleware
{

    public class RoutingMiddleware
    {

        private readonly Func<HTTPContext, Task> _next;

        public RoutingMiddleware(Func<HTTPContext, Task> next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HTTPContext context)
        {
            Console.WriteLine("Router Middleware");

            Request request = context.Request;
            try
            {
                string mainDirectory = @"/etc/MyApache/www/";
                string filePath = Path.Combine(mainDirectory, GetFilePath(request.Path));
                if (File.Exists(filePath))
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    DateTime lastModified = fileInfo.LastWriteTime;
                    string fileContent = await File.ReadAllTextAsync(filePath, Encoding.UTF8);
                    HttpResponse response = new HttpResponse(200)
                    {
                        Body = fileContent
                    };
                    SetContentTypeAndLength(response, "text/html; charset=utf-8");
                    //response.AddHeader("Last-Modified", lastModified.ToUniversalTime().ToString("R"));
                    context.Response = response;
                }
                else if (request.Path.StartsWith("/echo"))
                {
                   context.Response = await EchoHandler(request);
                }
                else if (request.Path.Equals("/user-agent"))
                {
                    context.Response = UserAgentHandler(request);
                }
                else
                {
                    context.Response = new HttpResponse(404);
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Return a generic error response
                context.Response = new HttpResponse(500);
            }

            await _next(context);

        }

        private static string GetFilePath(string path)
        {
            if (path == "/")
                return "index.html";
            else
                return path.Substring(1);
        }

        private static async Task<HttpResponse> EchoHandler(Request request)
        {
            string body = request.Path.Replace("/echo/", "");
            HttpResponse response = new HttpResponse(200)
            {
                Body = body
            };
            SetContentTypeAndLength(response, "text/plain; charset=utf-8");
            return response;
        }

        private static HttpResponse UserAgentHandler(Request request)
        {
            string userAgent = request.Headers["User-Agent"];
            HttpResponse response = new HttpResponse(200)
            {
                Body = userAgent
            };
            SetContentTypeAndLength(response, "text/plain; charset=utf-8");
            return response;
        }

        private static void SetContentTypeAndLength(HttpResponse response, string contentType)
        {
            response.AddHeader("Content-Type", contentType);
            response.AddHeader("Content-Length", response.Body.Length.ToString());
        }
    }
}
