

## Providing a custom connection manager


The default connection manager that comes with the transport is usually good enough for most users. To control how the connections with the broker are managed, implement a custom connection manager by inheriting from `IManageRabbitMqConnections`. This requires that connections be provided for:

 1. Administrative actions like creating queues and exchanges.
 1. Publishing messages to the broker.
 1. Consuming messages from the broker.

In order for the transport to use the above, register it as shown below:

snippet:rabbitmq-config-useconnectionmanager