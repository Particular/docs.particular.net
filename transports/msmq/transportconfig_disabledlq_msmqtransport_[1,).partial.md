
### DisableDeadLetterQueueing

Forwarding messages to the [Dead Letter Queue](/transports/msmq/dead-letter-queues.md) (DLQ) is ON by default as a safety mechanism. The DLQ setting prevents message loss due to either bad configuration or any other reason where the message could not be delivered to the destination queue.

Call this API to disable storing undeliverable messages in the DLQ. This setting must only be used where the loss of messages is an acceptable scenario. Writing to the DLQ can add performance overhead. In high volume scenarios, it can be turned off. When doing so, it's essential to ensure that the routing configuration is accurate, as it can lead to message loss otherwise.
 
snippet: disable-dlq

