using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Sample;

Console.Title = "SchedulingTimer";

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<TimerService>();

var endpointConfiguration = new EndpointConfiguration("Samples.Scheduling.Timer");
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();