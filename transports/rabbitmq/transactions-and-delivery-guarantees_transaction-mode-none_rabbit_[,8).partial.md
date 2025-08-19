## Unreliable (transactions disabled)

Like in `ReceiveOnly` mode, messages are consumed in manual acknowledgment mode. Regardless of whether a message is successfully processed, it is acknowledged via the AMQP [basic.ack](https://www.rabbitmq.com/amqp-0-9-1-quickref.html#basic.ack) method after the processing attempt. This means a message will be attempted once and moved to the error queue if it fails.

> [!WARNING]
> Since manual acknowledgment mode is being used, if the connection to the broker is lost for any reason before a message can be acknowledged, the broker will automatically re-queue the message. If this occurs, the endpoint will retry the message despite the transaction mode setting.
