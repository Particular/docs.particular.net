using Sender;
using Shared;

Console.Title = "Sender";
var endpointConfiguration = new EndpointConfiguration("Sender");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UsePersistence<LearningPersistence>();
var routingConfiguration = endpointConfiguration.UseTransport(new LearningTransport());

routingConfiguration.RouteToEndpoint(typeof(MyCommand), "Receiver");


var endpointInstance = await Endpoint.Start(endpointConfiguration);
await MessageSender.Start(endpointInstance);
await endpointInstance.Stop();

