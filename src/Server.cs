using codecrafters_http_server.src;
using codecrafters_http_server.src.Configuration;
using codecrafters_http_server.src.ConnectionManagement;
using codecrafters_http_server.src.LoggingAndMonitoring;
using codecrafters_http_server.src.Middleware;
using codecrafters_http_server.src.Middleware.codecrafters_http_server.src.Middleware;
using codecrafters_http_server.src.RequestHandling;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static async Task Main(string[] args)
    {
        ServerConfiguration serverConfiguration = new ServerConfiguration(80, 20, 5);
        ConnectionManager connectionManager = new ConnectionManager();
        Logger.Initialize(@"/etc/MyApache/log/", "myappache.log", 2, 1024);
        TerminalMiddleware terminalMiddleware = new TerminalMiddleware();
        RoutingMiddleware routingMiddleware = new RoutingMiddleware(terminalMiddleware.InvokeAsync);
        LoggerMiddleware loggerMiddleware = new LoggerMiddleware(routingMiddleware.InvokeAsync);
        Func<HTTPContext, Task> pipeline = loggerMiddleware.InvokeAsync;

        connectionManager.StartAsync(System.Net.IPAddress.Any, 4221, pipeline);


        while (true)
        {
            await connectionManager.AcceptConnectionsAsync(serverConfiguration);

        }
    }
}