using System;
using Microsoft.Extensions.Hosting;
using NServiceBus;
var builder = Host.CreateApplicationBuilder(args);

Console.Title = "MessageRepairingEndpoint";
var endpointConfiguration = new EndpointConfiguration("FixMalformedMessages.MessageRepairingEndpoint");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport<LearningTransport>();

builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();