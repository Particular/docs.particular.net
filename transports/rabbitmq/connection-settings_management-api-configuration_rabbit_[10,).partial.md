## Configuring RabbitMQ management API access

The transport uses the RabbitMQ management API to verify broker requirements and enable [delivery limit validation](#delivery-limit-validation).

The [RabbitMQ management plugin](https://www.rabbitmq.com/docs/management) must be enabled, and the plugin's [statistics and metrics collection must not be disabled](https://www.rabbitmq.com/docs/management#disable-stats)

By default, the transport will infer the settings to use to access the management API from the connection string. If the broker configuration requires different settings to access the management API, custom settings can be provided:

snippet: rabbitmq-management-api-configuration

There are also overloads to specify just the URL or just the credentials and continue to rely on the connection string for the unspecified settings.