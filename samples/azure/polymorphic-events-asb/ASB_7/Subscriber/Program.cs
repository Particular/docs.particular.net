using System;
using System.Threading.Tasks;
using Events;
using NServiceBus;
using NServiceBus.AzureServiceBus;
using NServiceBus.AzureServiceBus.Addressing;
using NServiceBus.Features;

class Program
{
    static void Main()
    {
        MainAsync().GetAwaiter().GetResult();
    }

    static async Task MainAsync()
    {
        Console.Title = "Samples.ASB.Polymorphic.Subscriber";
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.ASB.Polymorphic.Subscriber");
        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.ConnectionString(Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString"));
        var topology = transport.UseTopology<EndpointOrientedTopology>();
        transport.Sanitization().UseStrategy<EndpointOrientedTopologySanitization>();

        #region RegisterPublisherNames

        topology.RegisterPublisherForType("Samples.ASB.Polymorphic.Publisher", typeof(BaseEvent));
        topology.RegisterPublisherForType("Samples.ASB.Polymorphic.Publisher", typeof(DerivedEvent));

        #endregion
        
        endpointConfiguration.SendFailedMessagesTo("error");

        #region DisableAutoSubscripton

        endpointConfiguration.DisableFeature<AutoSubscribe>();

        #endregion

        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.DisableFeature<SecondLevelRetries>();


        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        try
        {
            #region ControledSubscriptions

            await endpoint.Subscribe<BaseEvent>();

            #endregion

            Console.WriteLine("Subscriber is ready to receive events");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}