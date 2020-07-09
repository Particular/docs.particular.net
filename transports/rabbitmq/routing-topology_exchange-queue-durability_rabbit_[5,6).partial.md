## Controlling exchange and queue durability

The routing topologies provided by the transport create durable exchanges and queues by default. To create transient exchanges and queues, use the following:

snippet: rabbitmq-disable-durable-exchanges

NOTE: To maintain backwards compatibility with previous versions of the transport, if durable messages have been [disabled globally](/nservicebus/messaging/non-durable-messaging.md#enabling-non-durable-messaging-global-for-the-endpoint) for the endpoint, the call to `UseDurableExchangesAndQueues` is required.

snippet: rabbitmq-disable-durable-messages