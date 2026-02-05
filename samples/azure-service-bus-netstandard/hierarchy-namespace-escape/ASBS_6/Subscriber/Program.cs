using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Transport.AzureServiceBus;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddConsole();

builder.Services.AddAzureServiceBusTopology(builder.Configuration);

var endpointConfiguration = new EndpointConfiguration("Samples.ASBS.HierarchyNs.Subscriber");

var section = builder.Configuration.GetSection("AzureServiceBus");
var topologyOptions = section.GetSection("Topology").Get<TopologyOptions>()!;
var topology = TopicTopology.FromOptions(topologyOptions);
var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new Exception("Could not read the 'AzureServiceBus_ConnectionString' environment variable. Check the sample prerequisites.");
}
var transport = new AzureServiceBusTransport(connectionString, topology)
{
    Topology =
    {
        // Validation is already done by the generic host so we can disable in the transport
        OptionsValidator = new TopologyOptionsDisableValidationValidator()
    }
};
transport.HierarchyNamespaceOptions = new HierarchyNamespaceOptions { HierarchyNamespace = "my-hierarchy" };
endpointConfiguration.UseTransport(transport);
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.EnableInstallers();

builder.UseNServiceBus(endpointConfiguration);

var app = builder.Build();
await app.RunAsync();