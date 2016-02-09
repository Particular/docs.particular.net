using System;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence;
using Shared;

class Program
{
    static void Main()
    {
        #region publisher-config
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("MsmqPublisher");
        busConfiguration.UsePersistence<NHibernatePersistence>()
            .ConnectionString(@"Data Source=.\SQLEXPRESS;Initial Catalog=PersistenceForMsmqTransport;Integrated Security=True");
        busConfiguration.EnableInstallers();
        #endregion

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press enter to publish an event.");
            #region PublishLoop
            while (Console.ReadLine() != null)
            {
                bus.Publish(new SomethingHappened());
                Console.WriteLine("SomethingHappened Event published");
            }
            #endregion
        }
    }
}
