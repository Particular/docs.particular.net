using System;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "UpgradedDestination";
var endpointConfiguration = new EndpointConfiguration("UpgradedDestination");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

var builder = Host.CreateApplicationBuilder();
builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
await builder.Build().StartAsync();
