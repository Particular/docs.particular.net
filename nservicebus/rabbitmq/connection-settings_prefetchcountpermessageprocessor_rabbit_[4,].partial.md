
### PrefetchCountPerMessageProcessor

The number of messages to [prefetch](http://www.rabbitmq.com/consumer-prefetch.html) when consuming messages from the broker, per message processor. The actual prefetch count used for the connection with the broker will calculated by multiplying the prefetch count per message processor by the number of message processors.

For controlling the number of message processors see [Tuning Throughput](/nservicebus/operations/tuning.md#tuning-throughput).
