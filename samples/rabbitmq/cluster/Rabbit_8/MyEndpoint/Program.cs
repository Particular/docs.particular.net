using System;

namespace Receiver
{
    using System.Threading.Tasks;
    using NServiceBus;

    class Program
    {
        static async Task Main()
        {
            Console.Title = "Samples.RabbitMQ.Cluster.MyEndpoint";
            var endpointConfiguration = new EndpointConfiguration("Samples.RabbitMQ.Cluster.MyEndpoint");
            var transport = new RabbitMQTransport(Topology.Conventional, "host=localhost");
            endpointConfiguration.UseTransport(transport);
            endpointConfiguration.EnableInstallers();

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            await endpointInstance.SendLocal(new MyCommand());

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}
