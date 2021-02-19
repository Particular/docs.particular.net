---
title: MS SQL Server Scripts
component: SqlPersistence
reviewed: 2020-07-16
related:
 - nservicebus/operations
 - persistence/sql/operational-scripting
redirects:
 - nservicebus/sql-persistence/sqlserver-scripts
---


Scripts and SQL used when interacting with a [SQL Server](https://www.microsoft.com/en-au/sql-server/) database.


## Build Time

Scripts are created at build time and are executed as part of a deployment or decommissioning of an endpoint.


### Outbox


#### Create Table

snippet: MsSqlServer_OutboxCreateSql


#### Drop Table

snippet: MsSqlServer_OutboxDropSql


### Saga

For a Saga with the following structure 

snippet: CreationScriptSaga


#### Create Table

snippet: MsSqlServer_SagaCreateSql


#### Drop Table

snippet: MsSqlServer_SagaDropSql


### Subscription


#### Create Table

snippet: MsSqlServer_SubscriptionCreateSql


#### Drop Table

snippet: MsSqlServer_SubscriptionDropSql


### Timeout


#### Create Table

snippet: MsSqlServer_TimeoutCreateSql


#### Drop Table

snippet: MsSqlServer_TimeoutDropSql


## Run Time

SQL used at runtime to query and update data.


### Outbox

Used at intervals to cleanup old outbox records.

snippet: MsSqlServer_OutboxCleanupSql


#### Get

Used by `IOutboxStorage.SetAsDispatched`.

snippet: MsSqlServer_OutboxGetSql


#### SetAsDispatched

Used by `IOutboxStorage.SetAsDispatched`.

snippet: MsSqlServer_OutboxSetAsDispatchedSql


#### Store

Used by `IOutboxStorage.Store`.

partial: outbox


### Saga


#### Complete

Used by `ISagaPersister.Complete`.

snippet: MsSqlServer_SagaCompleteSql


#### Save

Used by `ISagaPersister.Save`.

snippet: MsSqlServer_SagaSaveSql


#### GetByProperty

Used by `ISagaPersister.Get(propertyName...)`.

snippet: MsSqlServer_SagaGetByPropertySql


#### GetBySagaId

Used by `ISagaPersister.Get(sagaId...)`.

snippet: MsSqlServer_SagaGetBySagaIdSql


#### Update

Used by `ISagaPersister.Update`.

snippet: MsSqlServer_SagaUpdateSql


partial: finder


### Subscription


#### GetSubscribers

Used by `ISubscriptionStorage.GetSubscriberAddressesForMessage`.

snippet: MsSqlServer_SubscriptionGetSubscribersSql


#### Subscribe

Used by `ISubscriptionStorage.Subscribe`.

snippet: MsSqlServer_SubscriptionSubscribeSql


#### Unsubscribe

Used by `ISubscriptionStorage.Unsubscribe`.

snippet: MsSqlServer_SubscriptionUnsubscribeSql


### Timeout


#### Peek

Used by `IPersistTimeouts.Peek`.

snippet: MsSqlServer_TimeoutPeekSql


#### Add

Used by `IPersistTimeouts.Add`.

snippet: MsSqlServer_TimeoutAddSql


#### GetNextChunk

Used by `IQueryTimeouts.GetNextChunk`.

snippet: MsSqlServer_TimeoutNextSql

snippet: MsSqlServer_TimeoutRangeSql


#### TryRemove

Used by `IPersistTimeouts.TryRemove`.

snippet: MsSqlServer_TimeoutRemoveByIdSql


#### RemoveTimeoutBy

Used by `IPersistTimeouts.RemoveTimeoutBy`.

snippet: MsSqlServer_TimeoutRemoveBySagaIdSql
