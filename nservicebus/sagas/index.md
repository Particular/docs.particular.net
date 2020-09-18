---
title: Sagas
summary: NServiceBus uses event-driven architecture to include fault-tolerance and scalability in long-term business processes.
component: Core
reviewed: 2019-09-16
redirects:
- nservicebus/sagas-in-nservicebus
related:
- samples/saga
---

NOTE: Want to learn how to build sagas step-by-step? Check out the [NServiceBus sagas: Getting started](/tutorials/nservicebus-sagas/1-getting-started/) tutorial.

Long-running business processes exist in many systems. Whether the steps are automated, manual, or a combination, effective handling of these processes is critical. NServiceBus employs event-driven architectural principles to bake fault-tolerance and scalability into these processes. The saga is a pattern that addresses the challenges uncovered by the relational database community years ago, packaged in NServiceBus for ease of use.

One of the common mistakes developers make when designing distributed systems is based on the assumptions that time is constant. If something runs quickly on their machine, they're liable to assume it will run with a similar performance characteristic in production. Network invocations (like web service calls) are misleading this way. When invoked on the developer's local machine, they perform well. In production, across firewalls and data centers, they don't perform nearly as well.

While a single web service invocation need not be considered "long-running", once there are two or more calls within a given use case, consistency issues should be taken into account. The first call may be successful but the second call can time out. Sagas allow coding for these cases in a simple and robust fashion.

Design processes with more than one remote call to use sagas.

While it may seem excessive at first, the business implications of the system getting out of sync with the other systems it interacts with can be substantial. It's not just about exceptions that end up in the logs.

## A simple saga

With NServiceBus, behavior is specified by writing a class that inherits from `Saga<T>` where `T` is the saga data class. There is also a base class for sagas that contains many features required for implementing long-running processes.

snippet: simple-saga

## Long-running means stateful

Any process that involves multiple network calls (or messages sent and received) has an interim state. That state may be kept in memory, persisted to disk, or stored in a distributed cache; it may be as simple as 'Response 1 received, pending response 2', but the state exists.

Using NServiceBus, it is possible to explicitly define the data used for this state by inheriting from the `ContainSagaData` abstract class. All public get/set properties are persisted by default:

snippet: simple-saga-data

## Adding behavior

The important part of a long-running process is its behavior. Just like regular message handlers, the behavior of a saga is implemented via the `IHandleMessages<M>` interface for the message types to be handled.

## Starting a saga

Since a saga manages the state of a long-running process, under which conditions should a new saga be created? Sagas are, in essence, a message driven state machine. The trigger to start this state machine is the arrival of one or more specified message types. In the previous example, a new saga is started every time a message of type `StartOrder` arrives. This is declared by adding `IAmStartedByMessages<StartOrder>` to the saga.

NOTE: `IHandleMessages<StartOrder>` is redundant since `IAmStartedByMessages<StartOrder>` already implies that.

This interface tells NServiceBus that the saga not only handles `StartOrder`, but that when that type of message arrives, a new instance of this saga should be created to handle it, if there isn't already an existing saga that correlates to the message. In essence the semantics of `IAmStartedByMessages` is:

> Create a new instance if an existing one can't be found

partial: at-least-one

### Dealing with out of order delivery

NOTE: Always assume that messages can be delivered out of order, e.g. due to error recovery, network latency, or concurrent message processing.

Sagas not designed to handle the arrival of messages out of order can result in some messages being discarded. In the previous example, this could happen if a `CompleteOrder` message is received before the `StartOrder` message has had a chance to create the saga.

To ensure messages are not discarded when they arrive out of order:

- Implement multiple `IAmStartedBy<T>` interfaces for any message type that assumes the saga instance should already exist
- Override the saga not found behavior and throw an exception using `IHandleSagaNotFound` and rely on NServiceBus recoverability capability to retry messages and resolve out of order issues.

#### Multiple message types starting a saga

If multiple message types can start a saga, it's necessary to ensure that saga behavior will remain correct for all possible combinations of order in which those messages are received.

In the previous example, the `StartOrder` message contains both an `OrderId` and a `CustomerId` but the `CompleteOrder` message contains only an `OrderId`. The handler for `CompleteOrder` requires `CustomerId` to complete the order saga (i.e. initiate a shipping process). If a `StartOrder` message is processed first, the saga persists both the `OrderId` and the `CustomerId` in the saga data and when a corresponding `CompleteOrder` message arrives later, it retrieves the `CustomerId` from the saga data.

