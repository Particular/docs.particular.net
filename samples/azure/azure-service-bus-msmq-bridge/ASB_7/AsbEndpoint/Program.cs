using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Azure.ServiceBus.AsbEndpoint";
        var endpointConfiguration = new EndpointConfiguration("Samples.Azure.ServiceBus.AsbEndpoint");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.AddDeserializer<XmlSerializer>();
        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus.ConnectionString' environment variable. Check the sample prerequisites.");
        }
        transport.ConnectionString(connectionString);
        transport.UseForwardingTopology();
        var recoverability = endpointConfiguration.Recoverability();
        recoverability.DisableLegacyRetriesSatellite();

        #region connect-asb-side-of-bridge

        var routing = transport.Routing();
        var bridge = routing.ConnectToBridge("Bridge-ASB");

        #endregion

        #region subscribe-to-event-via-bridge

        bridge.RegisterPublisher(typeof(MyEvent), "Samples.Azure.ServiceBus.MsmqEndpoint");

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}