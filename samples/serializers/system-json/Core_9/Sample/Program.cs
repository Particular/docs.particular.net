using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Sample;
using System;
using System.Collections.Generic;

Console.Title = "SystemJson";

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<InputLoopService>();

var endpointConfiguration = new EndpointConfiguration("Samples.Serialization.SystemJson");

#region config

endpointConfiguration.UseSerialization<SystemJsonSerializer>();

#endregion

endpointConfiguration.UseTransport(new LearningTransport());
Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();
