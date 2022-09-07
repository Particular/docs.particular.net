using System;
using NServiceBus;

var endpointConfiguration = new EndpointConfiguration(Console.Title = "Samples.AsyncPages.Server");
endpointConfiguration.EnableCallbacks(makesRequests: false);
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseTransport<LearningTransport>();

var endpointInstance = await Endpoint.Start(endpointConfiguration);
Console.WriteLine("Press any key to exit");
Console.ReadKey();
await endpointInstance.Stop();