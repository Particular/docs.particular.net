#region ShippingProgram

using Microsoft.Extensions.Hosting;

Console.Title = "Shipping";

var builder = Host.CreateApplicationBuilder(args);

// Define the endpoint name
var endpointConfiguration = new EndpointConfiguration("Shipping");

// Choose JSON to serialize and deserialize messages
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

// Select the learning (filesystem-based) transport to
// communicate with other endpoints
endpointConfiguration.UseTransport(new LearningTransport());

// Enable monitoring errors, auditing, and heartbeats
// with the Particular Service Platform tools
endpointConfiguration.SendFailedMessagesTo("error");
endpointConfiguration.AuditProcessedMessagesTo("audit");
endpointConfiguration.SendHeartbeatTo("Particular.ServiceControl");

// Enable monitoring endpoint performance
var metrics = endpointConfiguration.EnableMetrics();
metrics.SendMetricDataToServiceControl("Particular.Monitoring", TimeSpan.FromMilliseconds(500));

builder.UseNServiceBus(endpointConfiguration);

var app = builder.Build();

await app.RunAsync();

#endregion