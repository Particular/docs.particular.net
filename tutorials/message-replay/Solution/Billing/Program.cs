﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Billing";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Billing");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();

endpointConfiguration.UseTransport<LearningTransport>();

Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();
