---
title: Native Publish/Subscribe
summary: Publish/Subscribe with multicast-enabled transports.
reviewed: 2020-09-28
component: Core
redirects:
 - nservicebus/publish-subscribe-sample
related:
 - nservicebus/messaging/publish-subscribe
---

The Publish/Subscribe pattern allows greater flexibility in developing distributed systems by decoupling system components from each other. Once the system is capable of publishing an event, additional capabilities can be added to the system by adding a new event subscriber. A new subscriber can be added in a separate physical process, without changing or redeploying the code that publishes the message.

This sample shows how to publish an event message from a messaging endpoint in one physical process, subscribe to the event in a separate messaging endpoint in a second physical process, and execute a message handler when an event message is received.

downloadbutton

Before running the sample, look over the solution structure, the projects and the classes. The projects `Publisher` and `Subscriber` are console applications that each host an instance of an NServiceBus messaging endpoint.

## Defining messages

The `Shared` project contains the definition of the messages that are sent between the processes. Open "OrderReceived.cs" to see the message that will be published by this sample. Note that this event implements an interface called `IEvent` to denote that this message is an event. To define messages without adding a dependency to NServiceBus, use [Unobtrusive Mode Messages](/nservicebus/messaging/unobtrusive-mode.md). 

## Creating and publishing messages

As the name implies, the `Publisher` project is a publisher of event messages. It uses the NServiceBus API to publish the `OrderReceived` event every time the `1` key is pressed. The created message is populated and [published](/nservicebus/messaging/publish-subscribe/) using the `Publish` API.

snippet: PublishLoop

## Implementing subscribers

To receive messages from the publisher, the subscribers must subscribe to the message types they are designed to handle.

 * The `Subscriber` handles and subscribes to the `OrderReceived` type.
 * The handlers in each project are in files that end with the word `Handler` for example `OrderReceivedHandler.cs`.
 * `Subscriber` uses the default auto-subscription feature of the bus where the bus automatically subscribes to the configured publisher. [The auto-subscribe feature can be explicitly disabled](/nservicebus/messaging/publish-subscribe/controlling-what-is-subscribed.md) as part of the endpoint configuration.
  
## Run the sample

When running the sample, notice the two open console applications. Bring the `Publisher` endpoint to the foreground.

Click the `1` key repeatedly in the `Publisher` process console window and notice how the messages appear in the `Subscriber` console window.

## Message Flow

In multicast-enabled transports, the broker handles the subscription mechanism, routing published events to all subscribed endpoints. See the [publish-subscribe documentation](/nservicebus/messaging/publish-subscribe/#mechanics-native) for further details.

## Fault-tolerant messaging

Shut down `Subscriber` by closing its console window. Return to the `Publisher` process and publish a few more messages by pressing the `1` key several more times. Notice how the publishing process does not change and there are no errors even though the subscriber process is no longer running.

In Visual Studio, right-click the project of the closed subscriber. Restart it by right-clicking the `Subscriber` project and selecting `Debug` followed by `Start new instance`.

Note how `Subscriber` immediately receives the messages that were published while it was not running. The publisher safely places the message into the transport in this case LearningTransport without knowledge of the running status of any subscriber. LearningTransport safely places the message in the inbound queue of the subscriber where it awaits handling. Even when processes or machines restart, NServiceBus protects messages from being lost. 
