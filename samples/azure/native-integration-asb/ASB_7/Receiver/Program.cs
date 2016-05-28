using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.AzureServiceBus;

class Program
{
    static void Main()
    {
        MainAsync().GetAwaiter().GetResult();
    }

    static async Task MainAsync()
    {
        Console.Title = "Samples.ASB.NativeIntegration";

        #region EndpointAndSingleQueue

        var endpointConfiguration = new EndpointConfiguration("Samples.ASB.NativeIntegration");

        #endregion

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseSerialization<JsonSerializer>();

        #region BrokeredMessageConvention

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.UseTopology<ForwardingTopology>();
        var topologySettings = transport.UseTopology<EndpointOrientedTopology>();
        topologySettings.BrokeredMessageBodyType(SupportedBrokeredMessageBodyTypes.Stream);

        #endregion

        topologySettings.ConnectionString(Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString"));

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        try
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}