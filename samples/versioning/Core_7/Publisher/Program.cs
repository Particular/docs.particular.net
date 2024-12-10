using System;
using Contracts;
using NServiceBus;

var endpointName = "Publisher";
Console.Title = endpointName;

var endpointConfiguration = new EndpointConfiguration(endpointName);
endpointConfiguration.UseTransport<LearningTransport>();

var endpointInstance = await Endpoint.Start(endpointConfiguration);

Console.WriteLine("Press enter to publish a message");
Console.WriteLine("Press any key to exit");

while (true)
{
    var key = Console.ReadKey();

    if (key.Key != ConsoleKey.Enter)
    {
        break;
    }

    await endpointInstance.Publish<ISomethingMoreHappened>(sh =>
    {
        sh.SomeData = 1;
        sh.MoreInfo = "It's a secret.";
    });

    Console.WriteLine("Published event.");
}

await endpointInstance.Stop();