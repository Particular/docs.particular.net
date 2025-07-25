using System;
using NServiceBus;
using Microsoft.Extensions.Hosting;

Console.Title = "Server";
var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.FullDuplex.Server");
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();
await host.StartAsync();

Console.WriteLine("Server is running. Press any key to exit");
Console.ReadKey();

await host.StopAsync();