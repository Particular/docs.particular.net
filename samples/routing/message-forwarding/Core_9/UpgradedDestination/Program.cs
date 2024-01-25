using System;
using NServiceBus;

Console.Title = "UpgradedDestination";
var config = new EndpointConfiguration("UpgradedDestination");
config.UseSerialization<SystemJsonSerializer>();
config.UseTransport(new LearningTransport());

var endpoint = await Endpoint.Start(config);

Console.WriteLine("Endpoint Started. Press any key to exit");

Console.ReadKey();

await endpoint.Stop();