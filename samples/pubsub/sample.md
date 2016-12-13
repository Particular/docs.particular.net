---
title: Publish/Subscribe
summary: Publish/Subscribe, fault-tolerant messaging, and durable subscriptions.
reviewed: 2016-12-12
component: Core
tags:
 - Publish Subscribe
 - Messaging Patterns
 - Durability
 - Fault Tolerance
redirects:
 - nservicebus/publish-subscribe-sample
related:
 - nservicebus/messaging/publish-subscribe
---

Before running the sample, look over the solution structure, the projects, and the classes. The projects `Publisher`,and `Subscriber` are Console Applications that each host an instance of NServiceBus.


## Defining messages

The `Shared` project contains the definition of the messages that are sent between the processes. Open "OrderReceived.cs" to see the message that will be published by this sample. Note that this event implements a marker interface called `IEvent` to denote that this message is an event. To define messages without adding a dependency to NServiceBus, use [Unobtrusive Mode Messages](/nservicebus/messaging/unobtrusive-mode.md). 


## Creating and publishing messages

As the name implies, the `Publisher` project is a publisher of event messages. It uses the NServiceBus API to publish the `OrderReceived` event every time the `1` key is pressed. The created message is populated and [published](/nservicebus/messaging/publish-subscribe/) using the `Publish` API.

snippet:PublishLoop


## Implementing subscribers

To receive messages from the publisher, the subscribers [must subscribe to the message types](/nservicebus/messaging/publish-subscribe/) they are designed to handle. A subscriber must have a handler for the type of message and a [configuration](/nservicebus/messaging/publish-subscribe/) that tells the endpoint where to send subscriptions for messages:

 * The `Subscriber` process handles and subscribes to the `OrderReceived` type.
 * The handlers in each project are in files that end in with the word `Handler` for example `OrderReceivedHandler.cs`. 
 * `Subscriber` process uses the default auto-subscription feature of the bus where the the bus automatically sends subscription messages to the configured publisher. [The auto-subscribe feature can be explicitly disabled](/nservicebus/messaging/publish-subscribe/controlling-what-is-subscribed.md) as part of the endpoint configuration.
  

## Run the sample

When running the sample, notice the three open console applications and many log messages on each. Almost none of these logs represent messages sent between the processes.

Bring the `Publisher` process to the foreground.

Click the `1` key repeatedly in the `Publisher` process console window, and see how the messages appear in the `Subscriber` console window. 


## Message Flow

Note: The below [persistence-based publish-subscribe](/nservicebus/messaging/publish-subscribe/#mechanics-persistence-based-message-driven) is the behavior for [unicast transports](/nservicebus/transports/#types-of-transports-unicast-only-transports).  [Broker transports](/nservicebus/transports/#types-of-transports-broker-transports) have [native publish-subscribe](/nservicebus/messaging/publish-subscribe/#mechanics-native) features.
  

### Subscribe

`Subscriber` sends a subscription message to `Publisher` which is stored in the [persistence](/nservicebus/persistence/) of `Publisher`. 

```mermaid
sequenceDiagram
Participant Subscriber As Subscriber
Participant Publisher As Publisher
Participant Persistence As Persistence
Subscriber ->> Publisher: Subscribe to OrderReceived
Publisher ->> Persistence: Store "Subscriber wants OrderReceived"
```

### Publish

When a publish occur on `Publisher` it queries the persistence for subscribers.

```mermaid
sequenceDiagram

Participant Subscriber As Subscriber
Participant Publisher As Publisher
Participant Persistence As Persistence 
Note over Publisher: Publish OrderReceived occurs
Publisher ->> Persistence: Requests "who wants OrderReceived"
Persistence ->> Publisher: Subscriber
Publisher ->> Subscriber: Send OrderReceived
```


## Fault-tolerant messaging

Shut down `Subscriber` by closing its console window. Return to the `Publisher` process and publish a few more messages by pressing the `1` key several more times. Notice how the publishing process does not change and there are no errors even though the subscriber process is no longer running.

In Visual Studio, right click the project of the closed subscriber, and restart it by right clicking the `Subscriber` project and selecting `Debug` and then `Start new instance`.

Note how `Subscriber` immediately receives the messages that were published while it was not running. The publisher safely places the message into the transport in this case MSMQ without knowledge of the running status of any subscriber. MSMQ safely places the message in the inbound queue of the subscriber where it awaits handling. Even when processes or machines restart, NServiceBus protects messages from being lost.


## MSMQ Subscriber Authorization

WARNING: The [Subscriber Authorization](/nservicebus/msmq/subscription-authorisation.md) feature is only supported when using the [MSMQ Transport](/nservicebus/msmq/).

Endpoints can be configured to prevent from either being able to subscribe to events from other endpoints or the ability to unsubscribe from events. This feature is only available when using the MSMQ transport.