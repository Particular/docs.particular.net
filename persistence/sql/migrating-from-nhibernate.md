---
title: Migrating from NHibernate to SQL persister
summary: Highlights differences in the database schema between the two persisters
reviewed: 2019-01-29
component: SqlPersistence
---

The database schema is not compatible between the [NHibernate](/persistence/nhibernate) and [SQL](/persistence/sql) persisters.

*Sagas*: NHibernate stores sagas as a table structure while SQL Persistence stores saga state as a JSON BLOB, similar to a key/value store. Each saga type has its own table or table structure for both NHibernate and SQL Persistence.

*Subscriptions*: NHibernate has a single table named `Subscription` where subscriptions are stored while with SQL Persistence, each logical publisher endpoint has its own subscription table, `<endpoint_name>_SubscriptionData`.

*Timeouts*: NHibernate has a `TimeoutEntity` table with an `Endpoint` column where SQL Persistence has a `<endpoint_name>_TimeoutData` table for each endpoint.

*Outbox*: NHibernate has an `OutboxRecord` table and requires a separate DB catalog, schema, or server for each logical endpoint. SQL Persistence has a table, `<endpoint_name>_OutboxData`, for each logical endpoint.

The current schemas are available via the scripting pages:

-  [NHibernate persistence SQL Server scripting](/persistence/nhibernate/scripting.md)
-  [SQL persistence SQL Server scripting](/persistence/sql/sqlserver-scripts.md)
