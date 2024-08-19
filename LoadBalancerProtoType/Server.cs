using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LoadBalancerProtoType
{
    public class Server
    {
        private readonly HttpListener _listener;
        public int _port { get; }

        public Server(int port)
        {
            _port = port;
            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://localhost:{_port}/");
        }

        public void Start()
        {
            _listener.Start();
            Console.WriteLine($"HTTP Server started and listening on port {_port}");

            while (true)
            {
                HttpListenerContext context = _listener.GetContext();
                Console.WriteLine($"Received request on port {_port}");

                // Process the request on a new thread
                ThreadPool.QueueUserWorkItem(HandleRequest, context);
            }
        }

        private void HandleRequest(object obj)
        {
            var context = (HttpListenerContext)obj;
            var request = context.Request;
            var response = context.Response;

            // Process the request
            string requestData = new StreamReader(request.InputStream, request.ContentEncoding).ReadToEnd();
            Console.WriteLine($"Received request data: {requestData}");

            // Send a response back to the client
            string responseData = $"Response from server on port {_port}";
            byte[] buffer = Encoding.UTF8.GetBytes(responseData);
            response.ContentLength64 = buffer.Length;
            using (var output = response.OutputStream)
            {
                output.Write(buffer, 0, buffer.Length);
            }

            Console.WriteLine($"Sent response from port {_port}");
        }
    }
}