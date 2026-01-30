---
title: Sagas
summary: Master NServiceBus sagas to coordinate distributed workflows and ensure reliable long-running processes.
component: Core
reviewed: 2026-01-23
redirects:
- nservicebus/sagas-in-nservicebus
related:
- samples/saga
- nservicebus/messaging/header-manipulation
---

> [!NOTE]
> Want to learn how to build sagas step-by-step? Check out the [NServiceBus saga tutorials](/tutorials/nservicebus-sagas/).

Long-running business processes exist in many systems. Whether the steps are automated, manual, or a combination, effective handling of these processes is critical. NServiceBus employs event-driven architectural principles to bake fault-tolerance and scalability into these processes. The saga is a pattern that addresses the challenges uncovered by the relational database community years ago, packaged in NServiceBus for ease of use.

One common mistake developers make when designing distributed systems is assuming time is constant. If something runs quickly on their machine, it is easy to assume it will perform similarly in production. Network invocations (like web service calls) are misleading this way. When invoked on the developer's local machine, they perform well, but across firewalls and data centers in the real world of production, performance is slow by comparison.

While a single web service invocation need not be considered "long-running", once there are two or more calls within a given use case, consistency issues should be taken into account. The first call may succeed, but the second may time out. Sagas allow coding for these cases in a simple and robust fashion.

Design processes with multiple remote calls to use sagas.

While it may seem excessive at first, the business implications of the system getting out of sync with the other systems it interacts with can be substantial. It's not just about exceptions that end up in the logs.

## A simple saga

With NServiceBus, behavior is specified by writing a class that inherits from `Saga<T>` where `T` is the saga data class. There is also a base class for sagas that contains many features required for implementing long-running processes.

snippet: simple-saga

## Long-running means stateful

Any process that involves multiple network calls (or messages sent and received) has an interim state. That state may be kept in memory, persisted to disk, or stored in a distributed cache; it may be as simple as 'Response 1 received, pending response 2', but the state exists.

Using NServiceBus, it is possible to explicitly define the data used for this state by inheriting from the `ContainSagaData` abstract class. All public get/set properties are persisted by default:

snippet: simple-saga-data

### Avoid sharing types between sagas

Saga data types should not be shared across different sagas. Sharing types can result in persisters physically sharing the same storage structure, which should be avoided.

Sharing complex property types should also be avoided. Depending on the persister implementation, sharing property types can result in the storage structure being shared between endpoints.

NServiceBus will perform a check at startup to ensure that saga data types are not shared across sagas. An exception will be thrown at startup if any shared root types are found. Complex types in properties that are shared between sagas are not included in this check.

partial: disable-shared-state-check

> [!NOTE]
> If a saga property is a [record type](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record), that record type must be mutable so it can be deserialized.

## Adding behavior

The important part of a long-running process is its behavior. Just like regular message handlers, the behavior of a saga is implemented via the `IHandleMessages<M>` interface for the message types to be handled.

## Starting a saga

Since a saga manages the state of a long-running process, under what conditions should a new saga be created? Sagas are, in essence, a message-driven state machine. The trigger to start this state machine is the arrival of one or more specified message types. In the previous example, a new saga is started every time a message of type `StartOrder` arrives. This is declared by adding `IAmStartedByMessages<StartOrder>` to the saga.

> [!NOTE]
> `IHandleMessages<StartOrder>` is redundant since `IAmStartedByMessages<StartOrder>` already implies that.

This interface tells NServiceBus that the saga not only handles `StartOrder`, but also that when a message of that type arrives, a new instance of this saga should be created to handle it, if there isn't already an existing saga that correlates to the message. As a convenience, in NServiceBus version 6 and above, the message will set its mapped correlation property on the created saga data. In essence, the semantics of `IAmStartedByMessages` is:

> Create a new instance of the saga if an existing instance cannot be found

### Dealing with out-of-order delivery

Messages can be delivered out of order, e.g. due to error recovery, network latency, or concurrent message processing, and sagas must be designed to handle the arrival of out-of-order messages. Sagas not designed to handle the arrival of messages out of order can result in some messages being discarded. In the previous example, this could happen if a `CompleteOrder` message is received before the `StartOrder` message has had a chance to create the saga.

To ensure messages are not discarded when they arrive out of order:

- Implement multiple `IAmStartedByMessages<T>` interfaces for any message type that assumes the saga instance should already exist
- Override the saga not found behavior and throw an exception using a saga not found handler and rely on NServiceBus recoverability capability to retry messages to resolve out-of-order issues.

#### Multiple message types starting a saga

If multiple message types can start a saga, it's necessary to ensure that saga behavior will remain correct for all possible combinations of the order in which those messages are received.

