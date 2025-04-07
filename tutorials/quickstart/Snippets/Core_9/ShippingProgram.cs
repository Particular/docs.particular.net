#region ShippingProgram

using System;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Shipping";

var builder = Host.CreateApplicationBuilder(args);

// Define the endpoint name
var endpointConfig = new EndpointConfiguration("Shipping");

// Choose JSON to serialize and deserialize messages
endpointConfig.UseSerialization<SystemJsonSerializer>();

// Select the learning (filesystem-based) transport to
// communicate with other endpoints
endpointConfig.UseTransport<LearningTransport>();

// Enable monitoring errors, auditing, and heartbeats
// with the Particular Service Platform tools
endpointConfig.SendFailedMessagesTo("error");
endpointConfig.AuditProcessedMessagesTo("audit");
endpointConfig.SendHeartbeatTo("Particular.ServiceControl");

// Enable monitoring endpoint performance
var metrics = endpointConfig.EnableMetrics();
metrics.SendMetricDataToServiceControl(
    "Particular.Monitoring",
    TimeSpan.FromMilliseconds(500)
);

builder.UseNServiceBus(endpointConfig);

var app = builder.Build();

await app.RunAsync();

#endregion