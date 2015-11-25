---
title: Sagas
summary: NServiceBus uses event-driven architecture to include fault-tolerance and scalability in long-term business processes.
tags:
- Sagas
redirects:
- nservicebus/sagas-in-nservicebus
related:
- samples/saga
---

Long-running business processes exist in many systems. Whether the steps are automated, manual, or a combination, effective handling of these processes is critical. NServiceBus employs event-driven architectural principles to bake fault-tolerance and scalability into these processes.The saga is a pattern that addresses the challenges uncovered by the relational database community years ago, packaged in NServiceBus for ease of use.

One of the common mistakes developers make when designing distributed systems is based on the assumptions that time is constant. If something runs quickly on their machine, they're liable to assume it will run with a similar performance characteristic when in production. Network invocations (like web service calls) are misleading this way. When invoked on the developer's local machine, they perform well. In production, across firewalls and data centers, they don't perform nearly as well.

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

There are two production-supported storage mechanisms for saga data in NServiceBus, namely RavenDB and NHibernate. Prior to version 5, RavenDB was a default implementation. Since NServiceBus 5, both implementations are equal and the user needs to explicitly chose one.

Both implementations have their strong points. While the schema-less nature of document databases makes them a perfect fit for saga storage where each saga instance is persisted as a single document, NHibernate allows using almost any relational database engine existing. 

You can, as always, swap out these technologies, by implementing the `ISagaPersister` interface (`IPersistSagas` prior to version 5).


## Adding behavior

The important part of a long-running process is its behavior. Just like regular message handlers, the behavior of a saga is implemented via the `IHandleMessages<M>` interface for the message types to be handled. 


## Starting a saga

Since a saga manages the state of a long-running process, under which conditions should a new saga be created? Sagas are, in essence, a message driven state machine. The trigger to started this state machine is the arrival of one or more specified message types. In our previous example, let's say that a new saga should be started every time a message of type `StartOrder` message arrives you would declare that by adding `IAmStartedByMessages<StartOrder>` to your saga. 

NOTE: `IHandleMessages<StartOrder>` is redundant since `IAmStartedByMessages<StartOrder>` already implies that. 

This interface tells NServiceBus that the saga not only handles `StartOrder`, but that when that type of message arrives, a new instance of this saga should be created to handle it if there isn't already an existing saga that correlates to the message. In essence the semantics of `IAmStartedByMessages` is:

> Create a new instance if a existing one can't be found

NOTE: As of Version 6 NServiceBus will require each saga to have at least one message that is able to start it.


## Correlating messages to a saga

Correlation is needed in order to find existing saga instances based on data on the incoming message. In our example how do we correlate a `CompleteOrder` message with the right saga that's already running? Usually, there's some applicative ID in both types of messages that can correlate between them. To declare this you need to use the `ConfigureHowToFindSaga` method and use the `Mapper` to specify to which saga property each message maps to. Note that NServiceBus will only allow you to correlate on a single saga property. Should you need to correlate on more than one property you need to use a custom saga finder mentioned below.

{{NOTE:
In Version 6 and higher NServiceBus will enforce that all correlated properties have a non default value when the saga instance is persisted.

In Version 6 and higher NServiceBus will not allow you to change the value of correlated properties for existing instances.

Since version 5 it is possible to specify the mapping to the message using expressions if the correlation information is split between multiple fields.

Version 5 and below allowed you to correlate on more than one saga property.
}}

snippet:saga-find-by-expression

Underneath the covers, when `CompleteOrder` arrives, NServiceBus asks the saga persistence infrastructure to find an object of the type `OrderSagaData` that has a property `OrderId` whose value is the same as the `OrderId` property of the message. If found the saga instance will be loaded a the `Handle` method for the `CompleteOrder` message will be invoked. Should the saga instance not be found and the message, like in this case, not be allowed to start a saga the [saga not found](/nservicebus/sagas/saga-not-found.md) handlers will be invoked.


### Auto correlation

A common usage of sagas is to have them send out a request message to get some work done and receive a response message back when the work is complete. To make this easier NServiceBus will auto correlate those response messages back to the correct saga instance without any need for mappings.

NOTE: A caveat of this feature is that it currently doesn't support auto correlation between sagas. So if the request is handled by a another saga you must add relevant message properties and map them to the requesting saga using the syntax described above.


### Custom saga finder

Should you need full control over how a message is correlated to a saga you can create a custom [saga finder](/nservicebus/sagas/saga-finding.md).


## Uniqueness

NServiceBus will make sure that all properties used for correlation is unique across all instances of the given saga type. How this is enforced is up to each persister but will most likely translate to a unique key constraint in the database.

