using System;
using System.IO;
using System.Threading.Tasks;
using NServiceBus;

public class Program
{
    public static async Task Main()
    {
        Console.Title = "Endpoint";

        var endpointConfiguration = new EndpointConfiguration("Endpoint");

        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        #region loadConnectionDetails
        var connectionPath = @".\platformConnection.json";
        var json = File.ReadAllText(connectionPath);
        var platformConnection = ServicePlatformConnectionConfiguration.Parse(json);
        #endregion

        #region configureConnection
        endpointConfiguration.ConnectToServicePlatform(platformConnection);
        #endregion

        var endpoint = await Endpoint.Start(endpointConfiguration);

        while (Console.ReadKey(true).Key != ConsoleKey.Escape)
        {
            await endpoint.SendLocal(new BusinessMessage { BusinessId = Guid.NewGuid() });
        }

        await endpoint.Stop();
    }
}