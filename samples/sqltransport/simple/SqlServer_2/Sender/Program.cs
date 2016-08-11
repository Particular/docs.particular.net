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
        transport.ConnectionString(@"Data Source=.\SqlExpress;Database=SqlServerSimple;Integrated Security=True");

        #endregion
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            bus.Send("Samples.SqlServer.SimpleReceiver", new MyMessage());
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}