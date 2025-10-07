using System;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "LeftReceiver";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.Bridge.LeftReceiver");
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

endpointConfiguration.Conventions().DefiningMessagesAs(t => t.Name == "OrderResponse");
endpointConfiguration.Conventions().DefiningEventsAs(t => t.Name == "OrderReceived");

endpointConfiguration.SendFailedMessagesTo("error");
endpointConfiguration.EnableInstallers();

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();
