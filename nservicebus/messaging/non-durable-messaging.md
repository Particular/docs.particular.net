---
title: Non-Durable Messaging
summary: Information on how non-durable messaging affects the behaviors of endpoints and message delivery.
component: Core
reviewed: 2025-05-19
redirects:
 - nservicebus/messaging/express-messages
 - samples/non-durable-messaging
related:
 - transports/msmq/connection-strings
---

Relaxed message delivery guarantees to improve performance at the cost of reliability.

## Overview

Non-durable messaging trades delivery guarantees for better performance, increasing the risk of message loss during server crashes or restarts.

> [!WARNING]
> This approach significantly increases the risk of message loss when servers crash or restart. See [Effect on Transports](#effect-on-transports) for details.

partial: enable

## Effect on Transports

Each transport implements non-durable messaging differently based on its underlying technology.

### MSMQ

MSMQ typically uses _Store and Forward_ by default, storing messages durably on disk before delivery. With non-durable messaging, MSMQ sends messages in [Express Mode](https://msdn.microsoft.com/en-us/library/ms704130) by setting [`Message.Recoverable`](https://msdn.microsoft.com/en-us/library/system.messaging.message.recoverable) to `false`.

Non-durable messages require [non-transactional queues](https://msdn.microsoft.com/en-us/library/ms704006). Configure this by setting `useTransactionalQueues` to `false` in the [transport connection string](/transports/msmq/connection-strings.md).

> [!NOTE]
> When using non-transactional queues, endpoints must be [configured not to use transactions](/transports/transactions.md#transaction-modes-unreliable-transactions-disabled).

### RabbitMQ

partial: rabbitmq


### SQL Server

The SQL Server transport has no support for this setting and it is ignored.


### Azure Service Bus

The Azure Service Bus transport has no support for this setting and it is ignored.

### Azure Storage Queues

The Azure Storage Queues transport has no support for this setting and it is ignored.

### Amazon SQS

The Amazon SQS transport has no support for this setting and it is ignored.
