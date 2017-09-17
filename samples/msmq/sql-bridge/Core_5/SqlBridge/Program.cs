using System;
using NServiceBus;
using NServiceBus.Persistence;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.SqlBridge";
        #region bridge-config

        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("SqlBridge");
        busConfiguration.EnableInstallers();
        var persistence = busConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.ConnectionString(@"Data Source=.\SqlExpress;Database=PersistenceForSqlTransport;Integrated Security=True");
        var transport = busConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(@"Data Source=.\SqlExpress;Database=NServiceBus;Integrated Security=True");

        #endregion
        using (Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("\r\nPress any key to stop program\r\n");
            Console.ReadKey();
        }

    }
}