---
title: Outbox with NHibernate Persistence
summary: How to use the outbox with NHibernate
versions: '[6.0,)'
component: NHibernate
reviewed: 2025-02-03
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

partial: transactionisolation

## Customizing outbox record persistence

By default the outbox records are persisted in the following way:

- The table has an auto-incremented integer primary key.
- The `MessageId` column has a unique index.
- There are indices on `Dispatched` and `DispatchedAt` columns.

The following API can be used to provide a different mapping of outbox data to the underlying storage:

snippet: OutboxNHibernateCustomMappingConfig

snippet: OutboxNHibernateCustomMapping

If custom mapping is required, the following characteristics of the original mapping must be preserved:

- Values stored in the `MessageId` column must be unique and an attempt to insert a duplicate entry must cause an exception.
- Querying by `Dispatched` and `DispatchedAt` columns must be efficient because these columns are used by the cleanup process to remove outdated records.

## Deduplication record lifespan

By default, the NHibernate implementation keeps deduplication records for seven days and checks for purgeable records every minute.

Specify different values in the config file using [timestamp strings](https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-timespan-format-strings):

snippet: OutboxNHibernateTimeToKeep

By specifying a value of `-00:00:00.001` (i.e. -1 millisecond, the value of `Timeout.InfiniteTimeSpan`) for the `NServiceBus/Outbox/NHibernate/FrequencyToRunDeduplicationDataCleanup` app settings, the cleanup task is disabled. This is useful when an endpoint is scaled out and instances are competing to run the cleanup task.

> [!NOTE]
> It is advised to run the cleanup task on only one NServiceBus endpoint instance per database. Disable the cleanup task on all other NServiceBus endpoint instances for the most efficient cleanup execution.
