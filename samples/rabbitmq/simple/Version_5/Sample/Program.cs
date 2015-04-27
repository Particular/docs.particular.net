using System;
using NServiceBus;

class Program
{

    static void Main()
    {

        #region ConfigureRabbit

        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.RabbitMQ.Simple");
        busConfiguration.UseTransport<RabbitMQTransport>()
            .ConnectionString("host=localhost");

        #endregion

        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (IStartableBus bus = Bus.Create(busConfiguration))
        {
            bus.Start();
            bus.SendLocal(new MyMessage());
            Console.WriteLine("\r\nPress any key to stop program\r\n");
            Console.ReadKey();
        }
    }
}