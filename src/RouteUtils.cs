using codecrafters_http_server.src;
using codecrafters_http_server.src.RequestHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_http_server.src
{
    public static class Routeutils
    {
        public static Response PathMapping(Request request) 
        {
            Response response = new Response();

            if (request.Verb.Equals("GET"))
            {
                if(request.Path.Equals("/"))
                {
                    response.StartLine = "HTTP/1.1 200 OK";

                }
                else if (request.Path.StartsWith("/echo"))
                {
                    Console.WriteLine($"path: {request.Path}");

                    response.StartLine = "HTTP/1.1 200 OK";
                    response.Body = request.Path.Replace("/echo/", "");
                    response.ContentType = "text/plain";
                }
                else if (request.Path.Equals("/user-agent"))
                {
                    response.StartLine = "HTTP/1.1 200 OK";
                    response.Body = request.Headers["User-Agent"];
                    response.ContentType = "text/plain";

                }
                else
                {
                    response.StartLine = "HTTP/1.1 404 Not Found";
                }
            }
            else
            {
                response.StartLine = "HTTP/1.1 404 Not Found";

            }


            return response;

        }

        public static async Task<HttpResponse> PathMappingAsync(HTTPContext context)
        {
            Request request = context.Request;
            try
            {
                string mainDirectory = @"C:\MyApache\www\";
                string filePath = Path.Combine(mainDirectory, GetFilePath(request.Path));
                if (File.Exists(filePath))
                {
                    string fileContent = await File.ReadAllTextAsync(filePath, Encoding.UTF8);
                    HttpResponse response = new HttpResponse(200)
                    {
                        Body = fileContent
                    };
                    SetContentTypeAndLength(response, "text/html; charset=utf-8");
                    return response;
                }
                else if (request.Path.StartsWith("/echo"))
                {
                    return await EchoHandler(request);
                }
                else if (request.Path.Equals("/user-agent"))
                {
                    return UserAgentHandler(request);
                }
                else
                {
                    return new HttpResponse(404);
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Return a generic error response
                return new HttpResponse(500);
            }
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
