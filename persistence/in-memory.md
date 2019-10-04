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

## Saga concurrency

In-memory persistence uses [optimistic concurrency control](https://en.wikipedia.org/wiki/Optimistic_concurrency_control) when accessing saga data. See below for examples of the exceptions thrown when conflicts occur. More information about these scenarios is available in _[saga concurrency](/nservicebus/sagas/concurrency.md)_, including guidance on how to reduce the number of conflicts.

### Creating saga data

```
System.InvalidOperationException: The saga with the correlation id 'Name: OrderId Value: f05c6e0c-aea6-48d6-846c-d1663998ebf2' already exists
```

### Updating or deleting saga data

```
System.Exception: InMemorySagaPersister concurrency violation: saga entity Id[a15a31fd-4f25-4dc3-b556-aad200e52dcb] already saved.
```
