using System;
using NServiceBus;

class Program
{

    static void Main()
    {

        Console.Title = "Samples.RabbitMQ.Simple";
        #region ConfigureRabbit

        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.RabbitMQ.Simple");
        var transport = busConfiguration.UseTransport<RabbitMQTransport>();
        transport.ConnectionString("host=localhost");

        #endregion

        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            bus.SendLocal(new MyMessage());
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}