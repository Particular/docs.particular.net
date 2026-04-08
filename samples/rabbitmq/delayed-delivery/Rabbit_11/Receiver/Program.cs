using Microsoft.Extensions.Hosting;

Console.Title = "Receiver";
var builder = Host.CreateApplicationBuilder(args);

#region ConfigureRabbit
var endpointConfiguration = new EndpointConfiguration("Samples.RabbitMQ.DelayedDelivery.Receiver");
var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
transport.UseConventionalRoutingTopology(QueueType.Quorum);
transport.ConnectionString("host=localhost");
#endregion

endpointConfiguration.EnableInstallers();

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();
