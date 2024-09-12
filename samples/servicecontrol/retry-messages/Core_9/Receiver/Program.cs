using System;
using NServiceBus;
using static System.Char;

Console.Title = "Receiver";
var endpointConfiguration = new EndpointConfiguration("FixMalformedMessages.Receiver");
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

var endpointInstance = await Endpoint.Start(endpointConfiguration);

Console.WriteLine("Press 't' to toggle fault mode or `Esc` to stop.");

while (true)
{
    var key = Console.ReadKey();
    if (key.Key == ConsoleKey.T)
    {
        SimpleMessageHandler.FaultMode = !SimpleMessageHandler.FaultMode;
        Console.WriteLine();
        Console.WriteLine("Fault mode " + (SimpleMessageHandler.FaultMode ? "enabled" : "disabled"));
    }

    if (key.Key == ConsoleKey.Escape || (key.Key == ConsoleKey.C && (key.Modifiers & ConsoleModifiers.Control) != 0))
    {
        break;
    }
}

await endpointInstance.Stop();