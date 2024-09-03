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

Console.WriteLine("Press 't' to toggle fault mode or `x` to stop.");

while (true)
{
    var key = ToLower(Console.ReadKey().KeyChar);
    if (key == 't')
    {
        SimpleMessageHandler.FaultMode = !SimpleMessageHandler.FaultMode;
        Console.WriteLine("Fault mode " + (SimpleMessageHandler.FaultMode ? "enabled" : "disabled"));
    }

    if (key == 'x')
    {
        break;
    }
}

await endpointInstance.Stop();