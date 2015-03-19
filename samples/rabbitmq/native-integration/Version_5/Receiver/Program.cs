using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        BusConfiguration busConfig = new BusConfiguration();
        #region ConfigureRabbitQueueName
        busConfig.EndpointName("Samples.RabbitMQ.NativeIntegration");
        #endregion
        busConfig.UseTransport<RabbitMQTransport>()
            .ConnectionString("host=localhost");

        busConfig.EnableInstallers();
        busConfig.UsePersistence<InMemoryPersistence>();

        using (IStartableBus bus = Bus.Create(busConfig))
        {
            bus.Start();

            Console.WriteLine("\r\nPress any key to stop program\r\n");
            Console.ReadKey();
        }
    }
}