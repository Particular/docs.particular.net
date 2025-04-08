﻿using Microsoft.Extensions.Hosting;
using NServiceBus;
using System;

Console.Title = "ClientUI";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("ClientUI");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();

var transport = endpointConfiguration.UseTransport(new LearningTransport());

builder.UseNServiceBus(endpointConfiguration);

var app = builder.Build();

await app.RunAsync();