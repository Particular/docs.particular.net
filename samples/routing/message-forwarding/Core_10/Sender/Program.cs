using System;
using Messages;
using NServiceBus;

Console.Title = "Sender";

#region route-message-to-original-destination

var config = new EndpointConfiguration("Sender");
var routing = config.UseTransport(new LearningTransport());

routing.RouteToEndpoint(typeof(ImportantMessage), "OriginalDestination");

#endregion

config.UseSerialization<SystemJsonSerializer>();

var endpoint = await Endpoint.Start(config);

Console.WriteLine("Endpoint Started. Press s to send a very important message. Any other key to exit");

var i = 0;

while (Console.ReadKey(true).Key == ConsoleKey.S)
{
    var message = new ImportantMessage
    {
        Text = $"Hello there: {i++}"
    };
    await endpoint.Send(message);
    Console.WriteLine("Message sent");
}

await endpoint.Stop();