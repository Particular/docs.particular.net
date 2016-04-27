using System;
using NServiceBus;
using NServiceBus.Persistence;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.SqlBridge.MsmqSubscriber";
        #region msmqsubscriber-config
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("MsmqSubscriber");
        busConfiguration.EnableInstallers();
        var persistence = busConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.ConnectionString(@"Data Source=.\SQLEXPRESS;Initial Catalog=PersistenceForMsmqTransport;Integrated Security=True");

        #endregion
        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("\r\nPress any key to stop program\r\n");
            Console.ReadKey();
        }
    }
}