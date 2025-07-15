using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBusEndpoint;

Console.Title = "NServiceBusEndpoint";

var host = Host.CreateDefaultBuilder(args)
    .UseNServiceBus(x =>
    {
        var endpointConfiguration = new EndpointConfiguration("NServiceBusEndpoint");
        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        endpointConfiguration.SendHeartbeatTo(
            serviceControlQueue: "Particular.ServiceControl",
            frequency: TimeSpan.FromSeconds(10),
            timeToLive: TimeSpan.FromSeconds(30));

        var recoverability = endpointConfiguration.Recoverability();

        recoverability.Delayed(retriesSettings =>
        {
            retriesSettings.NumberOfRetries(0);
        });

        recoverability.Immediate(retriesSettings =>
        {
            retriesSettings.NumberOfRetries(0);
        });

        return endpointConfiguration;
    })
    .Build();


await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Press 'Enter' to send a new message. Press any other key to finish.");

var cts = new CancellationTokenSource();

Console.CancelKeyPress += (_, eventArgs) =>
{
    eventArgs.Cancel = true; // Prevent the process from terminating.
    cts.Cancel();
};

while (!cts.IsCancellationRequested)
{
    try
    {
        var key = await ConsoleHelper.ReadKeyAsync(cts.Token);

        if (key != ConsoleKey.Enter)
        {
            break;
        }

        var message = new SimpleMessage
        {
            Id = Guid.NewGuid()
        };
        await messageSession.Send("NServiceBusEndpoint", message);

        Console.WriteLine($"Sent a new message with Id = {message.Id}.");
        Console.WriteLine("Press 'Enter' to send a new message.");
    }
    catch (Exception e) when (e is OperationCanceledException)
    {
    }
}

await host.StopAsync();