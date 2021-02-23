---
title: MySql Scripts
component: SqlPersistence
reviewed: 2020-07-16
related:
 - nservicebus/operations
 - persistence/sql/operational-scripting
redirects:
 - nservicebus/sql-persistence/mysql-scripts
---


Scripts and SQL used when interacting with a [MySql](https://www.mysql.com/) database.


## Build Time

Scripts are created at build time and are executed as part of a deployment or decommissioning of an endpoint.
 

### Outbox


#### Create Table

snippet: MySql_OutboxCreateSql


#### Drop Table

snippet: MySql_OutboxDropSql


### Saga

For a Saga with the following structure 

snippet: CreationScriptSaga


#### Create Table

snippet: MySql_SagaCreateSql


#### Drop Table

snippet: MySql_SagaDropSql


### Subscription


#### Create Table

snippet: MySql_SubscriptionCreateSql


#### Drop Table

snippet: MySql_SubscriptionDropSql


### Timeout


#### Create Table

snippet: MySql_TimeoutCreateSql


#### Drop Table

snippet: MySql_TimeoutDropSql


## Run Time

SQL used at runtime to query and update data.


### Outbox

Used at intervals to cleanup old outbox records.

snippet: MySql_OutboxCleanupSql


#### Get

Used by `IOutboxStorage.SetAsDispatched`.

snippet: MySql_OutboxGetSql


#### SetAsDispatched

Used by `IOutboxStorage.SetAsDispatched`.

snippet: MySql_OutboxSetAsDispatchedSql


#### Store

Used by `IOutboxStorage.Store`.

partial: outbox


### Saga


#### Complete

Used by `ISagaPersister.Complete`.

snippet: MySql_SagaCompleteSql


#### Save

Used by `ISagaPersister.Save`.

snippet: MySql_SagaSaveSql


#### GetByProperty

Used by `ISagaPersister.Get(propertyName...)`.

snippet: MySql_SagaGetByPropertySql


#### GetBySagaId

Used by `ISagaPersister.Get(sagaId...)`.

snippet: MySql_SagaGetBySagaIdSql


#### Update

Used by `ISagaPersister.Update`.

snippet: MySql_SagaUpdateSql


partial: finder


### Subscription


#### GetSubscribers

Used by `ISubscriptionStorage.GetSubscriberAddressesForMessage`.

snippet: MySql_SubscriptionGetSubscribersSql


#### Subscribe

Used by `ISubscriptionStorage.Subscribe`.

snippet: MySql_SubscriptionSubscribeSql


#### Unsubscribe

Used by `ISubscriptionStorage.Unsubscribe`.

snippet: MySql_SubscriptionUnsubscribeSql


### Timeout


#### Peek

Used by `IPersistTimeouts.Peek`.

snippet: MySql_TimeoutPeekSql


#### Add

Used by `IPersistTimeouts.Add`.

snippet: MySql_TimeoutAddSql


#### GetNextChunk

Used by `IQueryTimeouts.GetNextChunk`.

snippet: MySql_TimeoutNextSql

snippet: MySql_TimeoutRangeSql


#### TryRemove

Used by `IPersistTimeouts.TryRemove`.

snippet: MySql_TimeoutRemoveByIdSql


#### RemoveTimeoutBy

Used by `IPersistTimeouts.RemoveTimeoutBy`.

snippet: MySql_TimeoutRemoveBySagaIdSql
