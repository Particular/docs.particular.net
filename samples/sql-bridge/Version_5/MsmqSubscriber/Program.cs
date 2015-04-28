using System;
using NServiceBus;
using NServiceBus.Persistence;

class Program
{

    static void Main()
    {
        #region msmqsubscriber-config
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("MsmqSubscriber");
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<NHibernatePersistence>()
            .ConnectionString(@"Data Source=.\SQLEXPRESS;Initial Catalog=PersistenceForMsmqTransport;Integrated Security=True");

        #endregion
        using (IStartableBus bus = Bus.Create(busConfiguration))
        {
            bus.Start();
            Console.WriteLine("\r\nPress any key to stop program\r\n");
            Console.ReadKey();
        }
    }
}