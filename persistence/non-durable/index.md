---
title: Non-durable persistence
summary: Non-durable persistence (previously known as In-Memory persistence) stores data in a non-durable manner
component: NonDurablePersistence
reviewed: 2024-12-26
redirects:
- nservicebus/persistence/in-memory
---

partial: noteinmemory

Some scenarios require a non-durable persistence such as the development environment or a lightweight client not interested in durability across restarts:

## Persistence at a glance

For a description of each feature, see the [persistence at a glance legend](/persistence/#persistence-at-a-glance).

|Feature                    |   |
|:---                       |---
|Storage Types              |Sagas, Outbox, Subscriptions
|Transactions               |None
|Concurrency control        |Optimistic concurrency
|Scripted deployment        |Does not apply
|Installers                 |Does not apply

## Configuration

snippet: ConfiguringNonDurable

> [!CAUTION]
> All information stored is discarded when the process ends.

partial: timeoutmanager

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
