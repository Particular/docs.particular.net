using System;
using CustomAuditTransport;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "CustomAuditTransport";
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<InputService>();
var endpointConfiguration = new EndpointConfiguration("Samples.CustomAuditTransport");

endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
var asbConnectionString = Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString");
var transport = new AzureServiceBusTransport(asbConnectionString, TopicTopology.Default);
endpointConfiguration.UseTransport(transport);

endpointConfiguration.AuditProcessedMessagesTo("audit");
endpointConfiguration.EnableInstallers();

Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();