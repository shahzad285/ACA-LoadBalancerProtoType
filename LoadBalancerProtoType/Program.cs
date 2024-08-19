// See https://aka.ms/new-console-template for more information
using LoadBalancerProtoType;

Console.WriteLine("Load balancer prototype!");
int i =1;
// Initialize and start servers
var server1 = new Server(5000);
var server2 = new Server(5001);
var server3 = new Server(5002);

// Start each server on its own thread
var server1Thread = new Thread(server1.Start);
var server2Thread = new Thread(server2.Start);
var server3Thread = new Thread(server3.Start);

server1Thread.Start();
server2Thread.Start();
server3Thread.Start();

// Initialize the load balancer
var loadBalancer = new LoadBalancer(new[] {
            "http://localhost:5000/",
            "http://localhost:5001/",
            "http://localhost:5002/"
        });

// Simulate distributing multiple requests
while(true)
{
    await loadBalancer.DistributeRequestAsync($"Request {i}");
    Thread.Sleep(1000); // Optional: Pause between requests
    i++;
}
