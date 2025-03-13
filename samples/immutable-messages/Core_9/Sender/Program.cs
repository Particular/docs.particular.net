using System;
using Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using UsingClasses.Messages;


Console.Title = "Sender";
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<MessageSender>();
var endpointConfiguration = new EndpointConfiguration("Samples.ImmutableMessages.UsingInterfaces.Sender");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UsePersistence<LearningPersistence>();
var routingConfiguration = endpointConfiguration.UseTransport(new LearningTransport());

routingConfiguration.RouteToEndpoint(typeof(MyMessageImpl), "Samples.ImmutableMessages.UsingInterfaces.Receiver");
routingConfiguration.RouteToEndpoint(typeof(MyMessage), "Samples.ImmutableMessages.UsingInterfaces.Receiver");

endpointConfiguration.ApplyCustomConventions();
Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();