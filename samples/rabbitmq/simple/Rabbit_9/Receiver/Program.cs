using System;
using Microsoft.Extensions.Hosting;
using NServiceBus;


Console.Title = "SimpleReceiver";
var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.RabbitMQ.SimpleReceiver");
var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
transport.UseConventionalRoutingTopology(QueueType.Quorum);
transport.ConnectionString("host=localhost");
endpointConfiguration.EnableInstallers();


Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();