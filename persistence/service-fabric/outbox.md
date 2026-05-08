---
title: Service Fabric Persistence Outbox
reviewed: 2025-11-26
component: ServiceFabricPersistence
---

include: servicefabric-sunset

## Reliable collections

Outbox-related data is stored in a reliable dictionary called `outbox`.

In addition, it creates a reliable queue called `outboxCleanup`. This queue is used by the persistence to schedule cleanup for messages that have been successfully dispatched.

## Configuration

The Service Fabric implementation, by default, keeps deduplication records for 1 hour and runs cleanup logic every 30 seconds.

These values can be changed using:

snippet: ServiceFabricPersistenceOutboxConfiguration
