---
title: Outbox with SQL persistence
component: SqlPersistence
reviewed: 2020-03-09
related:
- nservicebus/outbox
---

The [outbox](/nservicebus/outbox) feature requires persistence in order to store the messages and enable deduplication.


## Table

To keep track of duplicate messages, the SQL persistence implementation of outbox requires the creation of dedicated outbox tables. The names of the outbox tables are generated automatically according to the rules for a given SQL dialect, for example the maximum name length limit.

See scripts used for table creation to learn more: [MS SQL Server](/persistence/sql/sqlserver-scripts.md#build-time-outbox-create-table), [Oracle](/persistence/sql/oracle-scripts.md#build-time-outbox-create-table), [MySQL](/persistence/sql/mysql-scripts.md#build-time-outbox-create-table) and [PostgreSQL](/persistence/sql/postgresql-scripts.md#build-time-outbox-create-table).

partial: modes

## Deduplication record lifespan

By default, the SQL persistence implementation keeps deduplication records for 7 days and runs the purge every minute.

partial: settings

In scaled-out environments, endpoint instances compete to execute outbox cleanup, which can result in occasional conflicts. There are a few options available to minimize this:

- Run the cleanup on only a single instance.
- Increase the cleanup interval so that, on average, one endpoint instance cleans up as often as a single instance would normally. The cleanup timer isn't strict and over time these will drift and will cause less overlap. For example, for 10 endpoint instances, let cleanup run every 10 minutes instead of every minute.
- Disable cleanup on all instances and have cleanup run as a scheduled job in the database.
