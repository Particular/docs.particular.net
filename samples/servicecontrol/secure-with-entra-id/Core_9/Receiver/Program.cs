using System;
using System.IO;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Receiver";
var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("SecureWithEntraID.Receiver");
endpointConfiguration.AuditProcessedMessagesTo("audit");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport<LearningTransport>().StorageDirectory(Path.Combine(".learningtransport"));

#region DisableRetries

var recoverability = endpointConfiguration.Recoverability();

recoverability.Delayed(
    customizations: retriesSettings =>
    {
        retriesSettings.NumberOfRetries(0);
    });
recoverability.Immediate(
    customizations: retriesSettings =>
    {
        retriesSettings.NumberOfRetries(0);
    });

#endregion

builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();