using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.SqlServer.SimpleSender";
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.SqlServer.SimpleSender");

        #region TransportConfiguration

        var transport = busConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(@"Data Source=.\SQLEXPRESS;Initial Catalog=SqlServerSimple;Integrated Security=True");

        #endregion
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            bus.Send("Samples.SqlServer.SimpleReceiver", new MyMessage());
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}