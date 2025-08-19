using System;
using Microsoft.Extensions.Hosting;
using NServiceBus;


Console.Title = "Billing";
var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.MultiTenant.Propagation.Billing");
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

var pipeline = endpointConfiguration.Pipeline;
pipeline.Register(new StoreTenantIdBehavior(), "Stores tenant ID in the session");
pipeline.Register(new PropagateTenantIdBehavior(), "Propagates tenant ID to outgoing messages");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();