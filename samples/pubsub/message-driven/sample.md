---
title: Message-driven Publish/Subscribe
summary: Persistence based Publish/Subscribe for unicast-only transports.
reviewed: 2020-09-28
component: Core
related:
 - nservicebus/messaging/publish-subscribe
---

This sample shows how to publish an event from one endpoint, subscribe to the event in a separate endpoint, and execute a message handler when an event is received. This sample uses the [message-driven publish-subscribe mechanism](/nservicebus/messaging/publish-subscribe#mechanics-message-driven-persistence-based) for unicast transports.

downloadbutton

Before running the sample, review the solution structure, the projects, and the classes. The projects `Publisher` and `Subscriber` are console applications that each host an instance of an NServiceBus messaging endpoint.

## Defining messages

The `Shared` project contains the definition of the messages that are sent between the processes. Open "OrderReceived.cs" to see the message that will be published by this sample. Note that this event implements an interface called `IEvent` to denote that this message is an event. To define messages without adding a dependency to NServiceBus, use [unobtrusive mode messages](/nservicebus/messaging/unobtrusive-mode.md). 

## Publishing the event

As the name implies, the `Publisher` project is a publisher of event messages. It uses the NServiceBus API to publish the `OrderReceived` event every time the <kbd>1</kbd> key is pressed. The created message is populated and [published](/nservicebus/messaging/publish-subscribe/) using the `Publish` API.

snippet: PublishLoop

## Subscribing to the event

To receive messages from the publisher, the subscribers [must subscribe to the message types](/nservicebus/messaging/publish-subscribe/) they are designed to handle. A subscriber must have a handler for the type of message and a configuration that tells the endpoint where to send subscriptions for messages to:

snippet: SubscriptionConfiguration

 * The `Subscriber` handles and subscribes to the `OrderReceived` event type.
 * The handlers in each project are in files that end with the word `Handler` for example `OrderReceivedHandler.cs`.
 * `Subscriber` uses the default auto-subscription feature of the bus where the bus automatically subscribes to the configured publisher. [The auto-subscribe feature can be explicitly disabled](/nservicebus/messaging/publish-subscribe/controlling-what-is-subscribed.md) as part of the endpoint configuration.
  
## Run the sample

When running the sample, two console applications will open. Bring the `Publisher` endpoint to the foreground.

Press the <kbd>1</kbd> key repeatedly in the `Publisher` process console window and notice how the messages appear in the `Subscriber` console window.

## Subscription mechanics

When starting up, the `Subscriber` endpoint sends a subscription message to the `Publisher` endpoint. The publisher then retrieves all subscribed endpoints from the persistence once an event is published. See the [publish-subscribe documentation](/nservicebus/messaging/publish-subscribe#mechanics-message-driven-persistence-based) for further details.

## Fault-tolerant messaging

Shut down `Subscriber` by closing its console window. Return to the `Publisher` process and publish a few more messages by pressing the <kbd>1</kbd> key several more times. Notice how the publishing process does not change and there are no errors even though the subscriber process is no longer running.

In Visual Studio, right-click the project of the closed subscriber. Restart it by right-clicking the `Subscriber` project and selecting `Debug` followed by `Start new instance`.

Note how `Subscriber` immediately receives the messages that were published while it was not running. The publisher safely places the event into the subscriber's queue without knowledge of the running status of any subscriber. Even when processes or machines restart, NServiceBus protects messages from being lost. 
