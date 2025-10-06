using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Endpoint";

var host = Host.CreateDefaultBuilder(args)
    .UseNServiceBus(x =>
    {
        var endpointConfiguration = new EndpointConfiguration(
            "Samples.Bridge.Endpoint");

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport
        {
            StorageDirectory = $"{LearningTransportInfrastructure.FindStoragePath()}2"
        });

        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Immediate(
            customizations: immediate => { immediate.NumberOfRetries(0); });
        recoverability.Delayed(delayed => delayed.NumberOfRetries(0));

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.SendHeartbeatTo("Particular.ServiceControl");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.EnableMetrics()
            .SendMetricDataToServiceControl("Particular.Monitoring", TimeSpan.FromSeconds(1));

        var routing = endpointConfiguration.UseTransport(new LearningTransport());
        routing.RouteToEndpoint(typeof(MyMessage), "Samples.Bridge.Endpoint");

        return endpointConfiguration;
    })
    .Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
var random = new Random();
Console.WriteLine("Press enter to exit");
Console.WriteLine("Press 'o' to send a message");
Console.WriteLine("Press 'f' to toggle simulating of message processing failure");

while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();
    if (key.Key == ConsoleKey.Enter)
    {
        break;
    }
    var lowerInvariant = char.ToLowerInvariant(key.KeyChar);
    switch (lowerInvariant)
    {
        case 'o':
            var id = string.Concat(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]));
            var message = new MyMessage(id);
            await messageSession.Send(message);
            break;
        case 'f':
            FailureSimulator.Enabled = !FailureSimulator.Enabled;
            Console.WriteLine($"Failure simulation is now turned {(FailureSimulator.Enabled ? "on" : "off")}");
            break;
    }
}

await host.StopAsync();