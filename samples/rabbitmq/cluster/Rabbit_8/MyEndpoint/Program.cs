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
            var transport = new RabbitMQClusterTransport(Topology.Conventional, "host=localhost", QueueMode.Quorum, DelayedDeliverySupport.Disabled);
            endpointConfiguration.UseTransport(transport);
            endpointConfiguration.EnableInstallers();

            // Delayed retries needs to be disabled since we can't safely use the timeout capabilities of the transport
            endpointConfiguration.Recoverability().Delayed(dc => dc.NumberOfRetries(0));

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press S to send a message, any other key to exit");

            while (Console.ReadKey().KeyChar == 's')
            {
                await endpointInstance.SendLocal(new MyCommand());
            }

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}
