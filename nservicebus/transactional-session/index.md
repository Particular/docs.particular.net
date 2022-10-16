---
title: Transactional session
summary: Atomicity when modifying data and sending messages outside the context of a message handler
reviewed: 2022-09-12
component: TransactionalSession
related:
- samples/transactional-session/aspnetcore-webapi
- samples/transactional-session/cosmosdb
---

## The consistency problem

Consider an ASP.NET Core controller that creates a user in the business database and publishes a `UserCreated` event. If a failure occurs during the execution of the request, two scenarios may occur, depending on the order of operations.

1. The controller creates the `User` in the database first, then publishes the `UserCreated` event. If a failure occurs between these two operations:
    * The user is created in the database, but the `UserCreated` event is not published.
    * This results in a user in the database which is never announced to the rest of the system.
2. The controller publishes the `UserCreated` event first, then creates the user in the database. If a failure occurs between these two operations:
    * The `UserCreated` event is published, but the user is not created in the database.
    * The rest of the system is notified about the creation of the user, but the user record is never created. This inconsistency causes errors, as parts of the system expect the record to exist in the database.

In the context of a message handler, the [Outbox](/nservicebus/outbox) feature can mitigate this problem. However, such scenarios remain unsolved outside the context of a message handler.

A common technique to address this problem on the client side is to defer all operations to a message handler. This entails sending a message to create the user and publishing the `UserCreated`-event from within a message handler.
However, there are scenarios where this approach is not feasible:

* Existing applications are likely to have non-trivial processing logic in controllers. Moving it all into dedicated message handlers requires significant effort.
* Processing logic in the controller may assume specific side effects to occur within the scope of the request, e.g., validation, notifications, or error handling. The logic may, therefore, not be ready for the asynchronous, fire-and-forget approach required when offloading work into message handlers.
* There may be other scenarios in which it's not feasible to delay the database operation.

The `TransactionalSession`, when combined with [Outbox](/nservicebus/outbox), solves this problem for messages sent and/or published outside the context of a message handler.

## Usage

To use the transactional session, first, install the [transactional session package for a supported persister](/nservicebus/transactional-session/persistences) in the project.

Then, enable the session integration on the endpoint as follows:

snippet: enabling-transactional-session

To ensure atomic consistency across database and message operations, enable the [Outbox](/nservicebus/outbox):

snippet: enabling-outbox

The transactional session can be resolved from the container, and needs to be opened:

snippet: opening-transactional-session

Sending messages in an atomic manner is done through the `ITransactionalSession`:

snippet: sending-transactional-session

The persistence-specific database session is accessible via the `transactionalSession.SynchronizedStorageSession` property or via dependency injection. See the [persistence-specific documentation](/nservicebus/transactional-session/persistences) for more details.

Once all the operations that are part of the atomic request have been executed, the session should be committed:

snippet: committing-transactional-session

Disposing the transactional session, without committing, will roll back any changes that were made.

### Advanced

#### Maximum commit duration

The maximum commit duration defaults to `TimeSpan.FromSeconds(15)`.

The maximum commit duration limits the amount of time it can take for a transaction to commit the changes. The value can be configured when opening the session.

snippet: configuring-timeout-transactional-session

The maximum commit duration does not represent the total transaction time, but rather the time it takes to complete the commit operation (as observed from the perspective of the control message). In practice, the observed total commit time might be longer due to delays in the transport caused by latency, delayed delivery, load on the input queue, endpoint concurrency limits, and more.

