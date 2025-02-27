## Configuring RabbitMQ delivery limit validation

In RabbitMQ version 4.0 and above, queues are created with a default delivery limit of 20.  However, for the NServiceBus recoverability process to function properly, the delivery limit should be set to unlimited (-1).

The RabbitMQ transport can verify and set the RabbitMQ delivery limit to unlimited (-1) using the management API.  This requires the [rabbitmq management plugin](https://www.rabbitmq.com/docs/management#getting-started) to be enabled on the RabbitMQ node.  The management API uses [basic access authentication](https://en.wikipedia.org/wiki/Basic_access_authentication) to connect.

To configure the management API and perform the delivery limit validation, set the URL details as follows:

snippet: rabbitmq-management-api-url

> [!NOTE]
> If the management API URL is not set, the transport will attempt to connect to the RabbitMQ management API with the default management API values based on the broker connection string.  For example, if the broker connection string is `host=localhost;port=5671;useTls=true`, the management API would attempt to connect to `https://guest:guest@localhost:15671` .

### Handling existing delivery limit policies

If a queue already has a policy and the delivery limit is not set to unlimited (-1), the transport will raise an exception indicating that the RabbitMQ node already has a policy applied.  The transport will not attempt to update the delivery limit of the existing policy or make a superseding delivery limit policy.

Manually updating the delivery limit of an existing policy to unlimited (-1) or removing the existing policy will allow the management API to validate the delivery limit.  If it's not feasible to update the delivery limit of an existing policy, the validation of the delivery limit can be disabled by calling the `DoNotValidateDeliveryLimits()` transport method.

> [!WARNING]
> Potential message loss can occur if the delivery limit is not set to unlimited on the queue.  If the delivery limit is not set to unlimited, the recoverability process may not function as intended.
