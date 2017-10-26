
### DisableDeadLetterQueueing

Forwarding messages to the [Dead Letter Queue](/transports/msmq/dead-letter-queues.md) is on by default as a safety mechanism. This setting prevents message loss due to either bad configuration or any other reason where the message could not be delivered to the destination queue.

Call this API to disable the storing of undeliverable messages in the DLQ. This setting must only be used where loss of messages is an acceptable scenario. As writing to the DLQ can have an extra overhead of making sure the messages are delivered and if not stored appropriately, in high volume scenarios, it can be turned off. However, it is essential to make sure that the routing configuration is accurate, as it can result in message loss otherwise.
 
snippet: disable-dlq

