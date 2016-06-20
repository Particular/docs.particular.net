---
title: Non-Durable Messaging
summary: Information on how non-durable messaging effects the behaviors of endpoints and message delivery.
redirects:
 - nservicebus/messaging/express-messages
related:
 - samples/non-durable-messaging
 - nservicebus/msmq/connection-strings
---

Relaxes message delivery guarantees in order to achieve better performance.

WARNING: This makes the endpoint susceptible to message loss during server crashes and restarts.


## Enabling non-durable messaging

This feature can be enabled in two ways


### For specific message types


#### Via an Express Attribute

A message can be configured to be non-durable via the use of an `[ExpressAttribute]`.

snippet:ExpressMessageAttribute


#### Using the configuration API

A subset of messages can be configured to be non-durable by using a convention.

snippet:ExpressMessageConvention


### Global for the endpoint

All message to be non durable via the configuration API.

snippet: set-to-non-durable


## Effect on transports

Individual transports interpret "non-durable" messaging with a custom approach dependent on how the underlying technology functions.


### MSMQ

The default behavior of MSMQ is to use the concept of Store and Forward. In this approach messages are stored durably on disk at the sender and then delivered by MSMQ to the receiver. When non-durable messaging is used the MSMQ Transport sends messages in [Express Mode](https://msdn.microsoft.com/en-us/library/ms704130).
The underlying setting that is used to achieve this is to set [Message.Recoverable](https://msdn.microsoft.com/en-us/library/system.messaging.message.recoverable) to `false`.


#### Extra config for MSMQ

There are some extra configurations required for MSMQ

 * The target `useTransactionalQueues` needs to be `false` in the [MSMQ connection strings](/nservicebus/msmq/connection-strings.md)
 * The endpoint needs to be non-transactional.


### RabbitMQ

The default behavior of sending messaged via RabbitMQ is to wait for the publisher confirmation before proceeding. When non-durable messaging is used the RabbitMQ transport skips the publisher confirmation step.

This effectively means the blocking call to [WaitForConfirmsOrDie](http://www.rabbitmq.com/javadoc/com/rabbitmq/client/Channel.html#waitForConfirmsOrDie%28long%29) is skipped.


### SQL Server

The SQL Server transport has no support for this setting and it is ignored.


### Azure

The Azure transports have no support for this setting and it is ignored.
