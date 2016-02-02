---
title: Publish/Subscribe
summary: Publish/Subscribe, fault-tolerant messaging, and durable subscriptions.
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

Before running the sample, look over the solution structure, the projects, and the classes. The projects `MyPublisher`, `Subscriber1`, and `Subscriber2` are Console Applications that each host an instance of NServiceBus.


## Defining messages

The "Shared" project contains the definition of the messages that are sent between the processes. Note that there are no project references to NServiceBus. Open "Messages.cs" to see that it contains a standard `IMyEvent` interface and two different class definitions.


## Creating and publishing messages

As the name implies, the "MyPublisher" project is a publisher of event messages. It uses the bus framework to send alternatively three different types of messages every time you click Enter in its console window. The created message is populated and [published](/nservicebus/messaging/publish-subscribe/) using `Publish`.

snippet:PublishLoop


## Implementing subscribers

To receive messages from the publisher, the subscribers [must subscribe to the message types](/nservicebus/messaging/publish-subscribe/) they are designed to handle. A subscriber must have a handler for the type of message and a [configuration](/nservicebus/messaging/publish-subscribe/) that tells the bus where to send subscriptions for messages:

 * The `Subscriber1` process handles and subscribes to both the `EventMessage` and `AnotherEventMessage` types.
 * The `Subscriber2` handles and subscribes to any message implementing the interface `IMyEvent`.

The handlers in each project are in files that end in with the word `Handler` for example `EventMessageHandler.cs`. Since both the `EventMessage` and `AnotherEventMessage` classes in the `Shared` project implement the `IMyEvent` interface, when they are published both subscribers receive it. When the specific message types of `EventMessage` and `AnotherEventMessage` are published, only the handlers of that specific type in `Subscriber1` are invoked.

 * `Subscriber1` uses the default auto-subscription feature of the bus where the the bus automatically sends subscription messages to the configured publisher.
 * `Subscriber2` explicitly disables the auto-subscribe feature in the `Program.cs` file. The subscriptions are therefore done explicitly at startup.


## Run the sample

When running the sample, you'll see three open console applications and many log messages on each. Almost none of these logs represent messages sent between the processes.

Bring the `MyPublisher` process to the foreground.

Click Enter repeatedly in the `MyPublisher` processes console window, and see how the messages appear in the other console windows. `Subscriber2` handles every published message and `Subscriber2` only handles `EventMessage` and `AnotherEventMessage`.

Now let's see some of the other features of NServiceBus.


## Fault-tolerant messaging

Shut down `Subscriber1` by closing its console window. Return to the `MyPublisher` process and publish a few more messages by clicking Enter several more times. Notice how the publishing process does not change and there are no errors even though one of the subscribers is no longer running.

In Visual Studio, right click the project of the closed subscriber, and restart it by right clicking the `Subscriber1` project and selecting `Debug` and then `Start new instance`.

Note how `Subscriber1` immediately receives the messages that were published while it was not running. The publisher safely places the message into the transport in this case MSMQ without knowledge of the running status of any subscriber. MSMQ safely places the message in the inbound queue of the subscriber where it awaits handling, so you can be sure that even when processes or machines restart, NServiceBus protects your messages so they won't get lost.
