using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Scheduler";

#region ConfigureHost

var builder = Host.CreateApplicationBuilder();

var endpointConfig = new EndpointConfiguration("Scheduler");
endpointConfig.UseTransport(new LearningTransport());
endpointConfig.UseSerialization<SystemJsonSerializer>();

builder.UseNServiceBus(endpointConfig);

builder.Services.AddHostedService<SendMessageJob>();

#endregion

var host = builder.Build();
await host.RunAsync();