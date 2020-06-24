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