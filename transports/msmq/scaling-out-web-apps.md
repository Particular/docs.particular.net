---
title: Scaling out Web Applications using MSMQ
summary: How to scale out web applications when using the MSMQ transport
component: MsmqTransport
related:
- samples/web
reviewed: 2018-10-15
---

In general, web applications using NServiceBus can be scaled out the same way as any other endpoint using [Sender-side distribution](). However, as web applications can scale up or down more frequently than regular endpoints, this quickly can become a maintenance burden. Consider the following recommendations when using NServiceBus with MSMQ transport on a web application:

## Send-only endpoints

[Send-only endpoints](/nservicebus/hosting/#self-hosting-send-only-hosting) allow to send and publish messages but without the message processing capabilities. As the web application itself does not handle business processes, they rarely need to consume messages themselves. This makes the web application process consuming less resources than using a regular endpoint and other endpoints don't need to be aware of the active instances.

NOTE: Send-only endpoint also are not capable of receicing event subscription messages when using MSMQ. This means that send-only endpoints with MSMQ transport cannot publish messages except when using the [Storage-driven transport topology](#storage-driven-transport-topology).


## Storage-driven Transport topology

For the MSMQ transport, it is inadvisable to have one of the web applications receive subscription requests. Instead, each web application instance can be implemented as a send-only endpoint, and a back-end service endpoint can be responsible for receiving the subscription request messages and updating the subscription storage.

![Storage-driven transport publishing topology](storage-based-publish-topology.png "width=400")

In the diagram above, two web servers are load balanced behind a network load balancer. The applications on both web servers cooperate by referring to the same subscription storage database.

An additional **Subscription Manager Endpoint** exists off to the side, also talking to the same subscription storage. When a subscriber is interested in an event, the **subscription request** message is routed here, not to either of the web servers. When a web server needs to publish an event, it looks up the event type in the subscription storage database, and publishes the event to the subscriber.

The subscription manager endpoint can be any endpoint identified to process the subscription requests, as long as it uses the same subscription storage as the web servers. The subscription manager endpoint can process other message types as well. This way, the web servers together with the subscription manager endpoint work in concert as one logical endpoint for publishing messages.

NOTE: HTTP is inherently unreliable and does not have built-in retries. If an exception occurs before the event is published, the only opportunity to publish that event may lost. In these circumstances, it's generally better to send a command with the payload of the HTTP request, and have another endpoint process that command with the advantages of [recoverability](/nservicebus/recoverability/).