---
title: Sagas in NServiceBus
summary: NServiceBus uses event-driven architecture to include fault-tolerance and scalability in long-term business processes.
tags:
- Sagas
---

Long-running business processes exist in many systems. Whether the steps are automated, manual, or a combination, effective handling of these processes is critical. NServiceBus employs event-driven architectural principles to bake fault-tolerance and scalability into these processes.The saga is a pattern that addresses the challenges uncovered by the relational database community years ago, packaged in NServiceBus for ease of use.

One of the common mistakes developers make when designing distributed systems is based on the assumptions that time is constant. If something runs quickly on their machine, they're liable to assume it will run with a similar performance characteristic when in production. Network invocations (like web service calls) are misleading this way. When invoked on the developer's local machine, they perform well. In production, across firewalls and datacenters, they don't perform nearly as well.

While a single web service invocation need not be considered "long-running", once there are two or more calls within a given use case, you should take issues of consistency into account. The first call may be successful but the second call can time out. Sagas allow coding for these cases in a simple and robust fashion.

Design processes with more than one remote call to use sagas.

While it may seem excessive at first, the business implications of your system getting out of sync with the other systems it interacts with can be substantial. It's not just about exceptions that end up in your log files.

## Long-running means stateful

Any process that involves multiple network calls (or messages sent and received) has an interim state. That state may be kept in memory, persisted to disk, or stored in a distributed cache; it may be as simple as 'Response 1 received, pending response 2', but the state exists.

Using NServiceBus, you can explicitly define the data used for this state by inheriting from the `ContainSagaData` abstract class. All public get/set properties are persisted by default:

<!-- import saga-data -->

In previous versions you are forced to use `IContainSagaData` interface directly (`ContainSagaData` implement `IContainSagaData`):

<!-- import saga-data-v4 -->

There are two production-supported storage mechanisms for saga data in NServiceBus, namely RavenDB and NHibernate. Prior to V5, RavenDB was a default implementation. Since NServiceBus 5, both implementations are equal and the user needs to explicitly chose one.

Both implementations have their strong points. While the schema-less nature of document databases makes them a perfect fit for saga storage where each saga instance is persisted as a single document, NHibernate allows using almost any relational database engine existing. 

You can, as always, swap out these technologies, by implementing the `ISagaPersister` interface (`IPersistSagas` prior to V5).

## Adding behavior

The important part of a long-running process is its behavior. With NServiceBus, you specify behavior by writing a class that inherits from `Saga<T>` where `T` is the saga data class. There is also a base class for sagas that contains many features required for implementing long-running processes. All the examples below make use of this base class.

Just like regular message handlers, the behavior of a saga is implemented via the `IHandleMessages<M>` interface for the message types to be handled. Here is a saga that processes messages of type `Message2`:

<!-- import saga-without-started-by -->

Please note that since NServiceBus 5, the `ConfigureHowToFindSaga` method is abstract. This way the new API tries to guide the user towards best practices. Prior to V5, an empty saga would look like this:

<!-- import saga-without-started-by-v4 -->

Even though the method is virtual, you *should* implement it for each saga.

## Starting and correlating sagas

Since a saga manages the state of a long-running process, under which conditions should a new saga be created? Sometimes it's simply the arrival of a given message type. In our previous example, let's say that a new saga should be started every time a message of type `Message1` arrives, like this:

<!-- import saga-without-mapping -->

Please note that `IHandleMessages<Message1>` is replaced with `IAmStartedByMessages<Message1>`. This interface tells NServiceBus that the saga not only handles Message1, but that when that type of message arrives, a new instance of this saga should be created to handle it.

How to correlate a `Message2` message with the right saga that's already running? Usually, there's some applicative ID in both types of messages that can correlate between them. You only need to store this in the saga data, and tell NServiceBus about the connection. Here's how:

<!-- import saga-with-started-by-and-correlation-id-set -->

Since V5 it is possible to specify the mapping to the message using expressions if the correlation information is split between multiple fields

<!-- import saga-find-by-expression -->

Previous releases (3.x and 4.x) of NServiceBus had slightly different API for configuring the mapping. Following snippets show the mapping in V4 and V3 respecively:

<!-- import saga-with-started-by-and-correlation-id-set-v4 -->

<!-- import saga-with-started-by-and-correlation-id-set-v3 -->

Underneath the covers, when `Message2` arrives, NServiceBus asks the saga persistence infrastructure to find an object of the type `MySagaData` that has a property `SomeID` whose value is the same as the `SomeID` property of the message.

## Uniqueness

For NServiceBus to ensure uniqueness across your saga instances, it's highly recommended that you adorn your correlation properties with the
`[Unique]` attribute. This tells NServiceBus that there can be only one instance for each property value. This also increases performance for property lockups in most cases. While there are plans for it, NServiceBus doesn't currently support mapping one message to more than one instance of the same saga.

Read more about the [Unique property and concurrency](nservicebus-sagas-and-concurrency.md) .

## Notifying callers of status

