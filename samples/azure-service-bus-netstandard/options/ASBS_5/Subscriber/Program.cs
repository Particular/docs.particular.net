using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Transport.AzureServiceBus;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddConsole();

builder.Services.AddAzureServiceBusTopology(builder.Configuration);

var endpointConfiguration = new EndpointConfiguration("Samples.AzureServiceBus.Options.Subscriber");

var section = builder.Configuration.GetSection("AzureServiceBus");
var topologyOptions = section.GetSection("Topology").Get<TopologyOptions>()!;
var topology = TopicTopology.FromOptions(topologyOptions);

endpointConfiguration.UseTransport(new AzureServiceBusTransport(section["ConnectionString"]!, topology));
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.EnableInstallers();

builder.UseNServiceBus(endpointConfiguration);

var app = builder.Build();
await app.RunAsync();