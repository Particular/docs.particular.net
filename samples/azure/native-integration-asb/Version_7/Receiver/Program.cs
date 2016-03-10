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
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();

        #region EndpointAndSingleQueue

        endpointConfiguration.EndpointName("Samples.ASB.NativeIntegration");

        #endregion

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseSerialization<JsonSerializer>();

        #region BrokeredMessageConvention

        AzureServiceBusTopologySettings topologySettings = endpointConfiguration.UseTransport<AzureServiceBusTransport>()
            .UseDefaultTopology();
        topologySettings.Serialization().BrokeredMessageBodyType(SupportedBrokeredMessageBodyTypes.Stream);

        #endregion

        topologySettings.ConnectionString(Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString"));

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
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
