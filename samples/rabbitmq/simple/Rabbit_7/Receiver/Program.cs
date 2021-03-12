using System;

namespace Receiver
{
    using System.Threading.Tasks;
    using NServiceBus;

    class Program
    {
        static async Task Main()
        {
            Console.Title = "Samples.RabbitMQ.SimpleReceiver";
            var endpointConfiguration = new EndpointConfiguration("Samples.RabbitMQ.SimpleReceiver");
            var transport = new RabbitMQTransport(Topology.Conventional, "host=localhost");
            endpointConfiguration.UseTransport(transport);
            endpointConfiguration.EnableInstallers();

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}
