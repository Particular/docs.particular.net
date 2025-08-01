using System;
using NServiceBus;

Console.Title = "OriginalDestination";

#region forward-messages-after-processing

var config = new EndpointConfiguration("OriginalDestination");
config.UseTransport(new LearningTransport());

config.ForwardMessagesAfterProcessingTo("UpgradedDestination");

#endregion

config.UseSerialization<SystemJsonSerializer>();

var endpoint = await Endpoint.Start(config);

Console.WriteLine("Endpoint Started. Press any key to exit");

Console.ReadKey();

await endpoint.Stop();