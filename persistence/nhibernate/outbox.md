---
title: Outbox with NHibernate persistence
summary: How to use the outbox with NHibernate
versions: '[6.0,)'
component: NHibernate
reviewed: 2018-05-15
tags:
 - Outbox
related:
- nservicebus/outbox
redirects:
 - nservicebus/nhibernate/outbox
---

The [outbox](/nservicebus/outbox) feature requires persistence in order to store the messages and enable deduplication.


## Table

To keep track of duplicate messages, the NHibernate implementation of outbox requires the creation of `OutboxRecord` table.

partial: customizing

## Deduplication record lifespan

The NHibernate implementation by default keeps deduplication records for 7 days and runs the purge every minute.

The default settings can be changed by specifying new defaults in the config file using [TimeStamp strings](https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-timespan-format-strings):

snippet: OutboxNHibernateTimeToKeep

By specifying a value of `-00:00:001` (i.e. 1 millisecond, the value of `Timeout.InfiniteTimeSpan`) for the `NServiceBus/Outbox/NHibernate/FrequencyToRunDeduplicationDataCleanup` appSetting, the cleanup task is disabled. This can be useful when an endpoint is scaled out and instances are competing to run the cleanup task.

NOTE: It is advised to run the cleanup task on only one NServiceBus endpoint instance per database. Disable the cleanup task on all other NServiceBus endpoint instances for the most efficient cleanup execution.
