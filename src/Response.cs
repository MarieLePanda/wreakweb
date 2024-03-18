using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_http_server.src
{
    public class Response
    {
        public string StartLine { get; set; }
        public string ContentType { get; set; }
        public string Body { get; set; }
        public Response() { }

        public string formatReponse()
        {
            byte[] contentLength;
            if (Body != null)
            {
                contentLength = System.Text.Encoding.UTF8.GetBytes(Body);
                return $"{StartLine}\r\n"
                + $"Content-Type: {ContentType}\r\n"
                + $"Content-Length: {contentLength.Length}\r\n"
                + $"\r\n"
                + $"{Body}";
            }
            else
            {
                return $"{StartLine}\r\n"
                + $"Content-Type: {ContentType}\r\n"
                + $"Content-Length: 0\r\n"
                + $"\r\n";
            }


        }

        
    }
}
