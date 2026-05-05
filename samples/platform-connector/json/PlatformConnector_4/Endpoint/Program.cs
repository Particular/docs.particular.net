using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

var builder = Host.CreateApplicationBuilder();
builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
var host = builder.Build();
var messageSession = host.Services.GetRequiredService<IMessageSession>();
await host.StartAsync();

Console.WriteLine("Press any key to send a message, ESC to stop");

while (Console.ReadKey(true).Key != ConsoleKey.Escape)
{
    await messageSession.SendLocal(new BusinessMessage { BusinessId = Guid.NewGuid() });
    Console.WriteLine("Message sent");
}

await host.StopAsync();
