using codecrafters_http_server.src.Configuration;
using codecrafters_http_server.src.ErrorHandling;
using codecrafters_http_server.src.LoggingAndMonitoring;
using codecrafters_http_server.src.RequestHandling;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

namespace codecrafters_http_server.src.ConnectionManagement
{
    public class ConnectionManager
    {
        private TcpListener _listener;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly ConcurrentDictionary<Guid, Socket> _clients = new ConcurrentDictionary<Guid, Socket>();
        private readonly ConcurrentDictionary<Guid, Socket> _clientsRejected = new ConcurrentDictionary<Guid, Socket>();
        private Func<HTTPContext, Task> _pipeline;
        public async Task StartAsync(IPAddress ipAddress, int port, Func<HTTPContext, Task> pipeline)
        {
            _listener = new TcpListener(ipAddress, port);
            _listener.Start();
            _pipeline = pipeline;
            Console.WriteLine($"Server listening on {ipAddress}:{port}");
            Logger.LogInfo($"Server listening on {ipAddress}:{port}");

            //await AcceptConnectionsAsync(n);
            //}
            //catch (Exception ex)
            //{
            //Console.WriteLine($"Error starting server: {ex.Message}");
            //}
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
            _listener.Stop();
            Console.WriteLine("Server stopped.");
        }

        public async Task AcceptConnectionsAsync(ServerConfiguration serverConfiguration)
        {
            try
            {
                if(!_listener.Pending())
                    return ;

                if (_clients.Count < serverConfiguration.MaxAllowedConnections)
                {

                    Socket client = await _listener.AcceptSocketAsync();
                    Guid clientId = Guid.NewGuid();
                    _clients.TryAdd(clientId, client);

                    Console.WriteLine($"Client connected: {client}");
                    IPAddress remoteIPAddress = ((IPEndPoint)client.RemoteEndPoint).Address;
                    Logger.LogInfo($"Client connected: {client.ProtocolType} {remoteIPAddress}");

                    // Handle client in a separate thread
                    Task.Run(() => HandleClientAsync(client, clientId));

                }
                else if(_clientsRejected.Count < serverConfiguration.MaxRejectedConnections)
                {
                    Socket client = await _listener.AcceptSocketAsync();
                    Guid clientId = Guid.NewGuid();
                    _clientsRejected.TryAdd(clientId, client);

                    Console.WriteLine("Too many connections - rejecting with HTTP 500");
                    Logger.LogInfo("Too many connections - rejecting with HTTP 500");

                    Task.Run(() => HandleClientRejectAsync(client, clientId));

                }
                else
                {
                    Console.WriteLine("To many connection");
                    Logger.LogInfo("To many connection");

                    var pendingClient = _listener.AcceptSocket();
                    pendingClient.Close();

                    return;                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accepting client connection: {ex.Message}");
            }
        }

        private async Task HandleClientAsync(Socket client, Guid clientId)
        {
            try
            {
                // Handle client communication here

                Request request = await ConnectionUtils.ParseRequestAsync(client);
                HTTPContext context = new HTTPContext(request, null);
               
                await _pipeline(context);
                
                await ConnectionUtils.SendResponseAsync(client, context.Response);
                client.Close();
                _clients.TryRemove(clientId, out client);
                IPAddress remoteIPAddress = ((IPEndPoint)client.RemoteEndPoint).Address;
                Logger.LogInfo($"Client closed: {client.ProtocolType} {remoteIPAddress}");

                Console.WriteLine("Closed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling client: {ex.Message}");
            }
        }

        private async Task HandleClientRejectAsync(Socket client, Guid clientId)
        {
            try
            {
                // Handle client communication here
                HttpResponse errorResponse = await ErrorHandler.HandleErrorAsync(new TooManyConnectionsException());
                await ConnectionUtils.SendResponseAsync(client, errorResponse);
                client.Close();
                _clientsRejected.TryRemove(clientId, out client);
                Console.WriteLine("Closed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling client: {ex.Message}");
            }
        }
    }
}
