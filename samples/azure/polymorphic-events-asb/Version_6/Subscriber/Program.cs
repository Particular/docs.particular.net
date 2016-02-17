using System;
using System.Threading.Tasks;
using Events;
using NServiceBus;
using NServiceBus.AzureServiceBus;
using NServiceBus.Features;

class Program
{
    static void Main()
    {
        MainAsync().GetAwaiter().GetResult();
    }

    static async Task MainAsync()
    {
        BusConfiguration busConfiguration = new BusConfiguration();

        busConfiguration.EndpointName("Samples.ASB.Polymorphic.Subscriber");
        busConfiguration.UseTransport<AzureServiceBusTransport>()
            .UseTopology<StandardTopology>()
            .ConnectionString(Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString"));
        busConfiguration.SendFailedMessagesTo("error");

        #region DisableAutoSubscripton

        busConfiguration.DisableFeature<AutoSubscribe>();

        #endregion


        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.DisableFeature<SecondLevelRetries>();


        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
        IBusSession bus = endpoint.CreateBusSession();
        try
        {
            #region ControledSubscriptions

            await bus.Subscribe<BaseEvent>();

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