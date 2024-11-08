﻿using System;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Billing";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Billing");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();

endpointConfiguration.UseTransport(new LearningTransport());

builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();