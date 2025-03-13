using System;
using NServiceBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Client;


Console.Title = "Client";
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<ClientLoopService>();
var endpointConfiguration = new EndpointConfiguration("Samples.FullDuplex.Client");
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

Console.WriteLine("Press enter to send a message");

#region ClientLoop

#endregion
builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();