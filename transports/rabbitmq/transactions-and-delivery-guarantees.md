---
title: Transactions and delivery guarantees
summary: A description of the transport transaction modes supported by RabbitMQ
reviewed: 2020-01-24
versions: '[4,]'
component: Rabbit
redirects:
 - nservicebus/rabbitmq/transactions-and-delivery-guarantees
---

The RabbitMQ transport supports the following [transport transaction modes](/transports/transactions.md):

 * Transport transaction - receive only
 * Unreliable (transactions disabled)


## Transport transaction - receive only

When running in `ReceiveOnly` mode, messages are consumed in manual acknowledgment mode. Successfully processed messages are acknowledged via the AMQP [basic.ack](https://www.rabbitmq.com/amqp-0-9-1-quickref.html#basic.ack) method, which allows the broker remove them from the queue. Failed messages that needs to be retried is re-queued via the AMQP [basic.reject](https://www.rabbitmq.com/amqp-0-9-1-quickref.html#basic.reject) method.

WARNING: If the connection to the broker is lost for any reason before a message can be acknowledged, even if the message was successfully processed, the message will automatically be re-queued by the broker. This will result in the endpoint processing the same message multiple times.


## Unreliable (transactions disabled)

Similar to `ReceiveOnly` mode messages are consumed in manual acknowledgment mode, but regardless of whether a message is successfully processed or not, it is acknowledged via the AMQP [basic.ack](https://www.rabbitmq.com/amqp-0-9-1-quickref.html#basic.ack) method after the processing attempt. This means that a message will be attempted once, and moved to the error queue if it fails.

WARNING: Since manual acknowledgment mode is being used, if the connection to the broker is lost for any reason before a message can be acknowledged, the message will automatically be re-queued by the broker. If this occurs, the message will be retried by the endpoint, despite the transaction mode setting.