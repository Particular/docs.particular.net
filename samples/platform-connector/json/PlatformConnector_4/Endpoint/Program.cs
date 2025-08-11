Console.Title = "Endpoint";

var endpointConfiguration = new EndpointConfiguration("Endpoint");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport<LearningTransport>();
endpointConfiguration.UsePersistence<NonDurablePersistence>();

#region loadConnectionDetails
var connectionPath = Path.Combine(".", "platformConnection.json");
var json = File.ReadAllText(connectionPath);
var platformConnection = ServicePlatformConnectionConfiguration.Parse(json);
#endregion

#region configureConnection
endpointConfiguration.ConnectToServicePlatform(platformConnection);
#endregion

var endpoint = await Endpoint.Start(endpointConfiguration);

Console.WriteLine("Press any key to send a message, ESC to stop");

while (Console.ReadKey(true).Key != ConsoleKey.Escape)
{
    await endpoint.SendLocal(new BusinessMessage { BusinessId = Guid.NewGuid() });
    Console.WriteLine("Message sent");
}

await endpoint.Stop();
