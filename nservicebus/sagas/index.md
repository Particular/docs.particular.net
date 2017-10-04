---
title: Sagas
summary: NServiceBus uses event-driven architecture to include fault-tolerance and scalability in long-term business processes.
component: Core
reviewed: 2016-08-30
tags:
- Saga
redirects:
- nservicebus/sagas-in-nservicebus
related:
- samples/saga
---

Long-running business processes exist in many systems. Whether the steps are automated, manual, or a combination, effective handling of these processes is critical. NServiceBus employs event-driven architectural principles to bake fault-tolerance and scalability into these processes. The saga is a pattern that addresses the challenges uncovered by the relational database community years ago, packaged in NServiceBus for ease of use.

One of the common mistakes developers make when designing distributed systems is based on the assumptions that time is constant. If something runs quickly on their machine, they're liable to assume it will run with a similar performance characteristic when in production. Network invocations (like web service calls) are misleading this way. When invoked on the developer's local machine, they perform well. In production, across firewalls and data centers, they don't perform nearly as well.

While a single web service invocation need not be considered "long-running", once there are two or more calls within a given use case, take issues of consistency into account. The first call may be successful but the second call can time out. Sagas allow coding for these cases in a simple and robust fashion.

Design processes with more than one remote call to use sagas.

While it may seem excessive at first, the business implications of the system getting out of sync with the other systems it interacts with can be substantial. It's not just about exceptions that end up in the logs.


## No saga instance available

If a message that doesn't start a saga is received but no related saga instance is found, then by default that message is discarded, i.e. it is possible that a saga instance had been already completed when further messages arrive and such messages should be discarded.

NOTE: Always assume that messages can be delivered out of order, i.e. due to invoking the error recovery mechanism, network latency, and/or concurrent message processing.

A similar message discarding behavior can also be invoked in case of saga instance creation. When a message is received that expects a saga instance to be there but that message that would create the saga instance has not yet been received or completed processing.

There are two options to avoid discarding incoming messages in this scenario:

- Let the saga instance be created for the message processing that previously assumed the saga instance would already exist using `IAmStartedBy<T>`
- Override the saga not found behavior and throw an exception using `IHandleSagaNotFound`

By letting the saga instance also be created for a message that previously relied on another message that would have create the saga instance we now have to make sure that the behavior of the saga will remain correct. Lets assume that we relied on message A to be processed before B but we now also have the saga instance created when B is received then maybe state from message A needs to be included that is added to the saga instance when A is received. This state is not yet stored in the saga if message B is processed before message A thus the message cannot be send yet. We now need to check that when message A is received if message B is already processed and then have the message send. This can make the saga much more complex!

A simpler solution is to throw an error when the saga instance does not exist so that it eventually gets moved to the error queue after all immediate and delayed retries attempts are done. Override saga not found behavior by [implementing `IHandleSagaNotFound` and throw an exception](saga-not-found.md). 


## A simple Saga

A minimal Saga implementation. With NServiceBus, behavior is specified by writing a class that inherits from `Saga<T>` where `T` is the saga data class. There is also a base class for sagas that contains many features required for implementing long-running processes.

snippet: simple-saga


## Long-running means stateful

Any process that involves multiple network calls (or messages sent and received) has an interim state. That state may be kept in memory, persisted to disk, or stored in a distributed cache; it may be as simple as 'Response 1 received, pending response 2', but the state exists.

Using NServiceBus, it is possible to explicitly define the data used for this state by inheriting from the `ContainSagaData` abstract class. All public get/set properties are persisted by default:

snippet: simple-saga-data


## Adding behavior

The important part of a long-running process is its behavior. Just like regular message handlers, the behavior of a saga is implemented via the `IHandleMessages<M>` interface for the message types to be handled.


## Starting a saga

Since a saga manages the state of a long-running process, under which conditions should a new saga be created? Sagas are, in essence, a message driven state machine. The trigger to start this state machine is the arrival of one or more specified message types. In the previous example, assume a new saga should be started every time a message of type `StartOrder` arrives. This would be declared by adding `IAmStartedByMessages<StartOrder>` to the saga.

NOTE: `IHandleMessages<StartOrder>` is redundant since `IAmStartedByMessages<StartOrder>` already implies that.

This interface tells NServiceBus that the saga not only handles `StartOrder`, but that when that type of message arrives, a new instance of this saga should be created to handle it, if there isn't already an existing saga that correlates to the message. In essence the semantics of `IAmStartedByMessages` is:

> Create a new instance if an existing one can't be found

partial: at-least-one


## Correlating messages to a saga

Correlation is needed in order to find existing saga instances based on data on the incoming message. See [Message Correlation](message-correlation.md) for more details.


## Ending a long-running process

After receiving all the messages needed in a long-running process, or possibly after a timeout (or two, or more), the stored state of the saga needs to be cleaned up. This is done by calling the `MarkAsComplete()` method.

The infrastructure contacts the Timeout Manager (if an entry for it exists) telling it that timeouts for the given saga ID can be cleared. If any messages that are handled by the saga(`IHandleMessages<T>`) arrive after the saga has completed, they are discarded. Note that a new saga will be started if a message that is configured to start a saga arrives(`IAmStartedByMessages<T>`).

For more information about setting (requesting) the timeouts and handling them, see [Saga Timeouts](timeouts.md).

When a message is received that could possibly be handled by a saga, and no existing saga can be found then that is handed by the [Saga Not Found](saga-not-found.md) feature.


### Transactional behavior

