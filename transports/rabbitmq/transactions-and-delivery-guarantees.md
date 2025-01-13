---
title: Transactions and delivery guarantees
summary: A description of the transport transaction modes supported by RabbitMQ
reviewed: 2025-01-13
versions: '[4,]'
component: Rabbit
redirects:
 - nservicebus/rabbitmq/transactions-and-delivery-guarantees
---

partial: supported-modes

## Transport transaction - receive only

In `ReceiveOnly` mode, messages are consumed in manual acknowledgment mode. Successfully processed messages are acknowledged via the AMQP [basic.ack](https://www.rabbitmq.com/amqp-0-9-1-quickref.html#basic.ack) method, at which point the broker will remove them from the queue. Failed messages that need to be retried are re-queued via the AMQP [basic.reject](https://www.rabbitmq.com/amqp-0-9-1-quickref.html#basic.reject) method.

> [!WARNING]
> If the connection to the broker is lost before a message can be acknowledged, even if the message was successfully processed, the broker will automatically re-queue the message. This will result in the endpoint processing the same message multiple times.

partial: transaction-mode-none
