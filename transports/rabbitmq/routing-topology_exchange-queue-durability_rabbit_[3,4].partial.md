## Controlling Exchange/Queue durability

The routing topologies provided by the transport will create durable exchanges and queues unless durable messages have been [disabled globally](/nservicebus/messaging/non-durable-messaging.md#enabling-non-durable-messaging-global-for-the-endpoint) for the endpoint. Transient exchanges and queues will be created when `DisableDurableMessages` is called.