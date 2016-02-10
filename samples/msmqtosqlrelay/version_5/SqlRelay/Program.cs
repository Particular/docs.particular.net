using System;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Persistence;

class Program
{
    static void Main()
    {
        #region sqlrelay-config
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("SqlRelay");
        busConfiguration.UseTransport<SqlServerTransport>()
            .ConnectionString(@"Data Source=.\SQLEXPRESS;Initial Catalog=PersistenceForSqlTransport;Integrated Security=True");
        busConfiguration.UsePersistence<NHibernatePersistence>()
            .ConnectionString(@"Data Source=.\SQLEXPRESS;Initial Catalog=PersistenceForSqlTransport;Integrated Security=True");
        busConfiguration.DisableFeature<AutoSubscribe>();
        busConfiguration.EnableInstallers();
        #endregion

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}

