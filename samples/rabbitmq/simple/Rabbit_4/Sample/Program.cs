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
        Console.Title = "Samples.RabbitMQ.Simple";
        #region ConfigureRabbit

        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.RabbitMQ.Simple");
        endpointConfiguration.UseTransport<RabbitMQTransport>()
            .ConnectionString("host=localhost");

        #endregion
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        await endpoint.SendLocal(new MyMessage());
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpoint.Stop();
    }
}