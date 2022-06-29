namespace Rabbit_7
{
    using NServiceBus;

    public class QuorumQueues
    {
        public void Configure(EndpointConfiguration endpointConfiguration)
        {
            #region quorum-queue-config

            var rabbitMqTransport = new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Quorum), "host=localhost");
            endpointConfiguration.UseTransport(rabbitMqTransport);

            #endregion
        }

        public void DisableDelayedDelivery(EndpointConfiguration endpointConfiguration)
        {
            #region disable-delayed-delivery

            var rabbitMqTransport = new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Quorum), "host=localhost");
            endpointConfiguration.UseTransport(rabbitMqTransport);

            // delayed retries are enabled by default and need to be explicitly disabled:
            endpointConfiguration.Recoverability().Delayed(c => c.NumberOfRetries(0));

            #endregion
        }

        public void EnableDelayedDelivery(EndpointConfiguration endpointConfiguration)
        {
            #region enable-delayed-delivery

            var rabbitMqTransport = new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Quorum), "host=localhost");
            endpointConfiguration.UseTransport(rabbitMqTransport);

            #endregion
        }
    }
}
