using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.Gateway.RemoteSite";
        var endpointConfiguration = new EndpointConfiguration("Samples.Gateway.RemoteSite");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        #region RemoteSiteGatewayConfig
        var gatewayConfig = endpointConfiguration.Gateway();
        gatewayConfig.AddReceiveChannel("http://localhost:25899/RemoteSite/");
        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("\r\nPress any key to stop program\r\n");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}