---
title: Transactional session
summary: Atomicity when modifying data and sending messages in the context of a request
reviewed: 2022-08-12
component: TransactionalSession
versions: "[7,)"
redirects:
---

## The consistency problem

Consider a web controller that creates a `User` in the business database, and also publishes a `UserCreated` event. If a failure occurs during the execution of the request, two scenarios may occur, depending on the order of operations.

1. **Phantom record**: The message handler creates the `User` in the database first, then publishes the `UserCreated` event. If a failure occurs between these two operations:
    * The `User` is created in the database, but the `UserCreated` event is not published.
    * This results in a `User` in the database, known as a phantom record, which is never announced to the rest of the system.
2. **Ghost message**: The controller publishes the `UserCreated` event first, then creates the `User` in the database. If a failure occurs between these two operations:
    * The `UserCreated` event is published, but the `User` is not created in the database.
    * The rest of the system is notified about the creation of the `User`, but the `User` does not exist in the database. This causes further errors in the system which expect the `User` to exist in the database.

When in the context of a message handler, this problem can be mitigated through the Outbox feature, however, this problem remains unsolved outside of the context of a message handler.
The only way to address this problem on the client-side, is to defer all operations to the message handler. This entails sending a message to create the user and publishing the `UserCreated`-event from that message handler.
However, there are scenarios where this approach is not feasible:
- Existing applications that want to introduce messaging, already have quite some logic in controllers. Moving all that logic into dedicated message handlers requires a lot of effort, and might no be feasible from day one.
- There may be other scenarios in which it's not feasible to delay the database operation.

The `TransactionalSession` feature solves exactly this problem.

## Usage

To use the transactional session, first, the `NServiceBus.TransactionalSession`-package needs to be installed in the project.

Then, the feature needs to be enabled on the endpoint:

snippet: enabling-transactional-session

The transactional session can be resolved from the container, and needs to be opened:

snippet: opening-transactional-session

To send messages in an atomic manner, they can be sent through the `ITransactionalSession`:

snippet: sending-transactional-session

Once all the operations that are part of this request have been executed, the session should be committed:

snippet: committing-transactional-session

## Important design consideration

- dedup should be done by the user based on session
- doesn't work in send only endpoints
- requires a persistence that supports transactional session (where do we keep the list?)

## How it works

