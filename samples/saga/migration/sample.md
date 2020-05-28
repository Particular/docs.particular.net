---
title: Migrating saga persistence
summary: How to migrate from one type of saga persistence to another without an off-line migration procedure
component: Core
reviewed: 2019-10-04
related:
- nservicebus/sagas
- persistence/nhibernate
- persistence/sql
---

When saga data is involved, migrating from one type of persistence to another can seem like an insurmountable task, since each persister stores NServiceBus saga data in different ways that would make creating a migration script difficult. Even if the script could be written, executing it may require lengthy downtime, possible with no ability to revert if something went wrong.

This sample takes a different approach, by showing how to migrate from one saga persister to another in a gradual fashion, without requiring an offline migration script. This is accomplished by running two versions of the endpoint in parallel and forwarding messages where the saga data cannot be found to the new endpoint.

This sample shows how to migrate from [NHibernate](/persistence/nhibernate/) to [SQL Persistence](/persistence/sql/), but the same concept can be used to migrate between any two persistence options.

downloadbutton

## Prerequisites

include: sql-prereq

The database created by this sample is `NsbSamplesSagaMigration`.


## Running the project

 1. Start the solution.
 1. Wait until `Type 'start <SagaId>' or 'complete <SagaId>'` is shown in the "Client" console window.
 1. Start a couple of sagas with easy to remember IDs (e.g. `start 1`, `start 2` and `start 3`).
 1. Verify sagas are started by running `SELECT * FROM [NsbSamplesSagaMigration].[dbo].[TestSaga]`.
 1. Verify that the messages were handled by the "Server" endpoint.
 1. Complete one of the sagas (e.g. `complete 1`) and observe the message flow. It can take up to 10 seconds to complete as the flow involves a saga timeout. The result should be completion of a saga (verify by checking that a corresponding row is removed from the saga table by re-running the previous query).
 1. Stop the solution.
 1. Uncomment the `#define MIGRATION` line in `TestSaga.cs`.
 1. Start the solution.
 1. Start and complete some new sagas (e.g. `start A`, `start B` and `start C`).
 1. Verify sagas are started by running `SELECT * FROM [NsbSamplesSagaMigration].[dbo].[NewTestSaga]`.
 1. Verify that the messages were handled by the "Server.New" endpoint.
 1. Notice that the "Server" console shows information indicating that the not-found handler has been used.
 1. Complete the previously created sagas (`complete 2` or `complete 3`) to drain the saga store.
 1. Verify the messages are handled by the old "Server" endpoint, not the "Server.New" endpoint.
 1. Complete one of the new sagas (e.g. `complete A`) to verify it is handled properly by "Server.New"
 1. Complete another saga (e.g. `complete B`) and stop the solution as soon as `Got a follow-up message.` is shown in the console.
 1. Run `SELECT [Destination], [SagaId] FROM [NsbSamplesSagaMigration].[dbo].[NewTimeoutData]` to verify the timeout is stored in the database and the destination is the "Server.New" queue.
 1. Uncomment the `#define POST_MIGRATION` line in `Program.cs` and `DrainTempQueueSatelliteFeature.cs` of "Server.New". This changes the input queue of "Server.New" back to the well-known `Samples.SagaMigration.Server` and enables an additional receiver that drains the temporary queue.
 1. Start only the "Server.New" project by right-clicking the project in Solution Explorer and selecting "Debug -> Start new instance".
 1. Notice "Server.New" prints `Moving message from Samples.SagaMigration.Server.New@<machine> to Samples.SagaMigration.Server@<machine>` and then `Got timeout. Completing.` which means the timeout has been successfully redirected from the temporary queue. This happens only if there were outstanding timeout messages present when new version of the endpoint replaced the old one.


## Code walk-through

This sample contains four projects:

 * Shared - contains definitions of messages exchanged between the Client and the Server.
 * Client - initiates a multi-message conversation with the server.
 * Server - implements a long running process via the Saga feature. Uses the [NHibernate-based](/persistence/nhibernate) saga persister.
 * Server.New - implements the same functionality as Server but uses the [SQL-based](/persistence/sql) saga persister.

The sample shows how to gradually migrate from one saga persister to another without requiring an off-line migration procedure/script. In this example NHibernate and SQL persisters are the source and target respectively but any persister can be used in any role i.e. the same method can be used to migrate, for example, from RavenDB to NHibernate.


### Message flow

The message flow is designed to demonstrate the correctness of migration logic:

 * The `StartingMessage` (sent via `start` command) starts the saga and assigns the correlation property.
 * The `CorrelatedMessage` (sent via `complete` command) contains the correlation property value of an already started saga. Handling of this message results in sending back a `ReplyMessage`
 * The `ReplyMessage` sent by the saga contains the saga ID header containing the storage ID (not the correlation property) of the saga instance that sent it
 * The `ReplyFollowUpMessage` send as a response to `ReplyMessage` contains the mentioned saga ID header by which the target saga is being looked up

snippet: Handlers

To summarize, sagas can be looked up by either their correlation property value or the storage ID.


### How it works

The migration procedure require that the new version of a given endpoint is deployed alongside the old one. In this sample "Server.New" represents the new version that uses SQL persistence. Before the migration is complete the new version is not visible to the outside world because it uses a different queue name.

The new version must be deployed and started before the migration process can begin. Only then can the old version be modified to enable the migration. This requires a couple of code changes. In this sample both versions share the same source code (though copied to different files) for the saga definition and the changes are done via pre-processor instructions. Enabling the migration happens by un-commenting this statement:

```
//#define MIGRATION
```


#### Handling not found sagas

In order to eventually migrate to the new persistence, all new saga instances need to be created by the "Server.New" endpoint. To ensure this, the old "Server" endpoint has to include a handler for not found sagas that forwards the messages to the new endpoint:

snippet: Forwarder

The messages are forwarded as-is, without any side effects and handled normally by the destination.

This handler is only invoked for messages that target an existing saga (either by correlation or by having a saga ID header). It is never invoked for a message that can start a saga so the saga code has to be modified to not treat `StartingMessage` as a saga started in the old endpoint.

snippet: Header

Notice `DummyMessage` is necessary as a saga starter. NServiceBus validation logic does not allow sagas without any starter messages. `DummyMessage` is never sent. It is only there so satisfy the validation.

The correlation property mappings also need to include `DummyMessage`

snippet: Mappings


#### Forwarding messages from temporary queue

When there are no sagas left in the old persistence, the old version of the endpoint can be decommissioned. In order to not lose any messages, the new version has to include, for a certain period of time, a redirection satellite that receives any remaining messages from the temporary queue and moves it to the regular queue.

snippet: DrainTempQueueSatellite


#### Decommissioning the old endpoint

Once all the sagas stored in the old persister are complete, the old endpoint can be decommissioned.

 1. Ensure the saga table is empty by running a command similar to `SELECT * FROM [NsbSamplesSagaMigration].[dbo].[TestSaga]`.
 1. Stop the old endpoint ("Server").
 1. Update the binaries and/or configuration of the old endpoint with the binaries and configuration of the new endpoint. Enable the redirection of the temporary queue.
 1. Start the endpoint.
 1. The redirection of temporary queue can be removed when there are no timeout messages for the temporary queue (e.g. by running a query like `SELECT [Destination], [SagaId] FROM [NsbSamplesSagaMigration].[dbo].[NewTimeoutData] WHERE [Destination] = <address of temp queue>`.