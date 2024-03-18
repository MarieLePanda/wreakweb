using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_http_server.src.Configuration
{
    public class ServerConfiguration
    {
        public int Port { get; set; }
        public int MaxAllowedConnections { get; set; }
        public int MaxRejectedConnections { get; set; }
        // Ajoutez d'autres propriétés de configuration selon vos besoins

        public ServerConfiguration(int port, int maxAllowedConnections, int maxRejectedConnections)
        {
            Port = port;
            MaxAllowedConnections = maxAllowedConnections;
            MaxRejectedConnections = maxRejectedConnections;
            // Initialisez d'autres propriétés de configuration si nécessaire
        }
    }
}