Mapping a single message to multiple saga instances is not supported. Should you need this you can simulate this by using a message handler that looks up all saga instance affected and send a separate message targeting each of those instances using the regular correlation described above.

NOTE: Versions prior to version 6 required you to put a `[Unique]` attribute on the saga properties used for correlation to enforce uniqueness

Read more about the [concurrency](concurrency.md).


## Ending a long-running process

After receiving all the messages needed in a long-running process, or possibly after a timeout (or two, or more) you will want to clean up the state that was stored for the saga. This is done by calling the `MarkAsComplete()` method.

The infrastructure contacts the Timeout Manager (if an entry for it exists) telling it that timeouts for the given saga ID can be cleared. If any messages that are handled by the saga(`IHandleMessages<T>`) arrive after the saga has completed, they are discarded. Note that a new saga will be started if a message that is configured to start a saga arrives(`IAmStartedByMessages<T>`).

For more information about setting (requesting) the timeouts and handling them, see [Saga Timeouts](timeouts.md).

When a message is received that could possibly be handled by a saga, and no existing saga can be found then that is handed by the [Saga Not Found](saga-not-found.md) feature. 


## Notifying callers of status

While you always have the option of publishing a message at any time in a saga, sometimes you want to notify the original caller who caused the saga to be started of some interim state that isn't relevant to other subscribers.

If you tried to use `Bus.Reply()` or `Bus.Return()` to communicate with the caller, that would only achieve the desired result in the case where the current message came from that client, and not in the case where any other partner sent a message arriving at that saga. For this reason, you can see that the saga data contains the original client's return address. It also contains the message ID of the original request so that the client can correlate status messages on its end.

To communicate status in our ongoing example:

snippet:saga-with-reply

This is one of the methods on the saga base class that would be very difficult to implement yourself without tying your saga code to low-level parts of the NServiceBus infrastructure.


## Configuring Saga persistence

Make sure to configure appropriate [saga persistence](/nservicebus/persistence/). 

snippet:saga-configure


## Sagas and automatic subscriptions

The auto subscription feature applies to sagas as well as your regular message handlers. This is a change compared to earlier versions of NServiceBus.


## Sagas and request/response

Sagas often play the role of coordinator, especially when used in integration scenarios. In essence this means that the saga decides what to do next and then asks someone else to do it. This allows you to keep your sagas free from interacting with non-transactional things like file systems and rest services. The type of communication pattern best suited best for these type of interactions is the request/response pattern since there is really only one party interested in the response and that is the saga itself.

A typical scenario is a saga controlling the process of billing a customer through Visa or MasterCard. In this case you probably have separate endpoints for making the web service/rest-calls to each payment provider and a saga coordinating retries and fallback rules. Each payment request would be a separate saga instance, so how would we know which instance to hydrate and invoke when the response returns?

The usual way is to correlate on some kind of ID and let the user tell you how to find the correct saga instance using that ID. While this is easily done we decided that this was common enough to warrant native support in NServiceBus for these type of interactions. In version 3.0, NServiceBus handles all this for you without getting in your way. If you do `IBus.Reply` in response to a message coming from a saga, NServiceBus will detect it and automatically set the correct headers so that you can correlate the reply back to the saga instance that issued the request. The exception to this rule is the request/response message exchange between two sagas. In such case the automatic correlation won't work and the reply message needs to be explicitly mapped using `ConfigureHowToFindSaga`.


## Querying Saga Data

Sagas manage state of potentially long-running business processes. When we want to access the current state of a business process we may feel the urge to query the saga data directly. It can be done, but we recommend against it. While this can be appropriate for very simple administrative or support functionality, we don't recommend it as a general-purpose approach for these reasons:

 * The way a given persistence chooses to store the saga data is an implementation detail to the specific persistence that can potentially change over time. By directly querying for the saga data you are coupling that query to this implementation and risk being affected by format changes.
 * By exposing the data outside of the safeguards of the business logic in the saga you risk the data is not treated as read-only. Eventually, a component tries to bypass the saga and directly modify the data. 
 * Querying the data might require additional indexes, resources etc. which need to be managed by the component issuing the query. Those additional resources can influence saga performance.
 * The saga data may not contain all the required data. A saga handling the order process may keep track of the "payment id" and the status of the payment, but it is not interested in keeping track of the amount paid. On the other hand, for querying we may want to query the paid amount along with other data.

The recommended approach is for the saga to publish events, containing the required data, and have handlers that process these events and store the data in one or more read model(s) for querying purposes. It reduces coupling to the internals of the specific saga persistence, removes contention and doesn't bypass the safeguard of the existing saga logic.
