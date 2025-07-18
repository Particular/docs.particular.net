Console.Title = "Receiver";
var endpointConfiguration = new EndpointConfiguration("Receiver");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

var endpointInstance = await Endpoint.Start(endpointConfiguration);

Console.WriteLine("Receiver started. Press any key to exit.");
Console.ReadKey();

await endpointInstance.Stop();