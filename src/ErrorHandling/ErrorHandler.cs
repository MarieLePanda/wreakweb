using codecrafters_http_server.src.RequestHandling;
using System;
using System.Net;
using System.Threading.Tasks;

namespace codecrafters_http_server.src.ErrorHandling
{
    public class ErrorHandler
    {
        public static async Task<HttpResponse> HandleErrorAsync(Exception exception)
        {
            Console.WriteLine($"An error occurred: {exception.Message}");

            // You can implement custom logic to handle different types of exceptions here
            // For simplicity, this example returns a generic error response
            return await GenerateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while processing the request.");
        }

        private static async Task<HttpResponse> GenerateErrorResponse(HttpStatusCode statusCode, string errorMessage)
        {
            var response = new HttpResponse((int)statusCode);
            response.AddHeader("Content-Type", "text/plain");
            response.Body = errorMessage;

            return response;
        }
    }
}
