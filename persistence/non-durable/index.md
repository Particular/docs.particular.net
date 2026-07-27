---
title: Non-durable persistence
summary: Non-durable persistence (previously known as In-Memory persistence) stores data in a non-durable manner
component: NonDurablePersistence
reviewed: 2026-07-01
redirects:
- nservicebus/persistence/in-memory
---

partial: noteinmemory

Some scenarios require a non-durable persistence such as the development environment, testing, high-throughput scenarios where speed outweighs the benefits of durability, or a lightweight client not interested in durability across restarts:

## Persistence at a glance

For a description of each feature, see the [persistence at a glance legend](/persistence/#persistence-at-a-glance).

|Feature                    |   |
|:---                       |---
|Storage Types              |Sagas, Outbox, Subscriptions
|Transactions               |Synchronized storage session
|Concurrency control        |Optimistic concurrency
|Scripted deployment        |Does not apply
|Installers                 |Does not apply

## Configuration

Configure the endpoint to use non-durable persistence:

snippet: ConfiguringNonDurable

partial: configuration-shortcut

partial: timeoutmanager

partial: gatewaydedupe

partial: extended

## Saga concurrency

When simultaneously handling messages, conflicts may occur. See below for examples of the exceptions which are thrown. _[Saga concurrency](/nservicebus/sagas/concurrency.md)_ explains how these conflicts are handled, and contains guidance for high-load scenarios.

By default, non-durable persistence uses [optimistic concurrency control](https://en.wikipedia.org/wiki/Optimistic_concurrency_control) when updating or deleting saga data. If a conflict is detected a `InvalidOperationException` will be thrown describing the conflict.

partial: pessimistic-mode