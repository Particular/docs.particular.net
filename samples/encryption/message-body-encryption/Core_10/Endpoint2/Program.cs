using System;
using Microsoft.Extensions.Hosting;
using NServiceBus;


Console.Title = "Endpoint2";
var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.MessageBodyEncryption.Endpoint2");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.RegisterMessageEncryptor();

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();
