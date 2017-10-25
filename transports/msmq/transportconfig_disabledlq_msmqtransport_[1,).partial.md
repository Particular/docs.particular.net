
### DisableDeadLetterQueueing

Forwarding messages to the DLQ is on by default as a safety mechanism. This setting protects the scenarios where bad configuration can lead to message loss. Undeliverable messages are forwarded to DLQ.  

Call this API to disable the storing of undeliverable messages in the dead letter queue. This setting must only be used where loss of messages is an acceptable scenario. As DLQ can have an extra overhead of making sure the messages are delivered and if not stored appropriately, in high volume scenarios, it can be turned off. However it is important to make sure that the routing configuration is accurate, as it can result in message loss otherwise. 
 
snippet: disable-dlq

