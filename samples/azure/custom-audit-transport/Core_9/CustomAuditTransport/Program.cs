using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Features;

Console.Title = "CustomAuditTransport";
var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.CustomAuditTransport");

endpointConfiguration.UsePersistence<LearningPersistence>();
//var persistence = endpointConfiguration.UsePersistence<AzureTablePersistence>();
//persistence.ConnectionString("UseDevelopmentStorage=true");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();

var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new Exception("Could not read the 'AzureServiceBus_ConnectionString' environment variable. Check the sample prerequisites.");
}
var transport = new AzureServiceBusTransport(connectionString, TopicTopology.Default);
//transport.TransportTransactionMode = TransportTransactionMode.ReceiveOnly;
//endpointConfiguration.EnableOutbox();
endpointConfiguration.UseTransport(transport);

endpointConfiguration.AuditProcessedMessagesTo("audit");

endpointConfiguration.Recoverability().Immediate(i => i.NumberOfRetries(1));
endpointConfiguration.Recoverability().Delayed(d => d.NumberOfRetries(1));

endpointConfiguration.EnableInstallers();

Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);

builder.Services.AddHostedService<InputService>();

await builder.Build().RunAsync();