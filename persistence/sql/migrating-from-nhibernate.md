---
title: Migrating from NHibernate to SQL persister
summary: Learn how to migrate from NHibernate to SQL persister
reviewed: 2025-03-06
component: SqlPersistence
---

## Schema

The database schemas for [NHibernate](/persistence/nhibernate) and [SQL](/persistence/sql) persisters are not compatible.

Current schemas can be found via their respective scripting pages:

-  [NHibernate persistence SQL Server scripting](/persistence/nhibernate/scripting.md)
-  [SQL persistence SQL Server scripting](/persistence/sql/sqlserver-scripts.md)

### Sagas

- NHibernate: stores sagas as a table structure.
- SQL Persistence: stores sagas as a JSON BLOB, similar to a key/value store.
- Each saga type has its own table or table structure for both NHibernate and SQL Persistence.

### Subscriptions

- NHibernate: stores subscriptions in a single `Subscription` table.
- SQL Persistence: each logical publisher endpoint has its own subscription table, `<endpoint_name>_SubscriptionData`.

### Timeouts

NHibernate: has a `TimeoutEntity` table with an `Endpoint` column 
SQL Persistence: has a `<endpoint_name>_TimeoutData` table for each endpoint.

### Outbox

- NHibernate: has an `OutboxRecord` table shared by all endpoints, with later versions prepending the endpoint name to the MessageId. 
- SQL Persistence: creates a separate outbox table, `<endpoint_name>_OutboxData`, for each logical endpoint.

## Outbox retention

An extended retention period combined with high message throughput can cause migration to take longer. Keeping the retention period as low as possible is recommended.

- [NHibernate Deduplication record lifespan](/persistence/nhibernate/outbox.md#deduplication-record-lifespan)
- [SQL Persistence Deduplication record lifespan](/persistence/sql/outbox.md#deduplication-record-lifespan)

## Data migration

### Conversion

Subscription, timeouts, and outbox data can be converted using scripts that map between the schemas.

Saga state migration is more complex due to custom schemas that cannot be easily migrated.

### Downtime migration

Downtime migration that uses custom scripting with deep knowledge on saga state serialization differences.

 - No complex deployment
 - Requires custom saga migration mappings
 - Downtime is relative to the size of the data set
 - Pretty easy if the saga state schema is fairly flat
 - Required for outbox, timeouts, and subscriptions
