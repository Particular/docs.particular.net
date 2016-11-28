---
title: Architectural Principles
summary: NServiceBus helps write code that is robust in production environments, preventing data loss under failure conditions.
reviewed: 2016-08-10
redirects:
 - nservicebus/architectural-principles
---

Autonomy and loose coupling at design time and at run time are not things that any technology can give.

Service-oriented architecture (SOA) and event-driven architecture together provide the basis for identifying where to use NServiceBus.

Strategic Domain-Driven Design helps bridge the business/IT divide and drives the choice of business events published using NServiceBus.


## How NServiceBus aligns with SOA

<iframe src="//fast.wistia.net/embed/iframe/6g70txthct" allowtransparency="true" frameborder="0" scrolling="no" class="wistia_embed" name="wistia_embed" allowfullscreen mozallowfullscreen webkitallowfullscreen oallowfullscreen msallowfullscreen width="640" height="480"></iframe>

In this presentation, Udi Dahan explains the disadvantages of classical web services thinking that places services at a layer below the user interface and above the business logic. Instead, he describes an approach that cuts across all layers of an application, outlining the inherent lines of loose and tight coupling. Finally, Udi shows how these vertical services collaborate together using events in order to bring about flexible and high performance business processes.


## Drilling down into details

One of the problems with the distributed systems built today is that they are fragile. As one part of the system slows down, the effect tends to ripple out and cripple the entire system. One of the primary design goals of NServiceBus is to eliminate that, guiding developers to write code that is robust in production environments. That robustness prevents data loss under failure conditions.

To make effective use of NServiceBus, it is necessary to understand the distributed systems architecture it is designed to support. In other words, if designing the system according to the principles laid out below, NServiceBus will make life a lot easier. On the other hand, if these principles are not followed, NServiceBus will probably make it harder.

The extensibility features in NServiceBus allow customizing its behavior to suit the specific needs, yet they are documented separately.

The communications pattern that enables robustness is one-way messaging, also known as "fire and forget". This is discussed in more detail shortly.

Since the amount of time it can take to communicate with another machine across the network is both unknown and unbounded, communications are based on a store-and-forward model, as shown in the following figure.


## Messaging versus RPC

NServiceBus enforces queued messaging, which has profound architectural implications. The principles and patterns underlying queued messaging are decades old and battle-tested through countless technological shifts.

It's very simple and straightforward to build an application and get it working using traditional RPC techniques that WCF supports. However, scalability and fault-tolerance are inherently hindered. When using blocking calls it's close to impossible to solve these problems, and even throwing more hardware at it has little effect. WCF doesn't force developers down this path, but it also doesn't prevent them.

NServiceBus directs away from these problems right from the beginning. There's no such thing as a blocking call when one uses one-way messaging. Common, transient errors can be resolved automatically, it's also very easy to recover from failures that require some manual intervention. Above all, even when something goes wrong, no data gets lost. 

In order to learn more about the relationship between messaging and reliable, scalable, highly-available systems, watch Udi Dahan's presentation:

<iframe allowfullscreen frameborder="0" height="300" mozallowfullscreen src="https://player.vimeo.com/video/6222577" webkitallowfullscreen width="400"></iframe>


### Store and forward messaging

![Store and Forward Messaging](store-and-forward.png)

In this model, when the client process calls an API to send a message to the server process, the API returns control to the calling thread before the message is sent. At that point, the transfer of the message across the network becomes the responsibility of the messaging technology. There may be various kinds of communications interference, the server machine may be down, or a firewall may be slowing down the transfer. Also, even though the message may have reached the target machine, the target process may currently be down.

