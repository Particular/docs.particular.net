---
title: Outbox with NHibernate Persistence
summary: How to use the outbox with NHibernate
versions: '[6.0,)'
component: NHibernate
reviewed: 2020-02-18
related:
- nservicebus/outbox
redirects:
 - nservicebus/nhibernate/outbox
---

The [outbox](/nservicebus/outbox) feature requires persistent storage in order to store the messages and enable deduplication.


## Table

To keep track of duplicate messages, the NHibernate implementation of the outbox requires the creation of an `OutboxRecord` table.

partial: table-name

partial: modes

partial: customizing

## Deduplication record lifespan

By default, the NHibernate implementation keeps deduplication records for seven days and checks for purgeable records every minute.

Specify different values in the config file using [timestamp strings](https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-timespan-format-strings):

snippet: OutboxNHibernateTimeToKeep

By specifying a value of `-00:00:00.001` (i.e. 1 millisecond, the value of `Timeout.InfiniteTimeSpan`) for the `NServiceBus/Outbox/NHibernate/FrequencyToRunDeduplicationDataCleanup` app settings, the cleanup task is disabled. This is useful when an endpoint is scaled out and instances are competing to run the cleanup task.

NOTE: It is advised to run the cleanup task on only one NServiceBus endpoint instance per database. Disable the cleanup task on all other NServiceBus endpoint instances for the most efficient cleanup execution.