In the previous example, the `StartOrder` message contains both an `OrderId` and a `CustomerId` but the `CompleteOrder` message contains only an `OrderId`. The handler for `CompleteOrder` requires `CustomerId` to complete the order saga (i.e. initiate a shipping process). If a `StartOrder` message is processed first, the saga persists both the `OrderId` and the `CustomerId` in the saga data and when a corresponding `CompleteOrder` message arrives later, it retrieves the `CustomerId` from the saga data.

When messages arrive in reverse order, the handler for the `CompleteOrder` message does not yet have access to the `CustomerId` because the related `StartOrder` message has not been processed yet. In this case, when the `StartOrder` message arrives later, the handler must notice that the `CompleteOrder` has already been processed and use its `CustomerId` to initiate the shipping process and complete the saga. This complicates the design of the saga, but makes it more resilient and allows it to handle messages out of order.

#### Relying on recoverability

In most scenarios, an acceptable solution to deal with out-of-order message delivery is to throw an exception when the saga instance does not exist. The message will be automatically retried, which may resolve the issue; otherwise, it will be placed in the error queue, where it can be manually retried.

To override the default saga not found behavior [implement a saga not found handler and throw an exception](saga-not-found.md).

## Correlating messages to a saga

Correlation is needed in order to find existing saga instances based on data in the incoming message. See [Message Correlation](message-correlation.md) for more details.

> [!NOTE]
> The saga `Id` available via `IContainSagaData` or `ContainSagaData` is considered internal to NServiceBus and must not be used to correlate messages. Instead, add an additional property that contains a unique saga instance identifier.

## Discarding messages when saga is not found

If a saga handles a message but no related saga instance is found, the message is discarded by default. Typically, this happens when the saga has already been completed by the time a message arrives and discarding the message is correct. If a different behavior is expected for specific scenarios, the default behavior [can be modified](saga-not-found.md). 

## Ending a saga

When a saga instance is no longer needed, it can be completed using the `MarkAsComplete()` API. This tells the saga infrastructure that the instance is no longer needed and can be cleaned up.

Instance cleanup is implemented differently by the various saga persisters and is not guaranteed to be immediate.

### Outstanding timeouts

Outstanding timeouts requested by the saga instance will be discarded when they expire without triggering the [saga not found handler](saga-not-found.md).

### Messages arriving after a saga has been completed

