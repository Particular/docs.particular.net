using System;
using NServiceBus;
using Microsoft.Extensions.Hosting;

Console.Title = "Server";
var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.FullDuplex.Server");
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

var endpointInstance = await Endpoint.Start(endpointConfiguration);
Console.WriteLine("Press any key to exit");
Console.ReadKey();
await endpointInstance.Stop();
builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();