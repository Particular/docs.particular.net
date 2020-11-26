---
title: Service Fabric Persistence Outbox
reviewed: 2020-11-26
component: ServiceFabricPersistence
---


## Reliable collections

Outbox related data is stored in a reliable dictionary called `outbox`.

In addition to that, it also creates a reliable queue called `outboxCleanup`. This queue is used by the persistence to schedule cleanup for messages that have been successfully dispatched.


## Configuration

The Service Fabric implementation by default keeps deduplication records for 1 hour and runs the cleanup logic every 30 seconds.

These values can be changed using:

snippet: ServiceFabricPersistenceOutboxConfiguration
