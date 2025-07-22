using System;
using System.Threading.Tasks;
using NServiceBus;

Console.Title = "WithoutDelayedRetries";

#region Disable
var endpointConfiguration = new EndpointConfiguration("Samples.ErrorHandling.WithoutDelayedRetries");
var recoverability = endpointConfiguration.Recoverability();
recoverability.Delayed(
    customizations: delayed =>
    {
        delayed.NumberOfRetries(0);
    });
#endregion

endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

var endpointInstance = await Endpoint.Start(endpointConfiguration);

Console.WriteLine("Press enter to send a message that will throw an exception.");
Console.WriteLine("Press any key to exit");

while (true)
{
    var key = Console.ReadKey();
    if (key.Key != ConsoleKey.Enter)
    {
        break;
    }

    var myMessage = new MyMessage
    {
        Id = Guid.NewGuid()
    };

    await endpointInstance.SendLocal(myMessage);
}

await endpointInstance.Stop();
