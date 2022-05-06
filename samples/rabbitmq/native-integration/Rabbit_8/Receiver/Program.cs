using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.RabbitMQ.NativeIntegration.Receiver";

        #region ConfigureRabbitQueueName
        var endpointConfiguration = new EndpointConfiguration("Samples.RabbitMQ.NativeIntegration");
        var transport = new RabbitMQTransport(Topology.Conventional, "host=localhost");
        endpointConfiguration.UseTransport(transport);
        #endregion

        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}