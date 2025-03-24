using Microsoft.Extensions.Hosting;

Console.Title = "SimpleReceiver";
var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.RabbitMQ.SimpleReceiver");
var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
transport.UseConventionalRoutingTopology(QueueType.Quorum);
transport.ConnectionString("host=localhost");
endpointConfiguration.EnableInstallers();

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();