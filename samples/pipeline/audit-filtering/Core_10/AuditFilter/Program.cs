using System;
using AuditFilter;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "AuditFilter";
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<InputService>();
var endpointConfiguration = new EndpointConfiguration("Samples.AuditFilter");

endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

#region addFilterBehaviors

endpointConfiguration.AuditProcessedMessagesTo("audit");

var pipeline = endpointConfiguration.Pipeline;
pipeline.Register(
    stepId: "AuditFilter.Filter",
    behavior: typeof(AuditFilterBehavior),
    description: "prevents marked messages from being forwarded to the audit queue");
pipeline.Register(
    stepId: "AuditFilter.Rules",
    behavior: typeof(AuditRulesBehavior),
    description: "checks whether a message should be forwarded to the audit queue");
pipeline.Register(
    stepId: "AuditFilter.Context",
    behavior: typeof(AuditFilterContextBehavior),
    description: "adds a shared state for the rules and filter behaviors");


#endregion

Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();