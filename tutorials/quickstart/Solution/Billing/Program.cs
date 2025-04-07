using System;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Billing";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Billing");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport<LearningTransport>();

endpointConfiguration.SendFailedMessagesTo("error");
endpointConfiguration.AuditProcessedMessagesTo("audit");
endpointConfiguration.SendHeartbeatTo("Particular.ServiceControl");

var metrics = endpointConfiguration.EnableMetrics();
metrics.SendMetricDataToServiceControl("Particular.Monitoring", TimeSpan.FromMilliseconds(500));

builder.UseNServiceBus(endpointConfiguration);

var app = builder.Build();

await app.RunAsync();

