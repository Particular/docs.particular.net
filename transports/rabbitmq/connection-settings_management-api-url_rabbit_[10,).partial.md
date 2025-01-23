## Configuring RabbitMQ delivery limit validation

In RabbitMQ version 4.0 and above, queues are created with a default delivery limit of 20.  However, for the NServiceBus recoverability process to function properly, the delivery limit should be set to unlimited (-1).

The RabbitMQ transport can verify that the RabbitMQ delivery limit is set to unlimited (-1) using the management API.  This requires the [rabbitmq management plugin](https://www.rabbitmq.com/docs/management#getting-started) to be enabled on the RabbitMQ node.  The management API uses [basic access authentication](https://en.wikipedia.org/wiki/Basic_access_authentication) to connect.

To configure the management API and perform the delivery limit validation, set the URL details as follows:

snippet: rabbitmq-management-api-url

> [!NOTE]
> If the management API URL is not set, the transport will use the credentials from the broker connection string and the default management API URL of `http://localhost:15672/api` to attempt to connect to the RabbitMQ management API.

The use of the management API and the validation of the delivery limit can be disabled by calling the `DoNotUseManagementApi()` transport method.

> [!WARNING]
> Potential message loss can occur if the delivery limit is not set to unlimited on the RabbitMQ node.  If the delivery limit is not set to unlimited, the recoverability process may not function as intended.
