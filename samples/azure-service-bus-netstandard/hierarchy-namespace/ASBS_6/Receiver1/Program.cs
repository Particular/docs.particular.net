using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Transport.AzureServiceBus;

Console.Title = "Receiver1";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.ASBS.HierarchyNamespace.Receiver1");
endpointConfiguration.EnableInstallers();

var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new Exception("Could not read the 'AzureServiceBus_ConnectionString' environment variable. Check the sample prerequisites.");
}

var transport = new AzureServiceBusTransport(connectionString, TopicTopology.Default);
transport.HierarchyNamespaceOptions = new HierarchyNamespaceOptions { HierarchyNamespace = "my-hierarchy" };
endpointConfiguration.UseTransport(transport);
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

builder.UseNServiceBus(endpointConfiguration);

var app = builder.Build();

await app.RunAsync();
