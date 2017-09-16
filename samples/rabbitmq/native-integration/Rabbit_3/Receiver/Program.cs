using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.RabbitMQ.NativeIntegration.Receiver";
        #region ConfigureRabbitQueueName
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.RabbitMQ.NativeIntegration");
        var transport = busConfiguration.UseTransport<RabbitMQTransport>();
        transport.ConnectionString("host=localhost");
        #endregion

        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}