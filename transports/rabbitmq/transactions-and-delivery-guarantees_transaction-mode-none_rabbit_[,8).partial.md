## Unreliable (transactions disabled)

Similar to `ReceiveOnly` mode, messages are consumed in manual acknowledgment mode, but regardless of whether a message is successfully processed or not, it is acknowledged via the AMQP [basic.ack](https://www.rabbitmq.com/amqp-0-9-1-quickref.html#basic.ack) method after the processing attempt. This means that a message will be attempted once, and moved to the error queue if it fails.

WARNING: Since manual acknowledgment mode is being used, if the connection to the broker is lost for any reason before a message can be acknowledged, the message will automatically be re-queued by the broker. If this occurs, the message will be retried by the endpoint, despite the transaction mode setting.
