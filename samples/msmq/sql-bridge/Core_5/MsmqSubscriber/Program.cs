using System;
using NServiceBus;
using NServiceBus.Persistence;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.SqlBridge.MsmqSubscriber";
        #region msmqsubscriber-config
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("MsmqSubscriber");
        busConfiguration.EnableInstallers();
        var persistence = busConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.ConnectionString(@"Data Source=.\SqlExpress;Database=PersistenceForMsmqTransport;Integrated Security=True");

        #endregion
        using (Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("\r\nPress any key to stop program\r\n");
            Console.ReadKey();
        }
    }
}