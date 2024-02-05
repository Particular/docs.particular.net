using System;
using NServiceBus;

Console.Title = "MessageRepairingEndpoint";
var endpointConfiguration = new EndpointConfiguration("FixMalformedMessages.MessageRepairingEndpoint");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport<LearningTransport>();

var endpointInstance = await Endpoint.Start(endpointConfiguration);

Console.WriteLine("Press 'Enter' to finish.");
Console.ReadLine();

await endpointInstance.Stop();