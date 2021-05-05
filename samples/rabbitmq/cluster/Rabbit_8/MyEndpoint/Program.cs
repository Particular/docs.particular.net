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

            #region cluster-configuration
            var transport = new RabbitMQClusterTransport(Topology.Conventional, "host=localhost", QueueMode.Quorum, DelayedDeliverySupport.Disabled);
            #endregion

            #region cluster-add-nodes
            // if you have setup the 3-node cluster as mentioned in the sample instructions
            transport.AddClusterNode("localhost", 5673);
            transport.AddClusterNode("localhost", 5674);
            #endregion

            endpointConfiguration.UseTransport(transport);
            endpointConfiguration.EnableInstallers();

            #region cluster-disable-retries
            // Delayed retries needs to be disabled since we can't safely use the timeout capabilities of the transport
            endpointConfiguration.Recoverability().Delayed(dc => dc.NumberOfRetries(0));
            #endregion

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press [s] to send a message, any other key to exit");

            while (Console.ReadKey().Key == ConsoleKey.S)
            {
                await endpointInstance.SendLocal(new MyCommand());
            }

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}
