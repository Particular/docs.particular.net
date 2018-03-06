---
title: Outbox with RavenDB persistence
component: Raven
reviewed: 2018-02-19
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

The [Outbox](/nservicebus/outbox) feature requires persistence in order to store messages and enable deduplication.


## Extra collections created by the RavenDB Outbox persistence

To keep track of duplicate messages, the RavenDB implementation of Outbox creates a special collection of documents called `OutboxRecord`.


## Deduplication record lifespan

The RavenDB implementation by default keeps deduplication records for 7 days and runs the purge every 1 minute.

These default settings can be changed by specifying new defaults in the settings dictionary:

snippet: OutboxRavendBTimeToKeep

By specifying a value of -1 (`Timeout.InfiniteTimeSpan`) for `SetFrequencyToRunDeduplicationDataCleanup` the cleanup task is disabled. This can be useful when an endpoint is scaled out and instances are competing to run the cleanup task.

NOTE: It is advised to run the cleanup task on only one NServiceBus endpoint instance per RavenDB database and disable the cleanup task on all other NServiceBus endpoint instances for the most efficient cleanup execution.

partial: effect-on-docstore

