using System;
using Events;
using NServiceBus;
using NServiceBus.Features;

class Program
{

    static void Main()
    {
        BusConfiguration busConfiguration = new BusConfiguration();

        busConfiguration.EndpointName("Samples.ASB.Polymorphic.Subscriber");
        busConfiguration.ScaleOut().UseSingleBrokerQueue();
        busConfiguration.UseTransport<AzureServiceBusTransport>()
            .ConnectionString(Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString"));

        #region DisableAutoSubscripton

        busConfiguration.DisableFeature<AutoSubscribe>();

        #endregion


        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.DisableFeature<SecondLevelRetries>();


        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            #region ControledSubscriptions

            bus.Subscribe<BaseEvent>();

            #endregion

            Console.WriteLine("Subscriber is ready to receive events");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}