
// Endpoint A is on "v1" contracts

using EndpointB.Contracts;
using NServiceBus;

var configuration = new EndpointConfiguration("EndpointA");
var learningTransport = new LearningTransport();
var routing = configuration.UseTransport(learningTransport);
routing.RouteToEndpoint(typeof(EndpointBCommand), "EndpointB");

var endpoint = await Endpoint.Start(configuration);

await endpoint.Send(new EndpointBCommand()
{
    CorrelationId = Guid.NewGuid()
});

Console.WriteLine("Press any key to exit");
Console.ReadKey();
await endpoint.Stop();

