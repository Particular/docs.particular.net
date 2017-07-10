---
title: SQL Persistence Saga rename
summary: Renaming a Saga that is stored in the SQL Persistence
reviewed: 2017-03-06
component: SqlPersistence
tags:
 - Saga
 - Timeout
related:
 - nservicebus/sagas
---


This sample shows a two sagas that need to be renamed.

The sagas implementations are rather contrived and their implementation exists only to illustrate the renaming of sagas. The functionality is, for sample purposes, split between the two versions. Saga starting logic existing in one endpoint and the handling logic existing in the other endpoint.

The sample consists of two Versions a single endpoint. The first version will start the sagas. The second version will handle the renaming of the sagas.


include: sqlpersistence-prereqs


## Timeout Saga

The timeout saga sends a timeout at startup and then handles that timeout. 

This Saga will be renamed from `MyNamespace1.MyTimeoutSagaVersion1` to `MyNamespace2.MyTimeoutSagaVersion2`.

This scenario is necessary to illustrate how, when the timeout message is received, its header needs to be translated over the new saga name.

The saga type is stored in the Headers of the TimeoutData Table ([Sql Server](/persistence/sql/sqlserver-scripts.md#build-time-timeout-create-table) and [MySql](/persistence/sql/mysql-scripts.md#build-time-timeout-create-table)) but will be converted back to a [message header](/nservicebus/messaging/headers.md#saga-related-headers-requesting-a-timeout-from-a-saga) when the timeout is executed.


## Reply Saga

The timeout saga sends a request at startup and then handles the response for that message. 


This Saga will be renamed from `MyNamespace1.MyReplySagaVersion1` to `MyNamespace2.MyReplySagaVersion2`.

This scenario is necessary to illustrate how, when the [reply message](/nservicebus/messaging/headers.md#saga-related-headers-replying-to-a-saga) is received, its header needs to be translated over the new saga name.


## Projects

The sample consists of several projects:


### Shared

The shared message contracts and common configuration for both versions. This project only exists for the purposes of code reuse between the two endpoint versions. In a normal codebase this project would not be required as both versions of sagas would never co-exist in the same solution.


#### Endpoint Configuration

Share endpoint configuration.

snippet: endpointConfig


#### Reply Handler

A reply handler which takes a request and sends a reply. In this case the reply will always be to a saga. It also duplicates the [OriginatingSagaType Header](/nservicebus/messaging/headers.md#saga-related-headers) on to the message for logging purposes when the reply is executed on the saga.

snippet: replyHandler


### EndpointVersion1

This project contains the first versions of the sagas. It also sends message to start both saga at startup.


#### Starting Sagas

Send message to start both the Reply Saga and the Timeout Saga.

snippet: startSagas


#### Reply Saga

At startup Sends a new Request message. The message is delayed to give EndpointVersion1 time to shut down prior to attempting to handle the Reply message.

The Handle method for the Reply message throws an exception since in this samples execution steps that method should never be executed.

snippet: replySaga1


#### Timeout Saga

At startup requests a SagaTimeout. The timeout is delayed to give EndpointVersion1 time to shut down prior to attempting to handle the timeout.

The Handle method for the SagaTimeout message throws an exception since in this samples execution steps that method should never be executed.

snippet: timeoutSaga1


###  EndpointVersion2

This project contains the second versions of the sagas.


#### Reply Saga

Handles the Reply message that has been sent from `MyReplySagaVersion1`.

The Handle method for the StartReplySaga message throws an exception since in this samples execution steps that method should never be executed.

snippet: replySaga2


#### Timeout Saga

Handles the timeout that has been sent from `MyTimeoutSagaVersion1`.

The Handle method for the StartTimeoutSaga message throws an exception since in this samples execution steps that method should never be executed.

snippet: timeoutSaga2


#### Timeout Saga

snippet: renameTables

WARNING: In a production scenario this code would be executed as part of an endpoint migration prior to starting the new version of the endpoint.


#### Mutator

The mutator is a [Incoming Transport Mutator](/nservicebus/pipeline/message-mutators.md#transport-messages-mutators-imutateincomingtransportmessages) that translates [saga reply headers](/nservicebus/messaging/headers.md#saga-related-headers-replying-to-a-saga) on incoming messages to the new saga names. 

This is required to handle the following scenario

 * When the EndpointVersion1 is shut down there can still be messages on the wire or pending timeouts stored in the database.
 * These messages and timeouts have headers which will correlate to the old version of the sagas.
 * When the EndpointVersion2 is started these messages and timeouts will be processed but the headers need to be translated to be the new version of the sagas.

snippet: mutator

DANGER: This mutator must remain in place until all messages and timeouts (that target the old saga versions) are processed.

For reply messages starting the saga a safe period of time to leave the mutator in places is the [configured discard time](/nservicebus/messaging/discard-old-messages.md) for those messages. Note that this period may be superseded by [messages in the error queue](/nservicebus/recoverability/configure-error-handling.md) being retied.

For timeouts targeting the saga a safe period of time to leave the mutator in places is dependent one the lifetime of the given saga. That is, dependent on the business rules of a give saga, how long it is expected to exist before it is [marked as complete](/nservicebus/sagas/#ending-a-long-running-process). Alternatively the TimeoutData table can be queried, using [json_value](https://docs.microsoft.com/en-us/sql/t-sql/functions/json-value-transact-sql), to check if there are any pending timeouts that target the old saga:

```sql
select Id
from Samples_RenameSaga_TimeoutData
where json_value(Headers,'$."NServiceBus.OriginatingSagaType"')
    like 'MyNamespace1.MyTimeoutSagaVersion1%'
```

If a saga is extremely long running then the data can be migrated programmatically in SQL using [json_modify](https://docs.microsoft.com/en-us/sql/t-sql/functions/json-modify-transact-sql) and [json_value](https://docs.microsoft.com/en-us/sql/t-sql/functions/json-value-transact-sql). For example:

```sql
update Samples_RenameSaga_TimeoutData
set Headers = json_modify(
        Headers,
        '$."NServiceBus.OriginatingSagaType"',
        'MyNamespace1.MyTimeoutSagaVersion1, Endpoint, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null')
where json_value(Headers, '$."NServiceBus.OriginatingSagaType"')
    like 'MyNamespace1.MyTimeoutSagaVersion1%'
```


##### Registration

The mutator is registered at endpoint startup.

snippet: registerMutator