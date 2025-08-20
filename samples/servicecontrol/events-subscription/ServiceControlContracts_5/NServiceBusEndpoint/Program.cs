using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.Title = "NServiceBusEndpoint";

var endpointConfiguration = new EndpointConfiguration("NServiceBusEndpoint");
endpointConfiguration.UseTransport<LearningTransport>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.EnableInstallers();
endpointConfiguration.SendFailedMessagesTo("error");

endpointConfiguration.SendHeartbeatTo(
    serviceControlQueue: "Particular.ServiceControl",
    frequency: TimeSpan.FromSeconds(10),
    timeToLive: TimeSpan.FromSeconds(30));

#region DisableRetries

var recoverability = endpointConfiguration.Recoverability();

recoverability.Delayed(retriesSettings =>
{
    retriesSettings.NumberOfRetries(0);
});

recoverability.Immediate(retriesSettings =>
{
    retriesSettings.NumberOfRetries(0);
});

#endregion

var builder = Host.CreateApplicationBuilder(args);
builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

while (true)
{
    Console.WriteLine("Press 'Enter' to send a new message. Press any other key to finish.");

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
}

await host.StopAsync();