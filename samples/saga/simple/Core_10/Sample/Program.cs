using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Sample;

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.SimpleSaga");
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

builder.UseNServiceBus(endpointConfiguration);
builder.Services.AddHostedService<InputLoopService>();

Console.Title = "Server";

var host = builder.Build();
await host.StartAsync();

