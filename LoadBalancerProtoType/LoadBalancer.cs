using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LoadBalancerProtoType
{
    internal class LoadBalancer
    {
        private readonly string[] _serverUrls;
        private int _currentServer = 0;
        private readonly HttpClient _httpClient;

        public LoadBalancer(string[] serverUrls)
        {
            _serverUrls = serverUrls;
            _httpClient = new HttpClient();
        }

        public async Task DistributeRequestAsync(string requestData)
        {
            // Example of round-robin load balancing
            _currentServer = (_currentServer + 1) % _serverUrls.Length;
            string serverUrl = _serverUrls[_currentServer];

            Console.WriteLine($"LoadBalancer directing request to server at {serverUrl}");

            // Forward the request to the selected server
            await ForwardRequestAsync(serverUrl, requestData);
        }

        private async Task ForwardRequestAsync(string url, string requestData)
        {
            try
            {
                var content = new StringContent(requestData, Encoding.UTF8, "text/plain");
                var response = await _httpClient.PostAsync(url, content);

                string responseData = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Received response: {responseData}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in ForwardRequestAsync: {ex.Message}");
            }
        }
    }
}
