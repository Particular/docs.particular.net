## Controlling Exchange/Queue durability

The routing topologies provided by the transport always create durable exchanges. Durable queues are created unless durable messages have been [disabled globally](/nservicebus/messaging/non-durable-messaging.md#enabling-non-durable-messaging-global-for-the-endpoint) for the endpoint. Transient queues will be created when `DisableDurableMessages` is called.