using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_http_server.src.ErrorHandling
{
    public class TooManyConnectionsException : Exception
    {
        public TooManyConnectionsException() : base("Too many connections") { }
        public TooManyConnectionsException(string message) : base(message) { }
        public TooManyConnectionsException(string message, Exception innerException) : base(message, innerException) { }
    }
}