While you always have the option of publishing a message at any time in a saga, sometimes you want to notify the original caller who caused the saga to be started of some interm state that isn't relevant to other subscribers.

If you tried to use `Bus.Reply()` or `Bus.Return()` to communicate with the caller, that would only achieve the desired result in the case where the current message came from that client, and not in the case where any other partner sent a message arriving at that saga. For this reason, you can see that the saga data contains the original client's return address. It also contains the message ID of the original request so that the client can correlate status messages on its end.

To communicate status in our ongoing example:

<!-- import saga-with-reply -->

This is one of the methods on the saga base class that would be very difficult to implement yourself without tying your applicative saga code to low-level parts of the NServiceBus infrastructure.

## Timeouts

When working in a message-driven environment, you cannot make assumptions about when the next message will arrive. While the connectionless nature of messaging prevents our system from bleeding expensive resources while waiting, there is usually an upper limit on how long from a business perspective to wait. At that point, some business-specific action should be taken, as shown:

<!-- import saga-with-timeout -->

The `RequestTimeout<T>` method on the base class tells NServiceBus to send a message to what is called a Timeout Manager(TM) which durably keeps time for us. In NServiceBus, each endpoint hosts a TM by default, so there is no configuration needed to get this up and running.

When the time is up, the Timeout Manager sends a message back to the saga causing its Timeout method to be called with the same state message originally passed.

**IMPORTANT** : Don't assume that other messages haven't arrived in the meantime. Plase note how the timeout handler method first checks if the `Message2` message has arrived.

## Ending a long-running process

After receiving all the messages needed in a long-running process, or possibly after a timeout (or two, or more) you will want to clean up the state that was stored for the saga. This is done simply by calling the `MarkAsComplete()` method. 

<!-- import saga-with-complete -->

The infrastructure contacts the Timeout Manager (if an entry for it exists) telling it that timeouts for the given saga ID can be cleared. If any messages that are handled by the saga(`IHandleMessages<T>`) arrive after the saga has completed, they are discarded. Note that a new saga will be started if a message that is configured to start a saga arrives(`IAmStartedByMessages<T>`).

If compensating actions need to be taken for messages that are handled by the saga which arrive after the saga has been marked as complete, then this can be done by implementing the `ISagaNotFound` interface.

<!-- import saga-not-found -->

Note that the message will be considered successfully processed and sent to the audit queue even if no saga was found. If you want the message to end up in the error queue just throw an exception from your `IHandleSagaNotFound` implementation.

## Complex saga finding logic

Sometimes a saga handles certain message types without a single simple property that can be mapped to a specific saga instance. In those cases, you'll want finer-grained control of how to find a saga instance, as follows:

<!-- import saga-finder -->

You can have as many of these classes as you want for a given saga or message type. If a saga can't be found, return null, and if the saga specifies that it is to be started for that message type, NServiceBus will know that a new saga instance is to be created. The above example uses NServiceBus extension for NHibernate that allows both framework and user code to share the same NHibernate session. Similar extension point exists for RavenDB.

## Sagas in self-hosted endpoints

When [hosting NServiceBus in your own endpoint](hosting-nservicebus-in-your-own-process.md), make sure to configure appropriate persistence mechanism. Below you can see how it is done via the V5 streamlined configuration API:

<!-- saga-configure-self-hosted -->

And via the old API:

<!-- saga-configure-self-hosted-v4 -->

## Sagas and automatic subscriptions

In NServiceBus V3.0 and onwards the autosubscription feature applies to sagas as well as your regular message handlers. This is a change compared to earlier versions of NServiceBus.

## Sagas and request/response

Sagas often play the role of coordinator, especially when used in integration scenarios. In essence this means that the saga decides what to do next and then asks someone else to do it. This allows you to keep your sagas free from interacting with non-transactional things like file systems and rest services. The type of communication pattern best suited best for these type of interactions is the request/response pattern since there is really only one party interested in the response and that is the saga itself.

A typical scenario is a saga controlling the process of billing a customer through Visa or Mastercard. In this case you probably have separate endpoints for making the webservice/rest-calls to each payment provider and a saga coordinating retries and fallback rules. Each payment request would be a separate saga instance, so how would we know which instance to hydrate and invoke when the response returns?

The usual way is to correlate on some kind of ID and let the user tell you how to find the correct saga instance using that ID. While this is easily done we decided that this was common enough to warrant native support in NServiceBus for these type of interactions. In V3.0, NServiceBus handles all this for you without getting in your way. If you do IBus.Reply in response to a message coming from a saga, NServiceBus will detect it and automatically set the correct headers so that you can correlate the reply back to the saga instance that issued the request. You can see all this in action in the [Video Store sample.](https://github.com/Particular/NServiceBus.Msmq.Samples/tree/master/VideoStore.Msmq)

## Learn more

- [Sagas and concurrency](nservicebus-sagas-and-concurrency)
- [Using Sagas in ServiceMatrix](/ServiceMatrix/getting-started-sagasfullduplex-2.0)
- [Saga View in ServiceInsight](/ServiceInsight/getting-started-overview#the-saga-view)
