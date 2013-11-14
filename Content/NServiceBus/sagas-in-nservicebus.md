<!--
title: "Sagas in NServiceBus"
tags: ""
summary: "<p>Long-running business processes exist in many systems. Whether the steps are automated, manual, or a combination, effective handling of these processes is critical. NServiceBus employs event-driven architectural principles to bake fault-tolerance and scalability into these processes.The saga is a pattern that addresses the challenges uncovered by the relational database community years ago, packaged in NServiceBus for ease of use.</p>
<p>One of the common mistakes developers make when designing distributed systems is based on the assumptions that time is constant. If something runs quickly on their machine, they're liable to assume it will run with a similar performance characteristic when in production. Network invocations (like web service calls) are misleading this way. When invoked on the developer's local machine, they perform well. In production, across firewalls and datacenters, they don't perform nearly as well.</p>
"
-->

Long-running business processes exist in many systems. Whether the steps are automated, manual, or a combination, effective handling of these processes is critical. NServiceBus employs event-driven architectural principles to bake fault-tolerance and scalability into these processes.The saga is a pattern that addresses the challenges uncovered by the relational database community years ago, packaged in NServiceBus for ease of use.

One of the common mistakes developers make when designing distributed systems is based on the assumptions that time is constant. If something runs quickly on their machine, they're liable to assume it will run with a similar performance characteristic when in production. Network invocations (like web service calls) are misleading this way. When invoked on the developer's local machine, they perform well. In production, across firewalls and datacenters, they don't perform nearly as well.

While a single web service invocation need not be considered
"long-running", once there are two or more calls within a given use case, you should take issues of consistency into account. The first call may be successful but the second call can time out. Sagas allow coding for these cases in a simple and robust fashion.

Design processes with more than one remote call use sagas.

While it may seem excessive at first, the business implications of your system getting out of sync with the other systems it interacts with can be substantial. It's not just about exceptions that end up in your log files.

Long-running means stateful
---------------------------

Any process that involves multiple network calls (or messages sent and received) has an interim state. That state may be kept in memory, persisted to disk, or stored in a distributed cache; it may be as simple as 'Response 1 received, pending response 2', but the state exists.

Using NServiceBus, you can explicitly define the data used for this state by implementing the interface IContainSagaData. All public get/set properties are persisted by default:

