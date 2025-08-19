using System;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Receiver";

var builder = Host.CreateApplicationBuilder();

var endpointConfig = new EndpointConfiguration("Receiver");
endpointConfig.UseTransport(new LearningTransport());
endpointConfig.UseSerialization<SystemJsonSerializer>();

builder.UseNServiceBus(endpointConfig);

var host = builder.Build();
await host.RunAsync();