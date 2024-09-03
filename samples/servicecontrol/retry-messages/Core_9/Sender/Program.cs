using System;
using NServiceBus;

Console.Title = "Sender";
var endpointConfiguration = new EndpointConfiguration("FixMalformedMessages.Sender");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
var routing = endpointConfiguration.UseTransport<LearningTransport>().Routing();
routing.RouteToEndpoint(typeof(SimpleMessage), "FixMalformedMessages.Receiver");

var endpointInstance = await Endpoint.Start(endpointConfiguration);

Console.WriteLine("Press 'Enter' to send a new message. Press any other key to finish.");

while (true)
{
    var key = Console.ReadKey();

    if (key.Key != ConsoleKey.Enter)
    {
        break;
    }

    var simpleMessage = new SimpleMessage();

    await endpointInstance.Send(simpleMessage);

    Console.WriteLine("Press 'Enter' to send a new message. Press any other key to finish.");
}

await endpointInstance.Stop();