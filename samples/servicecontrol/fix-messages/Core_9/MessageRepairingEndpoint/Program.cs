using System;
using Microsoft.Extensions.Hosting;
using NServiceBus;
var builder = Host.CreateApplicationBuilder(args);

Console.Title = "MessageRepairingEndpoint";
var endpointConfiguration = new EndpointConfiguration("FixMalformedMessages.MessageRepairingEndpoint");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport<LearningTransport>();

Console.WriteLine("Press 'Enter' to finish.");
Console.ReadLine();
builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();