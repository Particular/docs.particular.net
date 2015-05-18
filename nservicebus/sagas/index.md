---
title: Sagas
summary: NServiceBus uses event-driven architecture to include fault-tolerance and scalability in long-term business processes.
tags:
- Sagas
redirects:
- nservicebus/sagas-in-nservicebus
related:
- samples/saga
- nservicebus/sagas/timeouts
- nservicebus/sagas/saga-finding
- nservicebus/sagas/saga-not-found
---

Long-running business processes exist in many systems. Whether the steps are automated, manual, or a combination, effective handling of these processes is critical. NServiceBus employs event-driven architectural principles to bake fault-tolerance and scalability into these processes.The saga is a pattern that addresses the challenges uncovered by the relational database community years ago, packaged in NServiceBus for ease of use.

One of the common mistakes developers make when designing distributed systems is based on the assumptions that time is constant. If something runs quickly on their machine, they're liable to assume it will run with a similar performance characteristic when in production. Network invocations (like web service calls) are misleading this way. When invoked on the developer's local machine, they perform well. In production, across firewalls and datacenters, they don't perform nearly as well.

While a single web service invocation need not be considered "long-running", once there are two or more calls within a given use case, you should take issues of consistency into account. The first call may be successful but the second call can time out. Sagas allow coding for these cases in a simple and robust fashion.

Design processes with more than one remote call to use sagas.

While it may seem excessive at first, the business implications of your system getting out of sync with the other systems it interacts with can be substantial. It's not just about exceptions that end up in your log files.

## A simple Saga

A minimal Saga implementation. With NServiceBus, you specify behavior by writing a class that inherits from `Saga<T>` where `T` is the saga data class. There is also a base class for sagas that contains many features required for implementing long-running processes. 

<!-- import simple-saga --> 

## Long-running means stateful  

Any process that involves multiple network calls (or messages sent and received) has an interim state. That state may be kept in memory, persisted to disk, or stored in a distributed cache; it may be as simple as 'Response 1 received, pending response 2', but the state exists.

Using NServiceBus, you can explicitly define the data used for this state by inheriting from the `ContainSagaData` abstract class. All public get/set properties are persisted by default:

<!-- import simple-saga-data --> 

There are two production-supported storage mechanisms for saga data in NServiceBus, namely RavenDB and NHibernate. Prior to V5, RavenDB was a default implementation. Since NServiceBus 5, both implementations are equal and the user needs to explicitly chose one.

Both implementations have their strong points. While the schema-less nature of document databases makes them a perfect fit for saga storage where each saga instance is persisted as a single document, NHibernate allows using almost any relational database engine existing. 

You can, as always, swap out these technologies, by implementing the `ISagaPersister` interface (`IPersistSagas` prior to V5).

## Adding behavior

The important part of a long-running process is its behavior. Just like regular message handlers, the behavior of a saga is implemented via the `IHandleMessages<M>` interface for the message types to be handled. 

## Starting and correlating sagas

Since a saga manages the state of a long-running process, under which conditions should a new saga be created? Sometimes it's simply the arrival of a given message type. In our previous example, let's say that a new saga should be started every time a message of type `StartOrder` message arrives `IAmStartedByMessages<StartOrder>`

Please note that `IHandleMessages<StartOrder>` is redundant since `IAmStartedByMessages<StartOrder>` already implies that. This interface tells NServiceBus that the saga not only handles `StartOrder`, but that when that type of message arrives, a new instance of this saga should be created to handle it.

How to correlate a `CompleteOrder` message with the right saga that's already running? Usually, there's some applicative ID in both types of messages that can correlate between them. You only need to store this in the saga data, and tell NServiceBus about the connection. This is done in the `ConfigureHowToFindSaga` in the above saga.

Since V5 it is possible to specify the mapping to the message using expressions if the correlation information is split between multiple fields

<!-- import saga-find-by-expression -->

Underneath the covers, when `CompleteOrder` arrives, NServiceBus asks the saga persistence infrastructure to find an object of the type `OrderSagaData` that has a property `OrderId` whose value is the same as the `OrderId` property of the message.

## Uniqueness

For NServiceBus to ensure uniqueness across your saga instances, it's highly recommended that you adorn your correlation properties with the `[Unique]` attribute. This tells NServiceBus that there can be only one instance for each property value. This also increases performance for property lockups in most cases. While there are plans for it, NServiceBus doesn't currently support mapping one message to more than one instance of the same saga.

Read more about the [Unique property and concurrency](concurrency.md).

## Ending a long-running process

After receiving all the messages needed in a long-running process, or possibly after a timeout (or two, or more) you will want to clean up the state that was stored for the saga. This is done simply by calling the `MarkAsComplete()` method.

The infrastructure contacts the Timeout Manager (if an entry for it exists) telling it that timeouts for the given saga ID can be cleared. If any messages that are handled by the saga(`IHandleMessages<T>`) arrive after the saga has completed, they are discarded. Note that a new saga will be started if a message that is configured to start a saga arrives(`IAmStartedByMessages<T>`).

For more information about setting (requesting) the timeouts and handling them, see [Saga Timeouts](http://docs.particular.net/nservicebus/sagas/timeouts).

When a message is received that could possibly be handled by a saga, and no existing saga can be found then that is handed by the [Saga Not Found](saga-not-found.md) feature. 

## Notifying callers of status

While you always have the option of publishing a message at any time in a saga, sometimes you want to notify the original caller who caused the saga to be started of some interim state that isn't relevant to other subscribers.

If you tried to use `Bus.Reply()` or `Bus.Return()` to communicate with the caller, that would only achieve the desired result in the case where the current message came from that client, and not in the case where any other partner sent a message arriving at that saga. For this reason, you can see that the saga data contains the original client's return address. It also contains the message ID of the original request so that the client can correlate status messages on its end.

To communicate status in our ongoing example:

<!-- import saga-with-reply -->

This is one of the methods on the saga base class that would be very difficult to implement yourself without tying your applicative saga code to low-level parts of the NServiceBus infrastructure.

## Configuring Saga persistence

Make sure to configure appropriate persistence mechanism. 

<!-- import saga-configure -->

## Sagas and automatic subscriptions

The autosubscription feature applies to sagas as well as your regular message handlers. This is a change compared to earlier versions of NServiceBus.

## Sagas and request/response

Sagas often play the role of coordinator, especially when used in integration scenarios. In essence this means that the saga decides what to do next and then asks someone else to do it. This allows you to keep your sagas free from interacting with non-transactional things like file systems and rest services. The type of communication pattern best suited best for these type of interactions is the request/response pattern since there is really only one party interested in the response and that is the saga itself.

A typical scenario is a saga controlling the process of billing a customer through Visa or MasterCard. In this case you probably have separate endpoints for making the webservice/rest-calls to each payment provider and a saga coordinating retries and fallback rules. Each payment request would be a separate saga instance, so how would we know which instance to hydrate and invoke when the response returns?

The usual way is to correlate on some kind of ID and let the user tell you how to find the correct saga instance using that ID. While this is easily done we decided that this was common enough to warrant native support in NServiceBus for these type of interactions. In V3.0, NServiceBus handles all this for you without getting in your way. If you do `IBus.Reply` in response to a message coming from a saga, NServiceBus will detect it and automatically set the correct headers so that you can correlate the reply back to the saga instance that issued the request. The exception to this rule is the request/response message exchange between two sagas. In such case the automatic correlation won't work and the reply message needs to be explicitly mapped using `ConfigureHowToFindSaga`.
