using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "NServiceBusEndpoint";

var builder = Host.CreateDefaultBuilder(args);

builder.UseNServiceBus(x =>
{
    var endpointConfiguration = new EndpointConfiguration("NServiceBusEndpoint");
    endpointConfiguration.UseTransport<LearningTransport>();
    endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
    endpointConfiguration.EnableInstallers();
    endpointConfiguration.SendFailedMessagesTo("error");
    endpointConfiguration.SendHeartbeatTo("Particular.ServiceControl");

    #region DisableRetries

    var recoverability = endpointConfiguration.Recoverability();

    recoverability.Delayed(customizations: retriesSettings =>
    {
        retriesSettings.NumberOfRetries(0);
    });
    recoverability.Immediate(customizations: retriesSettings =>
    {
        retriesSettings.NumberOfRetries(0);
    });

    #endregion

    return endpointConfiguration;
});

var host = builder.Build();
await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Press 'Enter' to send a new message. Press any other key to exit.");
while (true)
{
    var key = Console.ReadKey();

    if (key.Key != ConsoleKey.Enter)
    {
        break;
    }

    var guid = Guid.NewGuid();

    var simpleMessage = new SimpleMessage
    {
        Id = guid
    };
    await messageSession.Send("NServiceBusEndpoint", simpleMessage);
    Console.WriteLine($"Sent a new message with Id = {guid}.");

    Console.WriteLine("Press 'Enter' to send a new message");
}

await host.StopAsync();