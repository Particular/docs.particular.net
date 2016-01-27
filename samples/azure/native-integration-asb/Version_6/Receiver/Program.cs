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
        BusConfiguration busConfiguration = new BusConfiguration();

        #region EndpointAndSingleQueue

        busConfiguration.EndpointName("Samples.ASB.NativeIntegration");

        #endregion

        busConfiguration.SendFailedMessagesTo("error");
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.UseSerialization<JsonSerializer>();

        #region BrokeredMessageConvention

        AzureServiceBusTopologySettings topologySettings = busConfiguration.UseTransport<AzureServiceBusTransport>()
            .UseDefaultTopology();
        topologySettings.Serialization().BrokeredMessageBodyType(SupportedBrokeredMessageBodyTypes.Stream);

        #endregion

        topologySettings.ConnectionString(Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString"));

        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
        try
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}
