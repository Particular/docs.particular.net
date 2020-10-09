---
title: Non-durable persistence
summary: Non-durable persistence (previously known as In-Memory persistence) stores data in a non-durable manner
component: NonDurablePersistence
reviewed: 2019-12-17
redirects:
- nservicebus/persistence/in-memory
---

partial: noteinmemory

Some scenarios require an non-durable persistence such as the development environment or a lightweight client not interested in durability across restarts:

snippet: ConfiguringNonDurable

DANGER: All information stored is discarded when the process ends.

NOTE: The [Delayed Retries](/nservicebus/recoverability/#delayed-retries) mechanism uses the [timeout manager](/nservicebus/messaging/timeout-manager.md) when a transport does not natively support delayed delivery. 
As Delayed Retries are enabled by default, using this persistence with a transport that uses the timeout manager has the risk of losing messages that have failed processing and are waiting for another retry attempt. Only use this persistence in scenarios where it is OK to lose messages.

partial: gatewaydedupe

## Saga concurrency

When simultaneously handling messages, conflicts may occur. See below for examples of the exceptions which are thrown. _[Saga concurrency](/nservicebus/sagas/concurrency.md)_ explains how these conflicts are handled, and contains guidance for high-load scenarios.

### Starting a saga

Example exception:

```
System.InvalidOperationException: The saga with the correlation id 'Name: OrderId Value: f05c6e0c-aea6-48d6-846c-d1663998ebf2' already exists
```

### Updating or deleting saga data

Non-durable persistence uses [optimistic concurrency control](https://en.wikipedia.org/wiki/Optimistic_concurrency_control) when updating or deleting saga data.

Example exception:

partial: concurrencyex
