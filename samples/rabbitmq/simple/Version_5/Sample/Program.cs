using System;
using NServiceBus;

class Program
{

    static void Main()
    {

        #region ConfigureRabbit

        BusConfiguration busConfig = new BusConfiguration();
        busConfig.EndpointName("Samples.RabbitMQ.Simple");
        busConfig.UseTransport<RabbitMQTransport>()
            .ConnectionString("host=localhost");

        #endregion

        busConfig.UseSerialization<JsonSerializer>();
        busConfig.EnableInstallers();
        busConfig.UsePersistence<InMemoryPersistence>();

        using (IStartableBus bus = Bus.Create(busConfig))
        {
            bus.Start();
            bus.SendLocal(new MyMessage());
            Console.WriteLine("\r\nPress any key to stop program\r\n");
            Console.ReadKey();
        }
    }
}