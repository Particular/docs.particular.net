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
        Console.Title = "Samples.RabbitMQ.NativeIntegration.Receiver";
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
        #region ConfigureRabbitQueueName
        endpointConfiguration.EndpointName("Samples.RabbitMQ.NativeIntegration");
        #endregion
        endpointConfiguration.UseTransport<RabbitMQTransport>()
            .ConnectionString("host=localhost");

        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        await endpoint.SendLocal(new MyMessage());
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpoint.Stop();
    }
}