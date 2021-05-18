---
title: Architectural principles
summary: NServiceBus helps write code that is robust in production environments, preventing data loss under failure conditions
reviewed: 2021-06-08
redirects:
 - nservicebus/architectural-principles
---

Messaging can be used to ensure autonomy and loose coupling in systems, both at design time and at run time. However, in order to benefit from those qualities, applications must be carefully designed and good practices followed.

Service-oriented architecture (SOA) and event-driven architecture provide the basis for identifying where to use messaging frameworks, such as NServiceBus. Strategic domain-driven design helps bridge the gap between business and IT and is an essential strategy for identifying service boundaries and finding meaningful business events.

## How NServiceBus aligns with SOA

<iframe allowfullscreen frameborder="0" height="300" mozallowfullscreen src="https://player.vimeo.com/video/113515335" webkitallowfullscreen width="400"></iframe>

In this presentation Udi Dahan explains the process of finding the right service boundaries. The presentation starts with introduction to SOA, explains challenges with traditional layered architectures and covers an approach that cuts across all application layers, outlining the inherent lines of loose and tight coupling. Finally, Udi shows how these vertical services collaborate using events, enabling flexible and high performance business processes.

## Drilling down into details

A common problem in many systems is that they are fragile. One part of the system slows down, affecting other parts of the system, ultimately crashing the entire system.

A primary design goal of NServiceBus is to eliminate that flaw by guiding developers to write code that is robust in production environments. That robustness prevents data loss under failure conditions.

To make effective use of NServiceBus, it is necessary to understand the distributed systems architecture it is designed to support. Those basic principles are explained briefly in this article. For more in-depth coverage, see the [Advanced Distributed Systems Design course](https://particular.net/adsd).

The basic communication pattern that enables robustness is one-way messaging, also known as "fire and forget". Since the amount of time it can take to communicate with another machine across the network is unknown and unbounded, asynchronous communication in NServiceBus is based on a store-and-forward model.

## Messaging versus RPC

NServiceBus enforces queued messaging, which has profound architectural implications. The principles and patterns underlying queued messaging are decades old and battle-tested through countless technological shifts.

It's very simple to build an application and get it working using traditional remote procedure call (RPC) techniques that, for example, WCF supports. However, scalability and fault-tolerance are inherently hindered when using blocking calls. Scaling up and throwing more hardware at the problem has little effect.

NServiceBus avoids these problems from the beginning. There's no such thing as a blocking call with one-way messaging. Common, transient errors can be resolved automatically with retries, and it's easy to recover from failures that require some manual intervention. Above all, even when a part of the system fails, no data gets lost.

To learn more about the relationship between messaging and reliable, scalable, and highly-available systems, watch the webinar about [handling failures with NServiceBus](https://particular.net/webinars/handling-failures-with-nservicebus) and other [webinar and presentations](https://particular.net/videos).

### Store-and-forward messaging

![Store and Forward Messaging](store-and-forward.png)

In this model, when the client process calls an API to send a message to the server process, the API returns control to the calling thread before the message is sent. At that point the transfer of the message across the network becomes the responsibility of the messaging technology. There may be communications interference. For example, the server machine may be down, or a firewall may slow the transfer. Also, even though the message may have reached the target machine, the target process may currently be down.

The client process is oblivious to those problems; as soon as the message is sent, messaging infrastructure takes over. As a result, critical resources like threads are not held waiting for the message processing to complete. This prevents the client process from losing stability while waiting for a response from another machine or process.

### Request/response and one-way messaging

The common pattern of request/response, which is more accurately described as synchronous RPC, is handled differently when using one-way messaging. From a network perspective, request/response is just two one-way interactions:

![Full duplex Request-Response messaging](full-duplex-messaging.png)

This communication pattern is particularly important for servers, as clients behind problematic network connections now have little effect on a server's stability. If a client crashes after sending the request, but before the server sends a response, the server will not have resources tied up waiting until the connection times out.

When used in combination with durable messaging, system-wide robustness increases even more.

Durable messaging differs from regular store-and-forward messaging in that messages are persisted to disk locally before attempts are made to send them. This means that, if the process crashes before the message is sent, the message is not lost. In server-to-server scenarios, where a server can complete a local transaction but might crash one second later, one-way durable messaging makes it easier to create a system which is robust overall, even though it is built using unreliable building blocks.

A different communication style involves one-to-many communication.

### Publish/subscribe

In this pattern, the sender of a message isn't aware of the subscriber's details.

#### Subscriptions

The additional loose coupling comes at the cost of subscribers explicitly opting-in to receiving messages:

![Subscription process](/nservicebus/messaging/publish-subscribe/subscribe.png)

Subscribers need to know which endpoint is responsible for publishing a given message type. This information is usually available as part of the contract, specifying to which endpoint a subscriber should send its subscription request. Part of the subscription request message is a subscriber's "return address", i.e. the address of the endpoint that wants to receive messages.

Keep in mind that the publisher may choose to store subscriptions in a highly available form. This allows multiple processes on multiple machines to publish messages to all subscribers, even if a specific publisher didn't receive the given subscription request.

Subscribers don't have to subscribe themselves. Through the use of the return address pattern, one central configuration component may send multiple messages to each publisher, specifying which subscriber endpoints to subscribe to which message.

Another approach is having multiple physical subscribers which act as a single logical subscriber. This makes it possible to load balance the handling of messages between multiple physical subscribers without any explicit coordination by the publisher or any one subscriber. All that is needed is that each subscriber to specify the same logical return address in the subscription message.

#### Publishing

![Publishing process](publish.png)

Publishing a message involves sending that message to all endpoints that previously subscribed to that message type.

Messages that are published often represent events, i.e. things that have happened. For example, "order cancelled", "product went out of stock", and "shipping delayed". Sometimes an event is published after handling a command. For example, successful handling of a "cancel order" command may result in publishing an "order cancelled" event. A publisher does not have to publish an event after handling a command, but it is a common scenario.

Since many commands may be received in a short period of time, publishing an event to all subscribers for each command may saturate the system with messages, and may not be the best solution. A better solution may be to publish a single message as a result of all the commands that were handled over a given period of time. The appropriate period of time depends on the Service Level Agreement of the publisher with respect to how soon an event should be published after a given command is handled. For example, in a financial domain that may have to be as little as 10 ms, but in an e-commerce, a minute may be acceptable.

### Command query separation

Many systems provide users with the ability to search, filter, sort, and change data.

In some client-server systems, a server simply exposes all CRUD (create, read, update, and delete) operations to the client. However, when the same database table is used both to perform CRUD operations in a highly consistent manner while handling commands, and to query data for users to read, those commands and queries contend with each other. This often results in poor system performance, both for commands and queries.

This problem can be avoided by separating commands and queries at the system level, above even the client and server. This solution takes advantage of the fact that in many, or even most, scenarios, the data returned to users does not have to be completely up to date; it can be slightly out of date without causing significant problems.

In this solution there are two components that each span both client and server. One component handles commands and the other responds to queries. The components communicate using only messages and their data is held separately, possibly even in separate databases, servers, or storage technologies. One component cannot access the other's data:

![Command Query Separation](cqs.png)

The command component publishes messages and the query component subscribes to them. When the query component receives a message, it stores appropriate data in a schema which is often optimized for queries, such as a star schema in a database or a cache of JSON documents. It may also cache some query responses in memory.
