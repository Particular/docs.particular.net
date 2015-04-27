using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        #region ConfigureRabbitQueueName
        busConfiguration.EndpointName("Samples.RabbitMQ.NativeIntegration");
        #endregion
        busConfiguration.UseTransport<RabbitMQTransport>()
            .ConnectionString("host=localhost");

        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (IStartableBus bus = Bus.Create(busConfiguration))
        {
            bus.Start();

            Console.WriteLine("\r\nPress any key to stop program\r\n");
            Console.ReadKey();
        }
    }
}