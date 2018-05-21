---
title: Outbox with SQL persistence
component: SqlPersistence
reviewed: 2017-11-29
tags:
 - Outbox
related:
- nservicebus/outbox
---

The [Outbox](/nservicebus/outbox) feature requires persistence in order to store the messages and enable deduplication.


## Table

To keep track of duplicate messages, the SQL persistence implementation of Outbox requires the creation of the dedicated Outbox tables. The names of the Outbox tables are generated automatically according to the rules specific for a given SQL dialect, for example the maximum name length limit.

See scripts used for table creation to learn more: [MS SQL Server](/persistence/sql/sqlserver-scripts.md#build-time-outbox-create-table), [Oracle](/persistence/sql/oracle-scripts.md#build-time-outbox-create-table), [MySQL](/persistence/sql/mysql-scripts.md#build-time-outbox-create-table) and [PostgreSQL](/persistence/sql/postgresql-scripts.md#build-time-outbox-create-table).


## Deduplication record lifespan

The SQL persistence implementation by default keeps deduplication records for 7 days and runs the purge every minute.

partial: settings