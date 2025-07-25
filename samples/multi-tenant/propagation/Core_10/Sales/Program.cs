using Microsoft.Extensions.Hosting;

Console.Title = "Sales";
var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.MultiTenant.Propagation.Sales");
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

#region configuration

var pipeline = endpointConfiguration.Pipeline;
pipeline.Register(new StoreTenantIdBehavior(), "Stores tenant ID in the session");
pipeline.Register(new PropagateTenantIdBehavior(), "Propagates tenant ID to outgoing messages");

#endregion

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();