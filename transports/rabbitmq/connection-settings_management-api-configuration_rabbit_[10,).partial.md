## Configuring RabbitMQ management API access

The transport uses the RabbitMQ management API to verify broker requirements and enable [delivery limit validation](#delivery-limit-validation).

The [RabbitMQ management plugin](https://www.rabbitmq.com/docs/management) must be enabled, and the plugin's [statistics and metrics collection must not be disabled](https://www.rabbitmq.com/docs/management#disable-stats). The port that the management API is using needs to be accessible by the transport. The default port is `15672` for HTTP and `15671` for HTTPS.

By default, the transport will infer the settings to use to access the management API from the connection string. If the broker configuration requires different settings to access the management API, custom settings can be provided:

snippet: rabbitmq-management-api-configuration

There are also overloads to specify just the URL or just the credentials and continue to rely on the connection string for the unspecified settings.

### Disabling broker requirement checks

Starting in version 10.1, it is possible to disable the broker requirement checks that the transport uses the management API to make. This should only be done in extreme circumstances when it not possible to give the transport access to the management API.

snippet: rabbitmq-disable-broker-requirement-checks

> [!CAUTION]
> Using a broker that does not meet all of the requirements can result in message loss or other incorrect operation, so disabling checks is not recommended.
