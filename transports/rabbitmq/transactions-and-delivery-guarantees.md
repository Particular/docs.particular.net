---
title: Transactions and delivery guarantees
summary: A description of the transport transaction modes supported by RabbitMQ
reviewed: 2018-04-20
versions: '[4,]'
component: Rabbit
tags:
 - Transactions
redirects:
 - nservicebus/rabbitmq/transactions-and-delivery-guarantees
---

The RabbitMQ transport supports the following [transport transaction modes](/transports/transactions.md):

 * Transport transaction - receive only
 * Unreliable (transactions disabled)


## Transport transaction - receive only

When running in `ReceiveOnly` mode, the RabbitMQ transport consumes messages from the broker in manual acknowledgment mode. After a message is successfully processed, it is acknowledged via the AMQP [basic.ack](http://www.rabbitmq.com/amqp-0-9-1-quickref.html#basic.ack) method, which allows the broker to know that the message can be removed from the queue. If a message is not successfully processed and needs to be retried, it is re-queued via the AMQP [basic.reject](http://www.rabbitmq.com/amqp-0-9-1-quickref.html#basic.reject) method.

WARNING: If the connection to the broker is lost for any reason before a message can be acknowledged, even if the message was successfully processed, the message will automatically be re-queued by the broker. This will result in the endpoint processing the same message multiple times.


## Unreliable (transactions disabled)

When running in `None` mode, the RabbitMQ transport consumes messages from the broker in manual acknowledgment mode. Regardless of whether a message is successfully processed or not, it is acknowledged via the AMQP [basic.ack](http://www.rabbitmq.com/amqp-0-9-1-quickref.html#basic.ack) method after the processing attempt. This means that a message will be attempted once, and moved to the error queue if it fails.

WARNING: Since manual acknowledgment mode is being used, if the connection to the broker is lost for any reason before a message can be acknowledged, the message will automatically be re-queued by the broker. If this occurs, the message will be retried by the endpoint, despite the transaction mode setting.
