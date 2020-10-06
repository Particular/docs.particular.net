---
title: In-Memory Persistence
summary: In-Memory persistence stores data in a non-durable manner for development-time only
component: InMemoryPersistence
reviewed: 2019-12-17
redirects:
- nservicebus/persistence/in-memory
---

Some scenarios require an in-memory persistence such as the development environment or a lightweight client not interested in durability across restarts.

## Persistence at a glance

|Feature                    |   |
|:---                       |---
|Storage Types              |Sagas, Outbox, Subscriptions, Timeouts
|Unsupported Storage Types  |None
|Transactions               |None
|Saga Concurrency Control   |Optimistic concurrency
|Scripted Deployment        |Not supported
|Installers                 |Not supported, the required folder structure is always created

## Configuration

snippet: ConfiguringInMemory

DANGER: All information stored in the In-Memory persistence is discarded when the process ends.

NOTE: The [Delayed Retries](/nservicebus/recoverability/#delayed-retries) mechanism uses the [timeout manager](/nservicebus/messaging/timeout-manager.md) when a transport does not natively support delayed delivery. As Delayed Retries are enabled by default, using In-Memory persistence with a transport that uses the timeout manager has the risk of losing messages that have failed processing and are waiting for another retry attempt. Use In-Memory persistence only in scenarios where it is OK to lose messages.

partial: gatewaydedupe

## Saga concurrency

When simultaneously handling messages, conflicts may occur. See below for examples of the exceptions which are thrown. _[Saga concurrency](/nservicebus/sagas/concurrency.md)_ explains how these conflicts are handled, and contains guidance for high-load scenarios.

### Starting a saga

Example exception:

```
System.InvalidOperationException: The saga with the correlation id 'Name: OrderId Value: f05c6e0c-aea6-48d6-846c-d1663998ebf2' already exists
```

### Updating or deleting saga data

In-memory persistence uses [optimistic concurrency control](https://en.wikipedia.org/wiki/Optimistic_concurrency_control) when updating or deleting saga data.

Example exception:

```
System.Exception: InMemorySagaPersister concurrency violation: saga entity Id[a15a31fd-4f25-4dc3-b556-aad200e52dcb] already saved.
```