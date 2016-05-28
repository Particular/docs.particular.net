using System;
using Events;
using NServiceBus;
using NServiceBus.Features;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.ASB.Polymorphic.Subscriber";
        var busConfiguration = new BusConfiguration();

        busConfiguration.EndpointName("Samples.ASB.Polymorphic.Subscriber");
        busConfiguration.ScaleOut().UseSingleBrokerQueue();
        var transport = busConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.ConnectionString(Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString"));

        #region DisableAutoSubscripton

        busConfiguration.DisableFeature<AutoSubscribe>();

        #endregion


        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.DisableFeature<SecondLevelRetries>();


        using (var bus = Bus.Create(busConfiguration).Start())
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