<script src="https://gist.github.com/Particular/6059775.js?file=MySagaData.cs"></script> By default, NServiceBus stores your sagas in RavenDB. The schema-less nature of document databases makes them a perfect fit for saga storage where each saga instance is persisted as a single document. There is also support for relational databases using
[NHibernate](http://nhforge.org/) . NHibernate support is located in the NServiceBus.NHibernate assembly. You can, as always, swap out these technologies, by implementing the IPersistSagas interface.

Adding behavior
---------------

The important part of a long-running process is its behavior. With NServiceBus, you specify behavior by writing a class that implements ISaga<t> where T is the saga data. There is also a base class for sagas that contains many features required for implementing long-running processes. All the examples below make use of this base class.

Just like regular message handlers, the behavior of a saga is implemented via the IHandleMessages<m> interface for the message types to be handled. Here is a saga that processes messages of type Message1 and Message2:

<script src="https://gist.github.com/Particular/6059775.js?file=MySaga.cs"></script> Starting and correlating sagas
------------------------------

Since a saga manages the state of a long-running process, under which conditions should a new saga be created? Sometimes it's simply the arrival of a given message type. In our previous example, let's say that a new saga should be started every time a message of type Message1 arrives, like this:

<script src="https://gist.github.com/Particular/6059775.js?file=MySaga2.cs"></script>
**NOTE** : IHandleMessages<message1> is replaced with IAmStartedByMessages<message1>. This interface tells NServiceBus that the saga not only handles Message1, but that when that type of message arrives, a new instance of this saga should be created to handle it.

How to correlate a Message2 message with the right saga that's already running? Usually, there's some applicative ID in both types of messages that can correlate between them. You only need to store this in the saga data, and tell NServiceBus about the connection. Here's how:

<script src="https://gist.github.com/Particular/6059775.js?file=MySaga3.cs"></script> Underneath the covers, when Message2 arrives, NServiceBus asks the saga persistence infrastructure to find an object of the type MySagaData that has a property SomeID whose value is the same as the SomeID property of the message.

Uniqueness
----------

For NServiceBus to ensure uniqueness across your saga instances, it's highly recommended that you adorn your correlation properties with the
[Unique] attribute. This tells NServiceBus that there can be only one instance for each property value. This also increases performance for property lockups in most cases. While there are plans for it, NServiceBus doesn't currently support mapping one message to more than one instance of the same saga.

Read more about the [Unique property and concurrency](nservicebus-sagas-and-concurrency.md) .

Notifying callers of status
---------------------------

While you always have the option of publishing a message at any time in a saga, sometimes you want to notify the original caller who caused the saga to be started of some interm state that isn't relevant to other subscribers.

If you tried to use Bus.Reply() or Bus.Return() to communicate with the caller, that would only achieve the desired result in the case where the current message came from that client, and not in the case where any other partner sent a message arriving at that saga. For this reason, you can see that the saga data contains the original client's return address. It also contains the message ID of the original request so that the client can correlate status messages on its end.

To communicate status in our ongoing example:

<script src="https://gist.github.com/Particular/6059775.js?file=MySaga4.cs"></script> This is one of the methods on the saga base class that would be very difficult to implement yourself without tying your applicative saga code to low-level parts of the NServiceBus infrastructure.

Timeouts
--------

When working in a message-driven environment, you cannot make assumptions about when the next message will arrive. While the connectionless nature of messaging prevents our system from bleeding expensive resources while waiting, there is usually an upper limit on how long—from a business perspective—to wait. At that point, some business-specific action should be taken, as shown:

<script src="https://gist.github.com/Particular/6059775.js?file=MySaga5.cs"></script> The RequestTimeout<t> method on the base class tells NServiceBus to send a message to what is called a Timeout Manager(TM) which durably keeps time for us. In NServiceBus, each endpoint hosts a TM by default, so there is no configuration needed to get this up and running.

When the time is up, the Timeout Manager sends a message back to the saga causing its Timeout method to be called with the same state message originally passed.

**IMPORTANT** : Don't assume that other messages haven't arrived in the meantime.

Ending a long-running process
-----------------------------

After receiving all the messages needed in a long-running process, or possibly after a timeout (or two, or more) you will want to clean up the state that was stored for the saga. This is done simply by calling the MarkAsComplete() method. The infrastructure contacts the Timeout Manager
(if an entry for it exists) telling it that timeouts for the given saga ID can be cleared. If any messages relating to that saga arrive after it has completed, they are discarded. If you want a copy of these messages to be maintained, that can be handled by the [generic audit functionality in NServiceBus](auditing-with-nservicebus.md) .

Complex saga finding logic
--------------------------

Sometimes a saga handles certain message types without a single simple property that can be mapped to a specific saga instance. In those cases, you'll want finer-grained control of how to find a saga instance, as follows:

<script src="https://gist.github.com/Particular/6059775.js?file=MySagaFinder.cs"></script> You can have as many of these classes as you want for a given saga or message type. If a saga can't be found, return null, and if the saga specifies that it is to be started for that message type, NServiceBus will know that a new saga instance is to be created.

Sagas in self-hosted endpoints
------------------------------

When [hosting NServiceBus in your own endpoint](hosting-nservicebus-in-your-own-process.md) , make sure to include .Sagas().RavenSagaPersister(), as follows:

<script src="https://gist.github.com/Particular/6059775.js?file=EndpointConfig.cs.cs"></script> Sagas and automatic subscriptions
---------------------------------

In NServiceBus V3.0 and onwards the autosubscription feature applies to sagas as well as your regular message handlers. This is a change compared to earlier versions of NServiceBus.

Sagas and request/response
--------------------------

Sagas often play the role of coordinator, especially when used in integration scenarios. In essence this means that the saga decides what to do next and then asks someone else to do it. This allows you to keep your sagas free from interacting with non-transactional things like file systems and rest services. The type of communication pattern best suited best for these type of interactions is the request/response pattern since there is really only one party interested in the response and that is the saga itself.

A typical scenario is a saga controlling the process of billing a customer through Visa or Mastercard. In this case you probably have separate endpoints for making the webservice/rest-calls to each payment provider and a saga coordinating retries and fallback rules. Each payment request would be a separate saga instance, so how would we know which instance to hydrate and invoke when the response returns?

The usual way is to correlate on some kind of ID and let the user tell you how to find the correct saga instance using that ID. While this is easily done we decided that this was common enough to warrant native support in NServiceBus for these type of interactions. In V3.0, NServiceBus handles all this for you without getting in your way. If you do IBus.Reply in response to a message coming from a saga, NServiceBus will detect it and automatically set the correct headers so that you can correlate the reply back to the saga instance that issued the request. You can see all this in action in the [Manufacturing sample.](https://github.com/NServiceBus/NServiceBus/tree/master/Samples/Manufacturing)

