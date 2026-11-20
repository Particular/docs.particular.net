using Microsoft.Extensions.Hosting;

Console.Title = "SimpleReceiver";
var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.RabbitMQ.SimpleReceiver");

var transport = new RabbitMQTransport(
    routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
    connectionString: "host=localhost");

endpointConfiguration.UseTransport(transport);
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.EnableInstallers();

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();