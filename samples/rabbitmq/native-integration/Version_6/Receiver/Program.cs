using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        #region ConfigureRabbitQueueName
        busConfiguration.EndpointName("Samples.RabbitMQ.NativeIntegration");
        #endregion
        busConfiguration.UseTransport<RabbitMQTransport>()
            .ConnectionString("host=localhost");

        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
        await endpoint.SendLocal(new MyMessage());
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpoint.Stop();
    }
}