When messages arrive in reverse order, the handler for the `CompleteOrder` message does not yet have access to the `CustomerId` because the related `StartOrder` message has not been processed yet. In this case, when the `StartOrder` message arrives later, the handler must notice that the `CompleteOrder` has already been processed and use its `CustomerId` to initiate the shipping process and complete the saga. This complicates the design of the saga, but makes it more resilient and allows it to handle messages out of order.

#### Relying on recoverability

In most scenarios, an acceptable solution to deal with out of order message delivery is to throw an exception when the saga instance does not exist. The message will be automatically retried, which may resolve the issue, or it will end up in the error queue, where it can be manually retried.

To override the default saga not found behavior [implement `IHandleSagaNotFound` and throw an exception](saga-not-found.md).

## Correlating messages to a saga

Correlation is needed in order to find existing saga instances based on data on the incoming message. See [Message Correlation](message-correlation.md) for more details.

Note: The saga `Id` available via `IContainSagaData` or `ContainSagaData` must not be used and is considered internal to NServiceBus. It is recommended to add an additional property that contains a unique saga instance identifier.

## Discarding messages when saga is not found

If a saga handles a message, but no related saga instance is found, then that message is discarded by default. Typically that happens when the saga has been already completed when the messages arrives and discarding the message is correct. If a different behavior is expected for specific scenarios, the default behavior [can be modified](saga-not-found.md).

## Ending a saga

When a saga instance is no longer needed it can be completed using the `MarkAsComplete()` API. This tells the saga infrastructure that the instance is no longer needed and can be cleaned up.

NOTE: Instance cleanup is implemented differently by the various saga persisters and is not guaranteed to be immediate.

### Outstanding timeouts

Outstanding timeouts requested by the saga instance will be discarded when they expire without triggering the [`IHandleSagaNotFound` API](saga-not-found.md) 

### Messages arriving after saga has been completed

