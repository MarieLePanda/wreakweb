using codecrafters_http_server.src.RequestHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_http_server.src.Middleware
{
    public class TerminalMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HTTPContext context)
        {
            Console.WriteLine("Last middleware");
        }
    }

}
