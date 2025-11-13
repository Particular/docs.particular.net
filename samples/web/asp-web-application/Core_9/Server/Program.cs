using System;
using NServiceBus;
using Microsoft.Extensions.Hosting;

Console.Title = "Server";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.Web.Server");
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();