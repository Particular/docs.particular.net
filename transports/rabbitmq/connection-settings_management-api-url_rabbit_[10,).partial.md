## Configuring RabbitMQ delivery limit check

> [!NOTE]
> RabbitMQ version 4.0 and above sets a default delivery limit value of 20 messages on queue creation.  Setting the delivery limit to unlimited (-1) is critical for the proper functioning of the NServiceBus recoverability process. Ensure that this setting is configured in your RabbitMQ node if the check is disabled.

The transport can verify that the RabbitMQ delivery limit is set to unlimited (-1) using the management API. This ensures that the NServiceBus recoverability process works correctly, preventing potential message loss. For this check to function, the RabbitMQ management plugin must be enabled on the RabbitMQ node.

To configure the HTTP client and perform the delivery limit check, set the authentication details as follows:

```csharp
var transport = new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Quorum), "host=localhost")
{
  ManagementApiUrl = "http://username:password@localhost:15672";
}

// Or

var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
transport.ManagementApiUrl("http://username:password@localhost:15672");
```