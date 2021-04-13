---
title: Outbox with RavenDB persistence
component: Raven
reviewed: 2019-06-10
versions: '[2.0,)'
related:
- nservicebus/outbox
redirects:
- nservicebus/ravendb/outbox
---

include: dtc-warning

include: cluster-configuration-warning

The [Outbox](/nservicebus/outbox) feature requires persistence in order to store messages and enable deduplication.


## Extra collections created by the RavenDB Outbox persistence

To keep track of duplicate messages, the RavenDB implementation of Outbox creates a special collection of documents called `OutboxRecord`.


## Deduplication record lifespan

The RavenDB implementation by default keeps deduplication records for 7 days and runs the purge every 1 minute.

These default settings can be changed by specifying new defaults in the settings dictionary:

snippet: OutboxRavendBTimeToKeep

partial: cleanup

partial: effect-on-docstore