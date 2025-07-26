using Shared;

Console.Title = "Receiver";

var endpointConfiguration = new EndpointConfiguration("Samples.ClaimCheck.Receiver");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

var claimCheck = endpointConfiguration.UseClaimCheck<FileShareClaimCheck, SystemJsonClaimCheckSerializer>();
var storagePath = new SolutionDirectoryFinder().GetDirectory("storage");
claimCheck.BasePath(storagePath);

var endpointInstance = await Endpoint.Start(endpointConfiguration);

Console.WriteLine("Press any key to exit");
Console.ReadKey();

await endpointInstance.Stop();