---
title: Outbox with NHibernate Persistence
summary: How to use the outbox with NHibernate
versions: '[6.0,)'
component: NHibernate
reviewed: 2026-09-03
related:
- nservicebus/outbox
redirects:
 - nservicebus/nhibernate/outbox
---

The [outbox](/nservicebus/outbox) feature requires persistent storage to store outgoing messages and enable deduplication.

## Table

To track duplicate messages, NHibernate Persistence requires an `OutboxRecord` table.

partial: table-name

partial: modes

partial: transactionisolation

## Customizing outbox record persistence

By default, NHibernate Persistence maps outbox records as follows:

- The table has an auto-incremented integer primary key.
- The `MessageId` column has a unique index.
- The `Dispatched` and `DispatchedAt` columns have indexes.

Use the following API to map outbox data differently:

snippet: OutboxNHibernateCustomMappingConfig

snippet: OutboxNHibernateCustomMapping

When using a custom mapping, the following characteristics of the default mapping must be preserved:

- Values in the `MessageId` column must be unique. Attempting to insert a duplicate value must cause an exception.
- Queries using the `Dispatched` and `DispatchedAt` columns must be efficient. The cleanup process uses these columns to remove outdated records.

## Deduplication record lifespan

By default, NHibernate Persistence keeps deduplication records for seven days and checks for outdated records every minute.

Specify different values in the configuration file using [timestamp strings](https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-timespan-format-strings):

snippet: OutboxNHibernateTimeToKeep

To disable the cleanup task, set the `NServiceBus/Outbox/NHibernate/FrequencyToRunDeduplicationDataCleanup` app setting to `-00:00:00.001`. This value represents -1 millisecond and is equivalent to `Timeout.InfiniteTimeSpan`. Disabling cleanup on the majority of instances avoids competition when an endpoint is scaled out.

> [!NOTE]
> Run the cleanup task on only one NServiceBus endpoint instance per database. For the most efficient cleanup, disable the task on all other endpoint instances that use the database.