Messages that [are allowed to start a new saga instance](#starting-a-saga) will cause a new instance with the same correlation id to be created.

Messages handled by the saga(`IHandleMessages<T>`), arriving after the saga has completed, will be passed to the [`IHandleSagaNotFound` API](saga-not-found.md).

### Consistency considerations

Completing a saga is a destructive operation so transaction support of the selected transport and persistence must be considered to ensure correctness. If the persistence is able to participate in the same transaction as the incoming receive operation, either using DTC or by sharing the transport's storage transaction (e.g. SQL Server transport), no further action is needed.

If the persistence can't participate in the same transaction as the incoming receive operation, then an additional action is needed to avoid saga completion causing incorrect behavior. The problematic scenario is when sagas are being completed together with sending/publishing outgoing messages. In case of failure after the saga is completed, the outgoing messages may not be dispatched. However, when the incoming message is retried the completed saga is not found, which results in outgoing messages being lost.

This issue can be avoided by:

 1. Enabling the [Outbox feature](/nservicebus/outbox/), if supported by the chosen persistence.
 1. Ensure that no outgoing messages will be dispatched by completing the saga from a timeout or sending an explicit command to self.
 1. Replace saga completion with soft delete by setting a flag/timestamp and use some native mechanism of the selected storage to cleanup old saga instances.

## Notifying callers of status

Messages can be published from a saga at any time. This is often used to notify the original caller that initiated the saga of some interim state that isn't relevant to other subscribers.

Using `Reply()` or `Return()` to communicate with the caller would only achieve the desired result in the case where the current message came from that client, and not in the case where any other partner sent a message arriving at that saga. For this reason, notice that the saga data contains the original client's return address. It also contains the message ID of the original request so that the client can correlate status messages on its end.

To communicate status in the previous example:

snippet: saga-with-reply

This is one of the methods on the saga base class that would be very difficult to implement without tying the saga code to low-level parts of the NServiceBus infrastructure.

## Configuring saga persistence

Make sure to configure appropriate [saga persistence](/persistence/).

snippet: saga-configure

## Sagas and automatic subscriptions

The auto subscription feature applies to sagas as well as the regular message handlers.

## Sagas and request/response

Sagas often play the role of coordinator, especially when used in integration scenarios. In essence this means that the saga decides what to do next and then asks someone else to do it. This allows sagas to remain free from interacting with non-transactional things like file systems and rest services. The most suitable communication pattern for this type of interaction is the request/response pattern since there is really only one party interested in the response and that is the saga itself.

A common scenario is a saga controlling the process of billing a customer through Visa or MasterCard. It is often the case that there are separate endpoints for making the web service/rest-calls to each payment provider and a saga coordinating retries and fallback rules. Each payment request would be a separate saga instance, so how would the instance hydrate and invoke when the response returns?

The usual way is to correlate on some kind of ID and let the user control how to find the correct saga instance using that ID. NServiceBus provides native support for these types of interactions. If a `Reply` is done in response to a message coming from a saga, NServiceBus will detect it and automatically set the correct headers so that it can correlate the reply back to the saga instance that issued the request. The exception to this rule is the request/response message exchange between two sagas. In such case the automatic correlation won't work and the reply message needs to be explicitly mapped using `ConfigureHowToFindSaga`.

## Accessing databases and other resources from a saga

WARNING: Other than interacting with its own internal state, a saga should **not** access a database, call out to web services, or access other resources - neither directly nor indirectly by having such dependencies injected into it.

The main reason for avoiding accessing data from external resources is possible contention and inconsistency of saga state. Depending on persister, the saga state is retrieved and persisted with either pessimistic or optimistic locking. If the transaction takes too long, it's possible another message will come in that correlates to the same saga instance. It might be processed on a different (concurrent) thread (or perhaps a scaled out endpoint) and it will either fail immediately (pessimistic locking) or while trying to persist the state (optimistic locking). In both cases the message will be retried.

This is the expected behavior and cannot be completely avoided, however, the longer the lock is open the more likely it is such situation will occur. In order to minimize chances of such problems use sagas only as a mechanism of business process orchestration.

If a saga needs additional data from external sources to process the message, then the best approach is to have the saga collect all the data it needs by handling messages that provide that data. This may result in sagas that "live forever", i.e. don't have a message that would cause them to `MarkAsComplete()`.

Since sagas, when they're not processing messages, are effectively a record in a database, leaving a saga running "forever" is equivalent to not deleting a record from a database. This is similar to how regular business entities in a system are not deleted.

This approach often results in sagas whose responsibility grows enough to be considered an [Aggregate Root in Domain-Driven Design](https://martinfowler.com/bliki/DDD_Aggregate.html).

When it comes to integration scenarios, like calling external systems, dedicated sagas should be designed to manage the interaction - yet those sagas shouldn't perform the external calls directly. Instead, those sagas should send messages to other endpoints which will perform those actions. It is common for those endpoints to send response messages back to the saga, as described in the previous section. These endpoints can be hosted in separate processes or in the same process as the saga depending on other architectural decisions.

## Querying saga data

Sagas manage state of potentially long-running business processes. It is possible to query the saga data directly but it is not recommended except for very simple administrative or support functionality. The reasons for this are:

 * The way a given persistence chooses to store the saga data is an implementation detail to the specific persistence that can potentially change over time. By directly querying for the saga data that query is being coupled to this implementation and risks being affected by format changes.
 * By exposing the data outside of the safeguards of the business logic in the saga the risk is that the data is not treated as read-only. Eventually, a component tries to bypass the saga and directly modify the data.
 * Querying the data might require additional indexes, resources etc. which need to be managed by the component issuing the query. Those additional resources can influence saga performance.
 * The saga data may not contain all the required data. A saga handling the order process may keep track of the "payment id" and the status of the payment, but it is not interested in keeping track of the amount paid. On the other hand, for querying it may be required to query the paid amount along with other data.

The recommended approach is for the saga to publish events containing the required data and have handlers that process these events and store the data in one or more read model(s) for querying purposes. This reduces coupling to the internals of the specific saga persistence, removes contention, and preserves the safeguards of the existing saga logic.

## Saga state read/write behavior

Saga state is read immediately before a message processing method is invoked, and written immediately after the method returns. The sequence of read, invoke, and write occurs once per message processing method.

- For persisters backed by SQL databases, the write will execute an `INSERT` statement if the read did not return any existing state, and the write will execute an `UPDATE` statement if the read _did_ return existing state. For other persisters, the equivalent operations will be executed.
- If the read did not return any existing state, and the saga is completed during the invoke, then no write will occur.
- The method of maintaining consistency with respect to concurrent message processing depends on the persister being used. Some may use a transactional lock, and others may perform an atomic change with optimistic concurrency control.
- Saga state reads and writes do not occur during a stage. They occur during invocation in the `Invoke Handlers` stage and cannot be intercepted.

If multiple saga types are invoked for the same message, each read, invoke, write cycle will occur sequentially, for each saga type.
