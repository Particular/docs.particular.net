---
title: Non-Durable Messaging
summary: Information on how non-durable messaging affects the behaviors of endpoints and message delivery.
component: Core
reviewed: 2020-06-17
redirects:
 - nservicebus/messaging/express-messages
 - samples/non-durable-messaging
related:
 - transports/msmq/connection-strings
---

The use of non-durable messages involves relaxing message delivery guarantees in order to achieve better performance.

WARNING: This can make an endpoint more susceptible to message loss during server crashes and restarts. See [effect on transports](#effect-on-transports) for more details.

partial: enable

## Effect on transports

Individual transports interpret "non-durable" messaging with a custom approach dependent on how the underlying technology functions.


### MSMQ

The default behavior of MSMQ is to use the concept of _Store and Forward_. In this approach, messages are stored durably on disk at the sender and then delivered by MSMQ to the receiver. When non-durable messaging is used, the MSMQ transport sends messages in [Express Mode](https://msdn.microsoft.com/en-us/library/ms704130). The underlying setting that is used to achieve this is to set [Message.Recoverable](https://msdn.microsoft.com/en-us/library/system.messaging.message.recoverable) to `false`.

Non-durable messages require the queues to be [non-transactional](https://msdn.microsoft.com/en-us/library/ms704006). Use non-transactional queues by setting `useTransactionalQueues` to `false` in the transport [connection string](/transports/msmq/connection-strings.md).

Note: When using non-transactional queues, an endpoint must be [configured not to use transactions](/transports/transactions.md#transactions-unreliable-transactions-disabled).


### RabbitMQ

partial: rabbitmq

partial: rabbitmq-confirms


### SQL Server

The SQL Server transport has no support for this setting and it is ignored.


### Azure

The Azure transports have no support for this setting and it is ignored.
