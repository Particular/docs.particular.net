using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;

Console.Title = "Receiver";

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();

var endpointConfiguration = new EndpointConfiguration("RetryFailedMessages.Receiver");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport<LearningTransport>();

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

var host = builder.Build();

await host.StartAsync();

Console.WriteLine("Press [t] to toggle fault mode. Press any other key to exit.");

while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key != ConsoleKey.T)
    {
        break;
    }

    SimpleMessageHandler.FaultMode = !SimpleMessageHandler.FaultMode;
    Console.WriteLine();
    Console.WriteLine("Fault mode " + (SimpleMessageHandler.FaultMode ? "enabled" : "disabled"));
}

await host.StopAsync();