Messages that [are allowed to start a new saga instance](#starting-a-saga) will cause a new instance with the same `CorrelationId` to be created.

Messages handled by the saga (`IHandleMessages<T>`) that arrive after the saga has completed will be passed to the [saga not found handler](saga-not-found.md).

### Consistency considerations

Completing a saga is a destructive operation, so the transactional capabilities of the chosen transport and persistence must be considered to ensure correctness. If the persistence can participate in the same transaction as the message receive operation, either via DTC or by sharing the transport’s storage transaction (e.g. SQL Server transport), no additional action is required. If it cannot, extra measures are necessary to prevent incorrect system behavior when a saga completes.

A risk arises when a saga is completed while sending/publishing outgoing messages. If a failure occurs after the saga is completed but before the outgoing messages are dispatched, those messages may be lost. If an incoming message is retried, the saga no longer exists, so the outgoing messages are not sent again and are lost.

This issue can be avoided by:

 1. Enabling the [Outbox feature](/nservicebus/outbox/) if it supported by the chosen persistence.
 1. Ensuring that no outgoing messages will be dispatched by completing the saga from a timeout or sending an explicit command to self.
 1. Replacing saga completion with a soft delete, then setting a flag/timestamp to clean up old saga instances afterward

## Notifying callers of status

Messages can be published from a saga at any time. This is often used to notify the original caller that initiated the saga of some interim state that isn't relevant to other subscribers. The saga data contains the original client's return address and the message ID of the original request so that the caller can correlate status messages on its end.Using `Reply()` or `Return()`. to communicate with the caller would only achieve the desired result when the current message came from that caller, not when another caller sent a message to that saga. For this reason, notice that

To communicate status in the previous example:

snippet: saga-with-reply

This is one of the methods on the saga base class that would be very difficult to implement without tying the saga code to low-level parts of the NServiceBus infrastructure.

## Saga persistence

Make sure to configure appropriate [saga persistence](/persistence/).

snippet: saga-configure

The choice of persistence can affect the design of saga data, such as the length of the saga class name, the use of virtual properties of the saga, etc. While the NServiceBus persister aims to abstract away these details, sometimes the limitations of the specific implementation can have an impact.

## Sagas and automatic subscriptions

The auto subscription feature applies to sagas as well as the regular message handlers.

## Sagas and request/response

Sagas often play the role of coordinator, especially when used in integration scenarios. In essence this means that the saga decides what to do next and then asks someone else to do it. This allows sagas to remain free from interacting with non-transactional things like file systems and rest services. The most suitable communication pattern for this type of interaction is the request/response pattern since there is really only one party interested in the response and that is the saga itself.

A common scenario is a saga controlling the process of billing a customer through Visa or MasterCard. It is often the case that there are separate endpoints for making the web service/rest-calls to each payment provider and a saga coordinating retries and fallback rules. Each payment request would be a separate saga instance, so how would the instance hydrate and invoke when the response returns?

The usual way is to correlate on some kind of ID and let the user control how to find the correct saga instance using that ID. NServiceBus provides native support for these types of interactions. If a `Reply` is done in response to a message coming from a saga, NServiceBus will detect it and automatically set the correct headers so that it can correlate the reply back to the saga instance that issued the request. This is called [auto-correlation](message-correlation.md#auto-correlation). The exception to this rule is the request/response message exchange between two sagas. In such case the automatic correlation won't work and the reply message needs to be explicitly mapped using `ConfigureHowToFindSaga`.

## Avoid External Resource Access

> [!WARNING]
> A saga should only interact with its own internal state and send or publish messages. It must not perform any I/O operations, including calls to databases, web services, or other external resources, either directly or indirectly through injected dependencies.

Accessing external resources from within a saga creates significant contention and consistency challenges. Saga state is retrieved and persisted using either pessimistic or optimistic locking, depending on the persister implementation. Extended transaction durations increase the likelihood of concurrent message processing issues.

When a transaction remains open for an extended period, another message correlating to the same saga instance may arrive. This message might be processed on a different thread or by a scaled-out endpoint instance, resulting in:

- **Pessimistic locking**: Immediate lock contention failure and message retry
- **Optimistic locking**: Concurrency conflict during state persistence and message retry

While this retry behavior is expected and cannot be entirely prevented, the duration of an open lock directly correlates to collision probability. To minimize these issues, use sagas exclusively as business process orchestration mechanisms.

### Event-Driven Data Collection

When a saga requires additional data from external sources to process messages, the recommended approach is to design the saga to collect that data through message handlers. Rather than querying external systems, the saga receives messages containing the necessary information.

This pattern often results in sagas that persist indefinitely without calling `MarkAsComplete()`. Since inactive sagas exist as database records, an indefinitely-running saga is functionally equivalent to maintaining any other business entity record in your system. This approach frequently evolves sagas into what Domain-Driven Design identifies as [Aggregate Roots](https://martinfowler.com/bliki/DDD_Aggregate.html).

### Managing External System Integration

For integration scenarios requiring external system calls, design dedicated sagas to orchestrate these interactions without performing the actual I/O operations. Instead, these sagas should:

1. Send (command) messages to dedicated message handlers.
2. Have those handlers execute the - sometimes very long-running - external operations and [reply](/nservicebus/messaging/reply-to-a-message.md) the result back to the saga for processing.
3. Process these results, not requiring IO, as all data is already contained in the response message.

This separation maintains the saga's role as a coordinator while delegating execution to appropriate handlers.

These handlers can be hosted in the same or separate endpoints.

Hosting these in separate endpoints allows for better tweaking of the concurrency limit and allowed retries that are better suited if these handlers access external resources that allow no 

## Querying saga data

Sagas manage the state of potentially long-running business processes. It is possible to query the saga data directly, but it is not recommended except for very simple administrative or support functionality. The reasons for this are:

 * The way a given persistence chooses to store the saga data is an implementation detail of the specific persistence that can potentially change over time. By directly querying for the saga data, that query is being coupled to this implementation and risks being affected by format changes.
 * By exposing the data outside of the safeguards of the business logic in the saga, the risk is that the data is not treated as read-only. Eventually, a component tries to bypass the saga and directly modify the data.
 * Querying the data might require additional indexes, resources, etc., which need to be managed by the component issuing the query. Those additional resources can influence saga performance.
 * The saga data may not contain all the required data. A saga handling the order process may keep track of the "payment id" and the payment status, but it is not interested in the amount paid. On the other hand, for querying, it may be required to query the paid amount along with other data.

The recommended approach is for the saga to publish events containing the required data and have handlers that process these events and store the data in one or more read models for querying purposes. This reduces coupling to the internals of the specific saga persistence, removes contention, and preserves the safeguards of the existing saga logic.

## Saga state read/write behavior

Saga state is read immediately before a message processing method is invoked, and written immediately after the method returns. The sequence of read, invoke, and write occurs once per message processing method.

- For persisters backed by SQL databases, the write will execute an `INSERT` statement if the read did not return any existing state, and the write will execute an `UPDATE` statement if the read _did_ return existing state. For other persisters, the equivalent operations will be executed.
- If the read did not return any existing state, and the saga is completed during the invoke, then no write will occur.
- The method of maintaining consistency with respect to concurrent message processing depends on the persister being used. Some may use a transactional lock, and others may perform an atomic change with optimistic concurrency control.
- Saga state reads and writes do not occur during a stage. They occur during invocation in the `Invoke Handlers` stage and cannot be intercepted.

If multiple saga types are invoked for the same message, each read, invoke, write cycle will occur sequentially, for each saga type.

partial: manual-registration
