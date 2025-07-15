using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NServiceBus;

Console.Title = "Sender";

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();

var endpointConfiguration = new EndpointConfiguration("RetryFailedMessages.Sender");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
var routing = endpointConfiguration.UseTransport<LearningTransport>().Routing();
routing.RouteToEndpoint(typeof(SimpleMessage), "RetryFailedMessages.Receiver");

builder.UseNServiceBus(endpointConfiguration);

builder.Services.AddHostedService<SenderWorker>();

var host = builder.Build();
await host.RunAsync();