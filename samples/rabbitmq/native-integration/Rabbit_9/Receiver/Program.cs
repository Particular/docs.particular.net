using Microsoft.Extensions.Hosting;

Console.Title = "Receiver";
var builder = Host.CreateApplicationBuilder(args);

#region ConfigureRabbitQueueName
var endpointConfiguration = new EndpointConfiguration("Samples.RabbitMQ.NativeIntegration");

var transport = new RabbitMQTransport(
    routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
    connectionString: "host=localhost");

endpointConfiguration.UseTransport(transport);
#endregion

endpointConfiguration.UseSerialization<XmlSerializer>();
endpointConfiguration.EnableInstallers();

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();