---
title: PostgreSQL Scripts
component: SqlPersistence
reviewed: 2019-08-21
related:
 - nservicebus/operations
 - persistence/sql/operational-scripting
versions: '[3,)'
---


Scripts and SQL statements used when interacting with a [PostgreSQL](https://www.postgresql.org/) database.


## Build Time

Scripts are created at build time and can be executed manually as part of a deployment or decommissioning of an endpoint.
 

### Outbox


#### Create Table

snippet: PostgreSQL_OutboxCreateSql


#### Drop Table

snippet: PostgreSQL_OutboxDropSql


### Saga

For a Saga with the following structure 

snippet: CreationScriptSaga


#### Create Table

snippet: PostgreSQL_SagaCreateSql


#### Drop Table

snippet: PostgreSQL_SagaDropSql


### Subscription


#### Create Table

snippet: PostgreSQL_SubscriptionCreateSql


#### Drop Table

snippet: PostgreSQL_SubscriptionDropSql


### Timeout


#### Create Table

snippet: PostgreSQL_TimeoutCreateSql


#### Drop Table

snippet: PostgreSQL_TimeoutDropSql


## Run Time

These are the SQL scripts used at runtime to query and update data.


### Outbox

Used at intervals to cleanup old outbox records.

snippet: PostgreSQL_OutboxCleanupSql


#### Get

Used by `IOutboxStorage.SetAsDispatched`.

snippet: PostgreSQL_OutboxGetSql


#### SetAsDispatched

Used by `IOutboxStorage.SetAsDispatched`.

snippet: PostgreSQL_OutboxSetAsDispatchedSql


#### Store

Used by `IOutboxStorage.Store`.

partial: outbox

### Saga


#### Complete

Used by `ISagaPersister.Complete`.

snippet: PostgreSQL_SagaCompleteSql


#### Save

Used by `ISagaPersister.Save`.

snippet: PostgreSQL_SagaSaveSql


#### GetByProperty

Used by `ISagaPersister.Get(propertyName...)`.

snippet: PostgreSQL_SagaGetByPropertySql


#### GetBySagaId

Used by `ISagaPersister.Get(sagaId...)`.

snippet: PostgreSQL_SagaGetBySagaIdSql


#### Update

Used by `ISagaPersister.Update`.

snippet: PostgreSQL_SagaUpdateSql


#### Select used by Saga Finder

snippet: postgresql_SagaSelectSql


### Subscriptions


#### GetSubscribers

Used by `ISubscriptionStorage.GetSubscriberAddressesForMessage`.

snippet: PostgreSQL_SubscriptionGetSubscribersSql


#### Subscribe

Used by `ISubscriptionStorage.Subscribe`.

snippet: PostgreSQL_SubscriptionSubscribeSql


#### Unsubscribe

Used by `ISubscriptionStorage.Unsubscribe`.

snippet: PostgreSQL_SubscriptionUnsubscribeSql


### Timeouts


#### Peek

Used by `IPersistTimeouts.Peek`.

snippet: PostgreSQL_TimeoutPeekSql


#### Add

Used by `IPersistTimeouts.Add`.

snippet: PostgreSQL_TimeoutAddSql


#### GetNextChunk

Used by `IQueryTimeouts.GetNextChunk`.

snippet: PostgreSQL_TimeoutNextSql

snippet: PostgreSQL_TimeoutRangeSql


#### TryRemove

Used by `IPersistTimeouts.TryRemove`.

snippet: PostgreSQL_TimeoutRemoveByIdSql


#### RemoveTimeoutBy

Used by `IPersistTimeouts.RemoveTimeoutBy`.

snippet: PostgreSQL_TimeoutRemoveBySagaIdSql
