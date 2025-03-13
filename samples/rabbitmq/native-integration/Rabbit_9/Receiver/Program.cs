using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Receiver";
var builder = Host.CreateApplicationBuilder(args);

#region ConfigureRabbitQueueName
var endpointConfiguration = new EndpointConfiguration("Samples.RabbitMQ.NativeIntegration");
var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
transport.UseConventionalRoutingTopology(QueueType.Quorum);
transport.ConnectionString("host=localhost");
#endregion

endpointConfiguration.UseSerialization<XmlSerializer>();
endpointConfiguration.EnableInstallers();

Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();