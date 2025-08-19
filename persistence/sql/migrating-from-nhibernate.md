---
title: Migrating from NHibernate to SQL persister
summary: Learn how to migrate from NHibernate to SQL persister
reviewed: 2025-03-13
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

### Subscriptions

- NHibernate: stores subscriptions in a single `Subscription` table.
- SQL Persistence: each logical publisher endpoint has its own subscription table, `<endpoint_name>_SubscriptionData`.

### Timeouts

- NHibernate: has a `TimeoutEntity` table with an `Endpoint` column 
- SQL Persistence: has a `<endpoint_name>_TimeoutData` table for each endpoint.

### Outbox

- NHibernate: has an `OutboxRecord` table shared by all endpoints, with later versions prepending the endpoint name to the MessageId. 
- SQL Persistence: creates a separate outbox table, `<endpoint_name>_OutboxData`, for each logical endpoint.

## Outbox retention

An extended retention period combined with high message throughput can cause migration to take longer. Keeping the retention period as low as possible is recommended.

- [NHibernate Deduplication record lifespan](/persistence/nhibernate/outbox.md#deduplication-record-lifespan)
- [SQL Persistence Deduplication record lifespan](/persistence/sql/outbox.md#deduplication-record-lifespan)

## Data migration

### Conversion

Subscription, timeouts, and outbox data have well-defined schemas that can be converted using scripts to map between the two persisters. However, saga state migration is more complex because of the need to convert from table structures to JSON blobs.

### Migration downtime

Migration downtime is influenced by a variety of factors:

 - The complexity of the deployment required
 - The need for and use of custom saga migration mappings
 - The size of the data set
 - The complexity of the structure of the saga state
 - Whether outbox, timeouts, and subscriptions need to be migrated
