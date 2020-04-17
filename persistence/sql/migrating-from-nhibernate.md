---
title: Migrating from NHibernate to SQL persister
summary: Highlights differences in the database schema between the two persisters and migration options
reviewed: 2020-03-04
component: SqlPersistence
---

## Schema

The database schema is not compatible between the [NHibernate](/persistence/nhibernate) and [SQL](/persistence/sql) persisters.

The current schemas are available via the scripting pages:

-  [NHibernate persistence SQL Server scripting](/persistence/nhibernate/scripting.md)
-  [SQL persistence SQL Server scripting](/persistence/sql/sqlserver-scripts.md)


### Sagas

NHibernate stores sagas as a table structure while SQL Persistence stores saga state as a JSON BLOB, similar to a key/value store. Each saga type has its own table or table structure for both NHibernate and SQL Persistence.

### Subscriptions

NHibernate has a single table named `Subscription` where subscriptions are stored while with SQL Persistence, each logical publisher endpoint has its own subscription table, `<endpoint_name>_SubscriptionData`.

### Timeouts

NHibernate has a `TimeoutEntity` table with an `Endpoint` column where SQL Persistence has a `<endpoint_name>_TimeoutData` table for each endpoint.

### Outbox

NHibernate has an `OutboxRecord` table shared by all endpoints, where in later versions the endpoint name is prepended to the MessageId. SQL Persistence has a table, `<endpoint_name>_OutboxData`, for each logical endpoint.

## Outbox retention

If the outbox retention period is set to a very large period and the message throughput is high then such a migration will take a while to complete. It is recommended to keep the retention period as low as possible. 

- [NHibernate Deduplication record lifespan](/persistence/nhibernate/outbox.md#deduplication-record-lifespan)
- [SQL Persistence Deduplication record lifespan](/persistence/sql/outbox.md#deduplication-record-lifespan)

## Data migration

### Conversion

Subscription, timeouts, and outbox data can be converted using script that can map between the schema differences.

Saga state has custom schemas which cannot be easily migrated.

### Downtime migration

Downtime migration that uses custom scripting with deep knowledge on saga state serialization differences.

 - No complex deployment
 - Requires custom saga migration mappings
 - Downtime is relative to the size of the data set
 - Pretty easy if the saga state schema is fairly flat
 - Required for outbox,  timeouts, and subscriptions
 
### Runtime migration

Runtime migration means that both NHibernate and SQL Persistence are used. New saga instances are created via SQL Persistence while legacy instances stored using NHibernate eventually will be removed once these saga instance complete.

The [Saga migration from NHibernate to SQL Persistence sample](/samples/saga/migration/) shows how this can be achieved.

- Complex deployment
- Requires saga logic to be mainted for both persisters
- Does not require serialization knowledge
- No downtime
- Not very suitable for saga instances that never complete or when instances complete far into the future
