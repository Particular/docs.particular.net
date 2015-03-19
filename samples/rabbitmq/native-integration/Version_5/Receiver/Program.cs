using System;
using System.Text;
using NServiceBus;
using NServiceBus.Logging;

class Program
{

    static void Main()
    {
        DefaultFactory defaultFactory = LogManager.Use<DefaultFactory>();

        defaultFactory.Level(LogLevel.Warn);


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