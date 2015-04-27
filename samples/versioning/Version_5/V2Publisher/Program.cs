using System;
using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.Persistence.Legacy;

class Program
{
    static void Main()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Versioning.V2Publisher");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.UsePersistence<MsmqPersistence>()
            .For(Storage.Subscriptions);
        busConfiguration.EnableInstallers();

        using (IStartableBus bus = Bus.Create(busConfiguration))
        {
            bus.Start();
            Console.WriteLine("Press 'Enter' to publish a message, Ctrl + C to exit.");
            while (Console.ReadLine() != null)
            {
                bus.Publish<V2.Messages.ISomethingHappened>(sh =>
                {
                    sh.SomeData = 1;
                    sh.MoreInfo = "It's a secret.";
                });

                Console.WriteLine("Published event.");
            }
        }
     
    }
}