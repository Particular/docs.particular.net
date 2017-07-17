using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.SqlServer.SimpleSender";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.SqlServer.SimpleSender");

        #region TransportConfiguration

        var transport = busConfiguration.UseTransport<SqlServerTransport>();
        var connection = @"Data Source=.\SqlExpress;Database=NsbSamplesSqlTransport;Integrated Security=True";
        transport.ConnectionString(connection);

        #endregion
        busConfiguration.UsePersistence<InMemoryPersistence>();

        SqlHelper.EnsureDatabaseExists(connection);
        using (var bus = Bus.Create(busConfiguration).Start())
        {
            bus.Send("Samples.SqlServer.SimpleReceiver", new MyMessage());
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}