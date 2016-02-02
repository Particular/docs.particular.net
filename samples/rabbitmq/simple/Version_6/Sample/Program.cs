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

        #region ConfigureRabbit

        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.RabbitMQ.Simple");
        busConfiguration.UseTransport<RabbitMQTransport>()
            .ConnectionString("host=localhost");

        #endregion

        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
        await endpoint.SendLocal(new MyMessage());
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpoint.Stop();
    }
}