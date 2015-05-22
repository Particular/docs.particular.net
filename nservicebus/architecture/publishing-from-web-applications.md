---
title: Publishing Events from Web Applications
summary: "Guidance on why you should or should not publish events from a web application"
tags: []
redirects:
---

You may have heard that [it's bad practice to publish events from a web application](http://www.make-awesome.com/2010/10/why-not-publish-nservicebus-messages-from-a-web-application/). This guidance is a bit outdated, as improvements Microsoft has made to IIS, more varied methods of hosting .NET web applications, and the addition of additional NServiceBus transports have made things much more nuanced.

Yes, it is possible, and in some circumstances, even advisable to publish events from within a web application. This article will describe the guidelines for publishing messages within web applications under different circumstances.

## Why/Why Not?

Historically, there have been good reasons to avoid publishing events from within web applications:

1. **Transactions and Consistency** - Generally good events should announce that something has *already* happened. Many times this involves making some changes to a database and then publishing the result. But HTTP is inherently unreliable and does not have built-in retries. If an exception occurs before the event is published, you may lose your only opportunity to publish that event. In these circumstances, it's generally better to send a command with the payload of the HTTP request, and have another endpoint process that command with the advantages of automatic retry and an error queue.
2. **Web Application Scale-Out** - Because events can only be published from one logical publisher, it can be problematic to scale out a web application that is publishing events. More on this later.
3. **Web App Recycling** - Prior to IIS 7.5 introducing the AlwaysRunning feature (AKA AlwaysOn), web applications not receiving could be recycled or idled at any time, which would result in incoming messages building up in an input queue, without an application actively processing them.

Because of this, conventional wisdom has suggested that when in the context of a web application, it is better to only send commands to a back-end service endpoint, which can then publish a similar event.

But, things change:

* With IIS 7.5 and later, IIS supports AlwaysRunning mode.
* IIS is no longer the only way to host web applications, which can now be hosted by [OWIN](http://owin.org/)/[Katana](http://www.asp.net/aspnet/overview/owin-and-katana), [NancyFx](http://nancyfx.org/), [FubuMVC](http://mvc.fubu-project.org/), and others. This largely removes the concern over web application recycling.
* NServiceBus 4.0 introduced the ability to have transports besides MSMQ. Not all of the transports handle Publish/Subscribe in the same way as MSMQ - some even support it natively.
* Cloud transports generally have an associated cost per interaction. This means that requiring an unnecessary send before publish costs extra money to operate.
* In NServiceBus 4.0 and 5.0, the process of creating a send-only endpoint (which is still capable of publishing messages, but does not have an input queue or process incoming messages) was made much easier.

## Publish/Subscribe Mechanics

First it's important to understand how publish/subscribe actually works, consisting of the subscription phase, and then the actual publish operation.

### Subscription

An endpoint must register its interest in a message by subscribing to a specific message type. How this occurs is dependent upon the message transport being used.

Some transports (Azure Service Bus, RabbitMQ) support publish/subscribe natively. This means that when an endpoint wants to subcribe to an event, it contacts the broker directly, and the broker maintains the list of subscriptions.

Other transports (MSMQ, SQL, Azure Storage Queues) do not support native publish/subscribe, and instead use storage-driven publishing with message-driven subscriptions. This means that each endpoint is responsible for maintaining its own subscription storage, usually in a database. When an endpoint wants to subscribe to an event, it sends a subscription request message to the owner endpoint, which will update its own subscription storage.

### Publishing

The publishing process also differs based on the message transport.

A transport that supports native publish/subscribe (Azure Service Bus, RabbitMQ) will send the event message directly to the broker, addressed to a topic, which will distribute the message to all interested parties.

A transport that uses storage-driven publishing will act differently. When it needs to publish, it will first consult its subscription storage to determine the interested parties. It will then send an independent copy of the event message to each subscriber.

While it is common to think of an event being published *from* a specific location, this doesn't always reflect the reality of the messaging mechanics. For native pub/sub brokers, the "publish from" location (the `Endpoint` value in the `MessageEndpointMappings` section) relates to a Topic in the broker.

In the case of transports, however, the "publish from" location actually relates to the input queue that is set up to receive subscription requests.

## Load Balancing

Typically, web applications will be scaled out with a network load balancer either to handle large amounts of traffic, or for high availability, or both. This is further complicated by elastic cloud scenarios, where additional instances of a web role can be provisioned and later removed, either manually or automatically, to keep up with a changing amount of load.

An important distinction of an event is that it is published from a single logical sender, and processed by one or more logical receivers. It's important not to confuse this with the physical deployment of code to multiple processes. In the case of a scaled-out web application, the individual web application instances are physical deployments; all of these together can act as a single logical publisher of events, provided the following requirements are met.

In order for a scaled-out web application to publish events as a single logical publisher:

* For native pub/sub transports (Azure Service Bus, RabbitMQ), all web application instances must publish to the same topic.
* For transports with storage-driven publishing (MSMQ, SQL, Azure Storage Queues), all web application instances must share the same subscription storage.

> **Note about Azure Service Bus**: Prior to Version 6.0, the Azure Service Bus transport did not offer native publish/subscribe capability, and used storage-driven publishing instead.

## Storage-driven Transport Topology

For storage-driven transports, it is inadvisable to have one of the web applications receive subscription requests. Instead, each web application instance can be implemented as an `ISendOnlyBus`, and a back-end service endpoint can be responsible for receiving the subscription request messages and updating the subscription storage.

> Need diagram here

In the diagram above, *describe diagram*

If the web applications need to also process messages from an input queue (for example, to receive notifications to drop cache entries) then a full `IBus` can be used, but none of the web servers will ever be used as the subscription endpoint for the events published by the web tier.

> Another, very similar diagram. Description after may be unnecessary.

In an elastic scale scenario, it's important for endpoints to unsubscribe themselves from events they receive when they shut down, since they may be decomissioned never to return.

## Summary

It can be useful, at times, to publish events from a web application. Always keep the following guidelines in mind.

* Be mindful of transport differences
    * For transports using native pub/sub must publish (Azure Service Bus, RabbitMQ), all web application instances must publish to the same topic.
    * Don't publish from the web tier if an exception before the event is published could lead to data loss.
* Never use an individual web server name to identify the source of an event being published, which would interfere with effective scale-out.
* If web application subscribes to events itself, use the IIS AlwaysRunning feature (or similar in other frameworks) to ensure messages don't build up in a queue due to a web application that is no longer listening.
