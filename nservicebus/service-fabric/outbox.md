---
title: Service Fabric Persistence Outbox
reviewed: 2017-03-17
component: ServiceFabricPersistence
---

The [Outbox](/nservicebus/outbox) feature requires persistence in order to store the outbound messages and enable deduplication.

## Reliable collections

When using the Service Fabric Persistence in a Service Fabric reliable service, it will store this data in a reliable dictionary called `outbox`. 

Next to that, it also creates a reliable queue called `outboxCleanup`. This queue is used by the persister in order to schedule cleanup commands to the cleanup mechanism after the outbound messages have been successfully dispatched.

## Configuration

The Service Fabric implementation by default keeps deduplication records for 1 hour and runs the cleanup logic every 30 seconds.

These values can be changed using

snippet:ServiceFabricPersistenceOutboxConfiguration