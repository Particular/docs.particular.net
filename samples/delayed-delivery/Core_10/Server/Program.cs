using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

var builder = Host.CreateApplicationBuilder(args);

Console.Title = "Server";
var endpointConfiguration = new EndpointConfiguration("Samples.DelayedDelivery.Server");
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

Console.WriteLine("Press any key, application loading");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();