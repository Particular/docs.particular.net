using System;
using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.Persistence.Legacy;

class Program
{
    static void Main()
    {
        var busConfig = new BusConfiguration();
        busConfig.EndpointName("Samples.Versioning.V2Publisher");
        busConfig.UseSerialization<JsonSerializer>();
        busConfig.UsePersistence<InMemoryPersistence>();
        busConfig.UsePersistence<MsmqPersistence>()
            .For(Storage.Subscriptions);
        busConfig.EnableInstallers();

        using (var bus = Bus.Create(busConfig))
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