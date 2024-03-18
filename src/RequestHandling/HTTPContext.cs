using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_http_server.src.RequestHandling
{
    public class HTTPContext
    {
        public Request Request {  get; set; }
        public HttpResponse Response { get; set; }
        public HTTPContext(Request request, HttpResponse response) 
        {
            Request = request;
            Response = response;
        }
    }
}