While all of this is going on, the client process is oblivious. Critical resources like threads (and it's allocated memory) are not held waiting for the call to complete. This prevents the client process from losing stability as a result of having many threads and all their memory used up waiting for a response from the other machine or process.


### Request/response and one-way messaging

The common pattern of Request/Response, which is more accurately described as Synchronous Remote Procedure Call, is handled differently when using one way messaging. Instead of letting the stack of the calling thread manage the state of the communications interaction, it is done explicitly. From a network perspective, request/response is just two one-way interactions, as shown in the next figure.

![Full duplex Request-Response messaging](full-duplex-messaging.png)

This communication is especially critical for servers as clients behind problematic network connections now have little effect on the server's stability. If a client crashes between the time that it sent the request until the server sends a response, the server will not have resources tied up waiting minutes and minutes until the connection times out.

When used in concert with Durable Messaging, system-wide robustness increases even more.

Durable messaging differs from regular store-and-forward messaging in that the messages are persisted to disk locally before attempting to be sent. This means that if after the calling thread has had control returned to it, the process crashes and the message sent is not lost. In server-to-server scenarios, where a server can complete a local transaction but might crash a second later, one-way durable messaging makes it easier to create an overall robust system even in the face of unreliable building blocks.

A different communication style involves one-to-many communication.


### Publish/subscribe

In this style, the sender of the message often does not know the specifics of those that wish to receive the message. This additional loose coupling comes at the cost of subscribers explicitly opting-in to receiving messages, as shown in the following diagram.


#### Subscriptions

![Subscription process](/nservicebus/messaging/publish-subscribe/subscribe.png)

Subscribers need to know which endpoint is responsible for a given message. This information is usually made available as part of the contract, specifying to which endpoint a subscriber should send its request. As a part of the subscription message, a subscriber passes its
"return address", the endpoint at which it wants to receive messages.

Keep in mind that the publisher may choose to store the information concerning which subscriber is interested in which message in a highly available manner. This allows multiple processes on multiple machines to publish messages to all subscribers, regardless if one received the subscription message or not.

Subscribers don't necessarily have to subscribe themselves. Through the use of the Return Address pattern, one central configuration station could send multiple messages to each publisher, specifying which subscriber endpoints to subscribe to which message.

Another option that can be used is for multiple physical subscribers to make themselves appear as one single logical subscriber. This makes it possible to load balance the handling of messages between multiple physical subscribers without any explicit coordination on the part of the publisher or the part of any one subscriber. All that is needed is for all subscribers to specify the same return address in the subscription message.


#### Publishing

![Publishing process](publish.png)

Publishing a message involves having the message arrive at all endpoints that previously subscribed to that type of message.

Messages that are published often represent events or things that have happened; for instance, Order Cancelled, Product Out of Stock, and Shipping Delayed. Sometimes, the cause of an event is the handling of a previous command message, for instance Cancel Order. A publisher is not required to publish a message as a part of handling a command message although it is the simplest solution.

Since many command messages can be received in a short period of time, publishing a message to all subscribers for every command message multiplies the incoming load and, as such, is a less than optimal solution. A better solution has the publisher rolling up all the changes that occurred in a given period of time into a single published message. The appropriate period of time depends on the Service Level Agreement of the publisher and its commitment to the freshness of the data. For instance, in the financial domain the publishing period may be 10ms, while in the business of consumer e-commerce, a minute may be acceptable.

Another advantage of publishing messages on a timer is that that activity can be offloaded from the endpoint/server processing command messages, effectively scaling out over more servers.


### Command query separation

Many systems provide users with the ability to search, filter, and sort data. While one-way messaging and publish/subscribe are core components of the implementation of these features, the way they are combined is not at all like a regular client-server request/response.

In regular client-server development, the server provides the client with all CRUD (create, read, update, and delete) capabilities. However, when users look at data they do not often require it to be up-to-date to the second (given that they often look at the same screen for several seconds to minutes at a time). As such, retrieving data from the same table as that being used for highly consistent transaction processing creates contention, resulting in poor performance for all CRUD actions under higher load.

A solution that avoids this problem separates commands and queries at the system level, even above that of client and server. In this solution there are two "services" that span both client and server: one in charge of commands (create, update, delete), and the other in charge of queries (read). These services communicate only via messages; one cannot access the database of the other, as shown in the following diagram:

![Command Query Separation](cqs.png)

The command service publishes messages about changes to data, to which the query service subscribes. When the query service receives such notifications, it saves the data in its own data store which may well have a different schema (optimized for queries like a star schema). The query service may also keep all data in memory if the data is small enough.
