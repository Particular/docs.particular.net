---
title: Non-durable Persistence
summary: Non-durable persistence stores data in a non-durable manner for development-time only
component: NonDurablePersistence
reviewed: 2020-10-06
redirects:
- nservicebus/persistence/non-durable
---

Some scenarios require an non-durable persistence such as the development environment or a lightweight client not interested in durability across restarts:

snippet: ConfiguringNonDurable

DANGER: All information stored in the Non-Durable persistence is discarded when the process ends.

NOTE: The [Delayed Retries](/nservicebus/recoverability/#delayed-retries) mechanism uses the [timeout manager](/nservicebus/messaging/timeout-manager.md) when a transport does not natively support delayed delivery. As Delayed Retries are enabled by default, using Non-durable persistence with a transport that uses the timeout manager has the risk of losing messages that have failed processing and are waiting for another retry attempt. Use Non-durable persistence only in scenarios where it is OK to lose messages.

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

```
System.Exception: NonDurableSagaPersister concurrency violation: saga entity Id[a15a31fd-4f25-4dc3-b556-aad200e52dcb] already saved.
```