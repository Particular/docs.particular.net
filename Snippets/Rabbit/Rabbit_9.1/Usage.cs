using System;
using System.Security.Cryptography.X509Certificates;

using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-disable-delayed-delivery

        var rabbitMqTransport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Classic),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq",
            enableDelayedDelivery: false
        );

        endpointConfiguration.UseTransport(rabbitMqTransport);

        #endregion
    }
}