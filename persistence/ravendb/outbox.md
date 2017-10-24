---
title: Outbox with RavenDB persistence
component: Raven
reviewed: 2016-08-24
versions: '[2.0,)'
tags:
 - Outbox
related:
- nservicebus/outbox
redirects:
- nservicebus/ravendb/outbox
reviewed: 2017-08-21
---

include: dtc-warning

The [Outbox](/nservicebus/outbox) feature requires persistence in order to store the messages and enable deduplication.


## Extra collections created by the RavenDB Outbox persistence

To keep track of duplicate messages, the RavenDB implementation of Outbox creates a special collection of documents called `OutboxRecord`.


## Deduplication record lifespan

The RavenDB implementation by default keeps deduplication records for 7 days and runs the purge every 1 minute.

These default settings can be changed by specifying new defaults in the settings dictionary:

snippet: OutboxRavendBTimeToKeep


partial: effect-on-docstore
