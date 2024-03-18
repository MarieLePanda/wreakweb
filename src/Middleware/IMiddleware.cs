using codecrafters_http_server.src.RequestHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_http_server.src.Middleware
{
    public interface IMiddleware
    {
        Task InvokeAsync(HTTPContext context);
    }

}