When the control message is consumed, but the outbox record is not yet available in storage, the following formula is applied to delay the message (see [Phase 2](#how-it-works-phase-2)):

```text
CommitDelayIncrement = 2 * CommitDelayIncrement;
RemainingCommitDuration = RemainingCommitDuration - 
   (CommitDelayIncrement > RemainingCommitDuration ? RemainingCommitDuration : CommitDelayIncrement)
```

The default commit delay increment is set to `Timespan.FromSeconds(2)`and cannot be overridden.

#### Metadata

It is possible to add custom headers to the control message that is used to settle the transaction outcome. Custom headers can be set by adding them to the metadata when opening the session. These headers can be accessed by a [custom behavior](/nservicebus/pipeline/manipulate-with-behaviors.md#add-a-new-step) in the `TransportReceive` part of the pipeline.

snippet: configuring-metadata-transactional-session

## Requirements

The transactional session feature requires a persistence to store outgoing messages. This feature is currently supported for the following persistence packages:

* [Azure Table](/persistence/azure-table)
* [CosmosDB](/persistence/cosmosdb)
* [SQL](/persistence/sql)
* [NHibernate](/persistence/nhibernate)
* [RavenDB](/persistence/ravendb)
* [MongoDB](/persistence/mongodb)

## Transaction consistency

To guarantee atomic consistency across database and message operations, the transactional session requires the [Outbox](/nservicebus/outbox) to be enabled. This combination of features provides the strongest consistency guarantees and is, therefore, the recommended, safe-by-default configuration.

NOTE: The outbox has to be [enabled explicitly](/nservicebus/outbox/#enabling-the-outbox) on the endpoint configuration.

With the Outbox disabled, database and message operations are not applied until the session is committed. All database operations share the same database transaction and are committed first. When the database operations complete successfully, the message operations are [batch-dispatched by the transport](/nservicebus/messaging/batched-dispatch.md). The message operations and the database changes are not guaranteed to be atomic. This might lead to inconsistencies in case of a failure during the commit phase.

## How it works

The transactional session feature guarantees that all outgoing message operations are eventually consistent with the data operations.

Returning to the earlier example of a message handler which creates a `User` and then publishes a `UserCreated` event, the following process occurs. Details are described following the diagram.

```mermaid
sequenceDiagram
    actor User
    autonumber
    Note over User: Phase 1
    activate User
    User->>TransactionalSession: Open()
    activate TransactionalSession
    TransactionalSession->>PendingTransportOperations: new()
    TransactionalSession->>Storage: BeginTransaction()
    activate Storage
    Storage-->>TransactionalSession: transaction
    deactivate Storage
    deactivate TransactionalSession

    User->>TransactionalSession: Publish<UserCreated>()
    activate TransactionalSession
    TransactionalSession->>PendingTransportOperations: Add()
    deactivate TransactionalSession
    activate TransactionalSession
    deactivate TransactionalSession
    User->>Storage: Store(user)

    User->>TransactionalSession: Commit()
    activate TransactionalSession
    TransactionalSession->>Transport: Dispatch(controlMessage)
    TransactionalSession->>PendingTransportOperations: ConvertToOutboxOperations
    activate PendingTransportOperations
    PendingTransportOperations-->>TransactionalSession: outboxRecord
    deactivate PendingTransportOperations
    TransactionalSession->>Storage: Store(outboxRecord)
    TransactionalSession->>Storage: Complete()
    deactivate TransactionalSession
    deactivate User

    Note over ReceivePipeline: Phase 2
    ReceivePipeline->>Transport: Read()
    activate ReceivePipeline
    ReceivePipeline->>Storage: GetOutboxRecord()
    activate Storage
    Storage-->>ReceivePipeline: outboxRecord
    deactivate Storage
    ReceivePipeline->>Transport: Dispatch(outboxRecord.Operations)
    ReceivePipeline->>Storage: SetAsDispatched()
    ReceivePipeline-->>Transport: Ack()
    deactivate ReceivePipeline
```

Internally, the transactional session doesn't use a single transaction that spans all the operations. The transactional session acknowledgement occurs in two separate phases:

### Phase 1

1. The user opens a transactional session.
2. A set of `PendingOperations` is initialized and collects the message operations.
3. A transaction is started on the storage seam.
4. The user can execute any required message operations using the transactional session.
5. The user can store any data using the persistence-specific session, which is accessible through the transactional session.
6. When all operations are registered, the user calls ´Commit´ on the transactional session.
7. A control message to complete the transaction is dispatched to the local queue. The control message is independent of the message operations and is not stored in the outbox record.
8. The message operations are converted and stored into an outbox record.
9. The transaction is committed, and the outbox record and business data modifications are stored atomically.

### Phase 2

The endpoint receives the control message and processes it as follows:

* Find the outbox record.
  * If it exists, it hasn't been marked as dispatched, and there are pending operations they are dispatched, and the outbox record is set as dispatched.
  * If it doesn't exist yet, delay the processing of the control message.

## Failure scenarios

The transactional session provides atomic store-and-send guarantees, similar to Outbox (except for incoming message de-duplication). That said, it cannot rely on the recoverability mechanism used by the Outbox, which uses [retries](/nservicebus/recoverability) to ensure outgoing messages are dispatched when failures occur. Instead, the control message is used to ensure that **exactly one** of these two outcomes occur:

* Transaction finishes with data being stored, and outgoing messages eventually sent - when the `Commit` path successfully stores the `OutboxRecord`
* Transaction finishes with no visible side effects - when the control message stores the `OutboxRecord`

Sending the control message first ensures that, eventually, the transaction will have an atomic outcome. If the `Commit` of the `OutboxRecord` succeeds, the control message will ensure the outgoing operations are sent out. If the `Commit` fails, the control message will (after the [maximum commit duration](#usage-advanced-maximum-commit-duration) elapses) eventually be consumed, leaving no side effects.

When dispatching the control message fails, the transactional session changes will be rolled back, and an error will be raised to the user committing the session.

## Limitations

* The transactional session cannot be used in send-only endpoints. A full endpoint is required to send a control message to the local queue.
* The transport must have the same or higher availability guarantees as the database.