Completing a saga is a destructive operation so transaction support of the selected transport and persistence must be considered to ensure correctness. If the persistence is able to participate in the same transaction as the incoming receive operation, either using DTC or by sharing the transports storage transaction (SQLServer transport), no further action is needed.

If the persistence can't participate in the same transaction as the incoming receive operation, then an additional action is needed to avoid saga completion causing incorrect behavior. The problematic scenario is when sagas are being completed together with sending/publishing outgoing messages. In case of failure after the saga is completed, the outgoing messages may not be dispatched. However, when the incoming message is retried the completed saga is not found, which results in outgoing messages being lost.

This issue can be avoided by:

 1. Enabling the [Outbox feature](/nservicebus/outbox/), if supported by the chosen persistence.
 1. Ensure that no outgoing messages will be dispatched by completing the saga from a timeout or sending an explicit command to self.
 1. Replace saga completion with soft delete by setting a flag/timestamp and use some native mechanism of the selected storage to cleanup old saga instances.


## Notifying callers of status

Messages can be publishing from a saga at any time. This is often used to notify the original caller, that caused the saga to be started, of some interim state that isn't relevant to other subscribers.

Attempting to use `Reply()` or `Return()` to communicate with the caller, that would only achieve the desired result in the case where the current message came from that client, and not in the case where any other partner sent a message arriving at that saga. For this reason, notice that the saga data contains the original client's return address. It also contains the message ID of the original request so that the client can correlate status messages on its end.

To communicate status in the example:

snippet: saga-with-reply

This is one of the methods on the saga base class that would be very difficult to implement without tying the saga code to low-level parts of the NServiceBus infrastructure.


## Configuring Saga persistence

Make sure to configure appropriate [saga persistence](/persistence/).

snippet: saga-configure


## Sagas and automatic subscriptions

The auto subscription feature applies to sagas as well as the regular message handlers. This is a change compared to earlier versions of NServiceBus.


## Sagas and request/response

Sagas often play the role of coordinator, especially when used in integration scenarios. In essence this means that the saga decides what to do next and then asks someone else to do it. This allows sagas to remain free from interacting with non-transactional things like file systems and rest services. The most suitable communication pattern for this type of interaction is the request/response pattern since there is really only one party interested in the response and that is the saga itself.

A typical scenario is a saga controlling the process of billing a customer through Visa or MasterCard. It is often the case that there are separate endpoints for making the web service/rest-calls to each payment provider and a saga coordinating retries and fallback rules. Each payment request would be a separate saga instance, so how would the instance hydrate and invoke when the response returns?

The usual way is to correlate on some kind of ID and let the user control how to find the correct saga instance using that ID. While this is easily done it was decided that this was common enough to warrant native support in NServiceBus for these type of interactions. From Version 3.0, NServiceBus handles this. If a `Reply` is done in response to a message coming from a saga, NServiceBus will detect it and automatically set the correct headers so that it can correlate the reply back to the saga instance that issued the request. The exception to this rule is the request/response message exchange between two sagas. In such case the automatic correlation won't work and the reply message needs to be explicitly mapped using `ConfigureHowToFindSaga`.


## Accessing databases and other resources from a saga

WARNING: Other than interacting with its own internal state, a saga should **not** access a database, call out to web services, or access other resources - neither directly nor indirectly by having such dependencies injected into it.

Instead, if a saga needs data as a part of its message processing logic, the best way to address that is to have the saga be started by an earlier message and then collect all the data it needs by handling all the messages over time that provide that data. This may result in sagas that "live forever", i.e. don't have a message that would cause them to `MarkAsComplete()`.

Since sagas, when they're not processing messages, are effectively a record in a database, leaving a saga running "forever" is equivalent to not deleting a record from a database. This is similar to how regular business entities in a system are not deleted.

This approach often results in sagas whose responsibility grows enough to be considered an [Aggregate Root in Domain-Driven Design](https://martinfowler.com/bliki/DDD_Aggregate.html).

When it comes to integration scenarios, like calling external systems, dedicated sagas should be designed to manage the interaction - yet those sagas shouldn't perform the external calls directly. Instead, those sagas should send messages to other endpoints which will perform those actions. It is common for those endpoints to send response messages back to the saga, as described in the previous section. These endpoints can be hosted in separate processes or in the same process as the saga depending on other architectural decisions.


## Querying Saga Data

Sagas manage state of potentially long-running business processes. To access the current state of a business process the urge exists to query the saga data directly. It can be done, but is recommend against it. While this can be appropriate for very simple administrative or support functionality, it is not recommend as a general-purpose approach for these reasons:

 * The way a given persistence chooses to store the saga data is an implementation detail to the specific persistence that can potentially change over time. By directly querying for the saga data that query is being coupled to this implementation and risks being affected by format changes.
 * By exposing the data outside of the safeguards of the business logic in the saga the risk is that the data is not treated as read-only. Eventually, a component tries to bypass the saga and directly modify the data.
 * Querying the data might require additional indexes, resources etc. which need to be managed by the component issuing the query. Those additional resources can influence saga performance.
 * The saga data may not contain all the required data. A saga handling the order process may keep track of the "payment id" and the status of the payment, but it is not interested in keeping track of the amount paid. On the other hand, for querying it may be required to query the paid amount along with other data.

The recommended approach is for the saga to publish events, containing the required data, and have handlers that process these events and store the data in one or more read model(s) for querying purposes. It reduces coupling to the internals of the specific saga persistence, removes contention and doesn't bypass the safeguard of the existing saga logic.
