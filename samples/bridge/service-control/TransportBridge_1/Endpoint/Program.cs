using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Endpoint";

Console.WriteLine("Starting...");

var builder = Host.CreateDefaultBuilder(args);
builder.UseNServiceBus(x =>
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
        customizations: immediate =>
        {
            immediate.NumberOfRetries(0);
        });
    recoverability.Delayed(delayed => delayed.NumberOfRetries(0));

    endpointConfiguration.SendFailedMessagesTo("error");
    endpointConfiguration.AuditProcessedMessagesTo("audit");
    endpointConfiguration.SendHeartbeatTo("Particular.ServiceControl");
    endpointConfiguration.EnableInstallers();
    endpointConfiguration.EnableMetrics().SendMetricDataToServiceControl("Particular.Monitoring", TimeSpan.FromSeconds(1));

    return endpointConfiguration;
});

var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

var random = new Random();
const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";

Console.WriteLine("Press 'o' to send a message");
Console.WriteLine("Press 'f' to toggle simulating of message processing failure");
Console.WriteLine("Press any other key to exit");

while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();
    if (key.Key != ConsoleKey.O && key.Key != ConsoleKey.F)
    {
        break;
    }

    if (key.Key == ConsoleKey.O)
    {
        var id = new string(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]).ToArray());
        var message = new MyMessage
        {
            Id = id,
        };
        await messageSession.SendLocal(message);
        continue;
    }

    // ConsoleKey.F pressed
    FailureSimulator.Enabled = !FailureSimulator.Enabled;
    Console.WriteLine("Failure simulation is now turned " + (FailureSimulator.Enabled ? "on" : "off"));
}
