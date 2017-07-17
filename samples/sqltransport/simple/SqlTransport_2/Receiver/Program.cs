using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.SqlServer.SimpleReceiver";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.SqlServer.SimpleReceiver");
        var transport = busConfiguration.UseTransport<SqlServerTransport>();
        var connection = @"Data Source=.\SqlExpress;Database=NsbSamplesSqlTransport;Integrated Security=True";
        transport.ConnectionString(connection);
        busConfiguration.UsePersistence<InMemoryPersistence>();
        SqlHelper.EnsureDatabaseExists(connection);
        using (Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.WriteLine("Waiting for message from the Sender");
            Console.ReadKey();
        }
    }
}