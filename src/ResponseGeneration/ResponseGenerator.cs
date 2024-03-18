using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using codecrafters_http_server.src.RequestHandling;

namespace codecrafters_http_server.src.ResponseGenerator
{
    public static class ResponseGenerator
    {
        public static string GenerateHttpResponse(HttpResponse httpResponse)
        {
            StringBuilder responseBuilder = new StringBuilder();

            // Start line
            responseBuilder.AppendLine($"HTTP/1.1 {httpResponse.StatusCode} {GetStatusMessage(httpResponse.StatusCode)}");

            // Headers
            foreach (var header in httpResponse.Headers)
            {
                responseBuilder.AppendLine($"{header.Key}: {header.Value}");
            }

            // Body
            if (!string.IsNullOrEmpty(httpResponse.Body))
            {
                responseBuilder.AppendLine($"Content-Length: {Encoding.UTF8.GetByteCount(httpResponse.Body)}");
                responseBuilder.AppendLine(); // Empty line before body
                responseBuilder.Append(httpResponse.Body);
            }
            else
            {
                responseBuilder.AppendLine(); // Empty line if no body
            }

            return responseBuilder.ToString();
        }

        private static string GetStatusMessage(int statusCode)
        {
            // You can add more status codes and messages as needed
            switch (statusCode)
            {
                case 200:
                    return "OK";
                case 404:
                    return "Not Found";
                default:
                    return "Unknown";
            }
        }
    }
}
