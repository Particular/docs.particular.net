---
title: Transactional session
summary: Atomicity when modifying data and sending messages outside the context of a mesage handler
reviewed: 2022-09-12
component: TransactionalSession
related:
- samples/transactional-session/aspnetcore-webapi
- samples/transactional-session/cosmosdb
---

## The consistency problem

Consider an ASP.NET Core controller that creates a user in the business database and publishes a `UserCreated` event. If a failure occurs during the execution of the request, two scenarios may occur, depending on the order of operations.

1. **Phantom record**: The controller creates the `User` in the database first, then publishes the `UserCreated` event. If a failure occurs between these two operations:
    * The user is created in the database, but the `UserCreated` event is not published.
    * This results is a user in the database, known as a phantom record, which is never announced to the rest of the system.
2. **Ghost message**: The controller publishes the `UserCreated` event first, then creates the user in the database. If a failure occurs between these two operations:
    * The `UserCreated` event is published, but the user is not created in the database.
    * The rest of the system is notified about the creation of the user, but the record does not exist in the database. This causes further errors in the system which expects the user to exist in the database.

In the context of a message handler, the [Outbox](/nservicebus/outbox) feature can mitigate this problem, however, such scenarios remain unsolved outside of the context of a message handler.

A common technique to address this problem on the client side is to defer all operations to a message handler. This entails sending a message to create the user and publishing the `UserCreated`-event from within a message handler.
However, there are scenarios where this approach is not feasible:
- Existing applications that want to introduce messaging already have quite some logic in controllers. Moving all that logic into dedicated message handlers requires a lot of effort, and might no be feasible from day one.
- Existing logic in the controller might assume certain side-effects to occur within the scope of the request (for example validation, notifications or error-handling) and not yet ready to fully embrace the asynchronous and fire&forget nature of offloading the work into message handlers.
- There may be other scenarios in which it's not feasible to delay the database operation.

The `TransactionalSession` feature solves this problem for messages sent/published outside of the context of a message handler when combined with the [Outbox](/nservicebus/outbox) feature.

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

The persistence specific database session is accessible via the `transactionalSession.SynchronizedStorageSession` property or via dependency injection. See the [persistence specific documentation](/nservicebus/transactional-session/persistences) for more details.

Once all the operations that are part of the atomic request have been executed, the session should be committed:

snippet: committing-transactional-session

Disposing the transactional session without committing, will roll-back any changes that were made.

### Advanced

#### Maximum commit duration

The maximum commit duration is by default set to `TimeSpan.FromSeconds(15)`.

The maximum commit duration represents the maximum duration the transaction is allowed to attempt to atomically commit. The value can be configured when opening the session.

snippet: configuring-timeout-transactional-session

The maximum commit duration is not actual total transaction time from the perspective of the clock but the actual time observed from the perspective of the control message that is used to settle the transaction outcome. The actual total transaction time observed might be longer, taking into account delays in the transport due to latency, delayed delivery, load on the input queue, endpoint concurrency limits and more.

When the control message arrives and the outbox record is not yet visible the following formula applies to delay the message (see [Phase 2](#phase-2)):

```
CommitDelayIncrement = 2 * CommitDelayIncrement;
RemainingCommitDuration = RemainingCommitDuration - 
   (CommitDelayIncrement > RemainingCommitDuration ? RemainingCommitDuration : CommitDelayIncrement)
```

The default commit delay increment is set to `Timespan.FromSeconds(2)`and cannot be overriden.

#### Metadata

It is possible to add custom headers to the control message that is used to settle the transaction outcome. Custom headers can be set by adding them to the metadata when opening the session.

snippet: configuring-metadata-transactional-session

## Requirements

The transactional session feature requires a persistence in order to store outgoing messages. This feature is currently supported for the following persistence packages:

* [Azure Table](/persistence/azure-table)
* [CosmosDB](/persistence/cosmosdb)
* [SQL](/persistence/sql)
* [NHibernate](/persistence/nhibernate)
* [RavenDB](/persistence/ravendb)
* [MongoDB](/persistence/mongodb)

## Transaction consistency

To guarantee atomic concistency across database and message operations, the transactional session requires the [Outbox](/nservicebus/outbox) to be enabled. Using the transactional session together with the outbox provides the strongest concistency guarantees and is therefore the recommended safe-by-default configuration.

With the Outbox disabled, database and message operations are not executed until the session is committed. All database operations share the same database transaction but message operations are not guaranteed to be atomic with the database changes. This might lead to phantom records or ghost messages when in case of a failure during the commit phase.

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

Internally the transactional session doesn't use a single transaction that spans all the operations. The transactional session ackknowledgement occurs in two seperate phases:

### Phase 1

1. The user opens a transactional session.
2. A set of `PendingOperations` is initialized and collects the message operations.
3. A transaction is started on the storage seam.
4. The user can execute any required message operations using the transactional session.
5. The user can store any data using the persistence-specific session, which is accessible through the transactional session.
6. When all operations are registered, the user calls ´Commit´ on the transactional session.
7. A control message is dispatched to the local queue.
8. The message operations are converted and stored into an outbox record.
9. The transaction is committed, and the outbox record and business data modifications are stored atomically.

### Phase 2

The endpoint receives the control message and processes it as follows:
   * Find the outbox record.
   * If it exists, it hasn't been marked as dispatched, and there are pending operations, dispatch those operations, and set the outbox record as dispatched.
   * If it doesn't exist, processing of the control message is delayed to a later point.

## Important design consideration

* The transactional session uses a control message that is sent to the local queue. Due to this design, this feature requires a full endpoint and cannot be used in send-only endpoints.
* Deduplication is guaranteed in Phase 2, but not in Phase 1. In Phase 2, the outbox record ensures that the operations will never be dispatched more than once. However, during Phase 1, a unique ID is assigned to the session. At that point, the user is responsible for ensuring that no duplicate requests are executed.
* The transport must have the same or higher availability guarantees as the database.