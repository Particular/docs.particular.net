using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Sender;
using Shared;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();

var endpointConfiguration = new EndpointConfiguration("Sender");

var connectionString = builder.Configuration.GetConnectionString("AzureServiceBusConnectionString");
var routing = endpointConfiguration.UseTransport(new AzureServiceBusTransport(connectionString, TopicTopology.Default));
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

endpointConfiguration.AuditProcessedMessagesTo("audit");
routing.RouteToEndpoint(typeof(Ping), "Receiver");

// Operational scripting: https://docs.particular.net/transports/azure-service-bus/operational-scripting
endpointConfiguration.EnableInstallers();

builder.UseNServiceBus(endpointConfiguration);

builder.Services.AddHostedService<SenderWorker>();

var host = builder.Build();

await host.RunAsync();
