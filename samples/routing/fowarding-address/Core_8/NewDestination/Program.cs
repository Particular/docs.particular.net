using System;
using NServiceBus;

Console.Title = "NewDestination";
var config = new EndpointConfiguration("NewDestination");
config.UseTransport<LearningTransport>();

var endpoint = await Endpoint.Start(config);

Console.WriteLine("Endpoint Started. Press any key to exit");
Console.ReadKey();

await endpoint.Stop();