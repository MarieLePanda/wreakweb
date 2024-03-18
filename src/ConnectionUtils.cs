using codecrafters_http_server.src;
using codecrafters_http_server.src.RequestHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_http_server.src
{
    public static class ConnectionUtils
    {

        public static Request ParseRequest(Socket client)
        {

            byte[] bufferRequest = new byte[1024];
            client.Receive(bufferRequest);
            string requestString = Encoding.ASCII.GetString(bufferRequest);

            Request request = new Request(requestString);

            Console.WriteLine("Request");
            Console.WriteLine(requestString);
            return request;
        }

        public static void SendResponse(Socket client, Response response) 
        {
            byte[] msg = Encoding.ASCII.GetBytes(response.formatReponse());
            Console.WriteLine("Reponse");
            Console.WriteLine(response.formatReponse());
            client.Send(msg);

        }

        public async static Task<Request> ParseRequestAsync(Socket client)
        {

            byte[] bufferRequest = new byte[1024];
            client.Receive(bufferRequest);
            string requestString = Encoding.UTF8.GetString(bufferRequest);

            Request request = new Request(requestString);

            Console.WriteLine("Request");
            //Console.WriteLine(requestString);
            return request;
        }

        public async static Task SendResponseAsync(Socket client, HttpResponse response)
        {
            try
            {
                byte[] msg = Encoding.UTF8.GetBytes(response.ToString());
                Console.WriteLine("Reponse");
                Console.WriteLine(response.ToString());
                //Console.WriteLine(response.formatReponse());
                await client.SendAsync(msg, SocketFlags.None);
            }
            catch( Exception ex) 
            {
                Console.WriteLine("Error sending response: " + ex.Message);
            }


        }
    }
}
