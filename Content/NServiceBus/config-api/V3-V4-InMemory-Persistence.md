---
title: Configuration API InMemory Persistence in V3 and V4
summary: Configuration API InMemory Persistence in V3 and V4
tags:
- NServiceBus
- Fluent Configuration
- V3
- V4
---

Some scenarios require an in-memory persistence configuration, such as the development environment or a lightweight client not interested in durability across restarts:

* `InMemoryFaultManagement()`: configures the fault manager to run in memory.
* `InMemorySagaPersister()`: configures the saga persistence to run in memory.
* `InMemorySubscriptionStorage()`: configures the subscription manager to persist subscriptions in memory.

Details of all the persistence options are in the [Persistence in NServiceBus](persistence-in-nservicebus) article.