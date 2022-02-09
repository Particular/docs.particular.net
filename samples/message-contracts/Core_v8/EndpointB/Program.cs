using NServiceBus;

var configuration = new EndpointConfiguration("EndpointB");
var learningTransport = new LearningTransport();
var routing = configuration.UseTransport(learningTransport);

var endpoint = await Endpoint.Start(configuration);

Console.WriteLine("Press any key to exit");
Console.ReadKey();
await endpoint.Stop();
