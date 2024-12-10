using System;
using NServiceBus;

var endpointName = "V1.Subscriber";
Console.Title = endpointName;

var endpointConfiguration = new EndpointConfiguration(endpointName);
endpointConfiguration.UseTransport<LearningTransport>();

var endpointInstance = await Endpoint.Start(endpointConfiguration);

Console.WriteLine("Press any key to exit");
Console.ReadKey();

await endpointInstance.Stop();