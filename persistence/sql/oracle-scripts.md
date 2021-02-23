---
title: Oracle Scripts
component: SqlPersistence
reviewed: 2020-11-26
related:
 - nservicebus/operations
 - persistence/sql/operational-scripting
redirects:
 - nservicebus/sql-persistence/oracle-scripts
versions: '[2,)'
---


Scripts and SQL used when interacting with a [Oracle](https://www.oracle.com/database/index.html) database.


## Build Time

Scripts created at build time and executed as part of a deployment or decommissioning of an endpoint.


### Outbox


#### Create Table

snippet: Oracle_OutboxCreateSql


#### Drop Table

snippet: Oracle_OutboxDropSql


### Saga

For a Saga with the following structure

snippet: CreationScriptSaga


#### Create Table

snippet: Oracle_SagaCreateSql


#### Drop Table

snippet: Oracle_SagaDropSql


### Subscription


#### Create Table

snippet: Oracle_SubscriptionCreateSql


#### Drop Table

snippet: Oracle_SubscriptionDropSql


### Timeout


#### Create Table

snippet: Oracle_TimeoutCreateSql


#### Drop Table

snippet: Oracle_TimeoutDropSql


## Run Time

SQL used at runtime to query and update data.


### Outbox

Used at intervals to cleanup old outbox records.

snippet: Oracle_OutboxCleanupSql


#### Get

Used by `IOutboxStorage.SetAsDispatched`.

snippet: Oracle_OutboxGetSql


#### SetAsDispatched

Used by `IOutboxStorage.SetAsDispatched`.

snippet: Oracle_OutboxSetAsDispatchedSql


#### Store

Used by `IOutboxStorage.Store`.

partial: outbox


### Saga


#### Complete

Used by `ISagaPersister.Complete`.

snippet: Oracle_SagaCompleteSql


#### Save

Used by `ISagaPersister.Save`.

snippet: Oracle_SagaSaveSql


#### GetByProperty

Used by `ISagaPersister.Get(propertyName...)`.

snippet: Oracle_SagaGetByPropertySql


#### GetBySagaId

Used by `ISagaPersister.Get(sagaId...)`.

snippet: Oracle_SagaGetBySagaIdSql


#### Update

Used by `ISagaPersister.Update`.

snippet: Oracle_SagaUpdateSql


### Subscription


#### GetSubscribers

Used by `ISubscriptionStorage.GetSubscriberAddressesForMessage`.

snippet: Oracle_SubscriptionGetSubscribersSql


#### Subscribe

Used by `ISubscriptionStorage.Subscribe`.

snippet: Oracle_SubscriptionSubscribeSql


#### Unsubscribe

Used by `ISubscriptionStorage.Unsubscribe`.

snippet: Oracle_SubscriptionUnsubscribeSql


### Timeout


#### Peek

Used by `IPersistTimeouts.Peek`.

snippet: Oracle_TimeoutPeekSql


#### Add

Used by `IPersistTimeouts.Add`.

snippet: Oracle_TimeoutAddSql


#### GetNextChunk

Used by `IQueryTimeouts.GetNextChunk`.

snippet: Oracle_TimeoutNextSql

snippet: Oracle_TimeoutRangeSql


#### TryRemove

Used by `IPersistTimeouts.TryRemove`.

snippet: Oracle_TimeoutRemoveByIdSql


#### RemoveTimeoutBy

Used by `IPersistTimeouts.RemoveTimeoutBy`.

snippet: Oracle_TimeoutRemoveBySagaIdSql
