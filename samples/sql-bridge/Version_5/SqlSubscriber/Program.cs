using System;
using NServiceBus;
using NServiceBus.Persistence;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.SqlBridge.SqlSubscriber";
        #region sqlsubscriber-config
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("SqlSubscriber");
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<NHibernatePersistence>()
            .ConnectionString(@"Data Source=.\SQLEXPRESS;Initial Catalog=PersistenceForSqlTransport;Integrated Security=True");
        busConfiguration.UseTransport<SqlServerTransport>()
            .ConnectionString(@"Data Source=.\SQLEXPRESS;Initial Catalog=NServiceBus;Integrated Security=True");
        #endregion
        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("\r\nPress any key to stop program\r\n");
            Console.ReadKey();
        }
    }
}