---
title: In-Memory Persistence
summary: In-Memory persistence stores data in a non-durable manner for development-time only
component: InMemoryPersistence
reviewed: 2019-04-16
redirects:
- nservicebus/persistence/in-memory
tags:
- Persistence
---

Some scenarios require an in-memory persistence such as the development environment or a lightweight client not interested in durability across restarts:

snippet: ConfiguringInMemory

DANGER: All information stored in the In-Memory persistence is discarded when the process ends.

NOTE: The [Delayed Retries](/nservicebus/recoverability/#delayed-retries) mechanism uses the [timeout manager](/nservicebus/messaging/timeout-manager.md) when a transport does not natively support delayed delivery. As Delayed Retries are enabled by default, using In-Memory persistence with a transport that uses the timeout manager has the risk of losing messages that have failed processing and are waiting for another retry attempt. Use In-Memory persistence only in scenarios where it is OK to lose messages.

partial: gatewaydedupe

## Saga concurrency behavior

The in-memory persister applies [optimistic concurrency control](https://en.wikipedia.org/wiki/Optimistic_concurrency_control) when saga instances are updated.
The in-memory persister does not support locking. Optimistic concurrency control conflicts require the message to be reprocessed via recoverability.
Please read the guidance on [saga concurrency](/nservicebus/sagas/concurrency.md) on potential improvements.

## Concurrent access to non-existing saga instances

No locking is uses and race conditions errors can occur. When this happens the following error will be visible in the log:

```
System.InvalidOperationException: The saga with the correlation id 'Name: OrderId Value: f05c6e0c-aea6-48d6-846c-d1663998ebf2' already exists
```

## Concurrent access to existing saga instances

No locking is uses and race conditions errors can occur. When this happens the following error will be visible in the log:

```
System.Exception: InMemorySagaPersister concurrency violation: saga entity Id[a15a31fd-4f25-4dc3-b556-aad200e52dcb] already saved.
```
