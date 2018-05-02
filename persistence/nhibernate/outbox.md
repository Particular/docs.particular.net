---
title: Outbox with NHibernate persistence
versions: '[6.0,)'
component: NHibernate
reviewed: 2016-10-26
tags:
 - Outbox
related:
- nservicebus/outbox
redirects:
 - nservicebus/nhibernate/outbox
---

The [Outbox](/nservicebus/outbox) feature requires persistence in order to store the messages and enable deduplication.


## Table

To keep track of duplicate messages, the NHibernate implementation of Outbox requires the creation of `OutboxRecord` table.

partial: customizing

## Deduplication record lifespan

The NHibernate implementation by default keeps deduplication records for 7 days and runs the purge every 1 minute.

These default settings can be changed by specifying new defaults in the config file using [TimeStamp strings](https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-timespan-format-strings):

snippet: OutboxNHibernateTimeToKeep

By specifying a value of -1 (`Timeout.InfiniteTimeSpan`) for `SetFrequencyToRunDeduplicationDataCleanup` the cleanup task is disabled. This can be useful when an endpoint is scaled out and instances are competing to run the cleanup task.

NOTE: It is advised to run the cleanup task on only one NServiceBus endpoint instance per database and disable the cleanup task on all other NServiceBus endpoint instances for the most efficient cleanup execution.
