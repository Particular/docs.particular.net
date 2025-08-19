using NServiceBus;

class OutgoingNativeMessageCustomization
{
    OutgoingNativeMessageCustomization(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-customize-outgoing-message

        var rabbitMqTransport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Classic),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq",
            enableDelayedDelivery: false
        );

        rabbitMqTransport.OutgoingNativeMessageCustomization = (operation, properties) =>
        {
            //Set values on IBasicProperties
            properties.ContentType = "application/my-type";
        };

        #endregion
    }
}