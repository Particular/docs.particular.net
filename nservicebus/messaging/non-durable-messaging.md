---
title: Non-Durable Messaging
summary: Information on how non-durable messaging effects the behaviors of endpoints and message delivery.
component: Core
reviewed: 2016-11-08
redirects:
 - nservicebus/messaging/express-messages
related:
 - samples/non-durable-messaging
 - transports/msmq/connection-strings
---

Relaxes message delivery guarantees in order to achieve better performance.

WARNING: This can make an endpoint more susceptible to message loss during server crashes and restarts. See [effect on transports](#effect-on-transports) for more details.


## Enabling non-durable messaging

This feature can be enabled in two ways


### For specific message types


#### Via an Express Attribute

A message can be configured to be non-durable via the use of an `[ExpressAttribute]`.

snippet: ExpressMessageAttribute


#### Using the configuration API

A subset of messages can be configured to be non-durable by using a convention.

snippet: ExpressMessageConvention


partial: global


## Effect on transports

Individual transports interpret "non-durable" messaging with a custom approach dependent on how the underlying technology functions.


### MSMQ

The default behavior of MSMQ is to use the concept of Store and Forward. In this approach messages are stored durably on disk at the sender and then delivered by MSMQ to the receiver. When non-durable messaging is used the MSMQ Transport sends messages in [Express Mode](https://msdn.microsoft.com/en-us/library/ms704130). The underlying setting that is used to achieve this is to set [Message.Recoverable](https://msdn.microsoft.com/en-us/library/system.messaging.message.recoverable) to `false`.

Non durable messages requires the queues to be [non transactional](https://msdn.microsoft.com/en-us/library/ms704006). Use non-transactional queues by setting `useTransactionalQueues` to `false` in the transport [connection string](/transports/msmq/connection-strings.md).

Note: When using non transactional queues an endpoint has to be [configured to not use transactions](/transports/transactions.md#transactions-unreliable-transactions-disabled).


### RabbitMQ

partial: rabbitmq

partial: rabbitmq-confirms


### SQL Server

The SQL Server transport has no support for this setting and it is ignored.


### Azure

The Azure transports have no support for this setting and it is ignored.
