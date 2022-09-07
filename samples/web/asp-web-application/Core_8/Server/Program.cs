using System;
using NServiceBus;

var endpointConfiguration = new EndpointConfiguration(Console.Title = "Samples.AsyncPages.Server");
endpointConfiguration.EnableCallbacks(makesRequests: false);
endpointConfiguration.UseTransport(new LearningTransport());

var endpointInstance = await Endpoint.Start(endpointConfiguration);
Console.WriteLine("Press any key to exit");
Console.ReadKey();
await endpointInstance.Stop();