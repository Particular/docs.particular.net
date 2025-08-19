using System;
using ClientUI;
using Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "ClientUI";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("ClientUI");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();

var routing = endpointConfiguration.UseTransport(new LearningTransport());

routing.RouteToEndpoint(typeof(PlaceOrder), "Sales");

builder.UseNServiceBus(endpointConfiguration);

builder.Services.AddHostedService<InputLoopService>();

var app = builder.Build();

await app.RunAsync();