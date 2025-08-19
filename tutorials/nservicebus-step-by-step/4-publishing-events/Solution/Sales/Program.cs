using System;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Sales";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Sales");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();

endpointConfiguration.UseTransport(new LearningTransport());

builder.UseNServiceBus(endpointConfiguration);

var app = builder.Build();

await app.RunAsync();