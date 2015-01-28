---
title: Publish/Subscribe Sample
summary: Publish/subscribe, fault-tolerant messaging, and durable subscriptions.
tags:
- Publish Subscribe
- Messaging Patterns
- Durability
- Fault Tolerance
redirects:
- /nservicebus/publish-subscribe-sample.md
--- 

## Reviewing the solution

Before running the sample, look over the solution structure, the projects, and the classes. The projects `MyPublisher`, `Subscriber1`, and `Subscriber2` are Console Applications that each host an instance of NServiceBus. 

### Defining messages

The "MyMessages" project contains the definition of the messages that are sent between the processes. Note that there are no project references to NServiceBus. Open "Messages.cs" to see that it contains a standard `IMyEvent` interface and two different class definitions.

### Creating and publishing messages

As the name implies, the "MyPublisher" project is a publisher of event messages. It uses the bus framework to send alternatively three different types of messages every time you click Enter in its console window. The first message is constructed using the provided factory function `Bus.CreateInstance(<messagetype>)`, which creates a message that implements a specified interface or a specific type. This is in keeping with the suggested practice of [using interfaces for events](/nservicebus/messages-as-interfaces.md) . The other messages are created simply using the 'new' keyword. The created message is populated and
[published](/nservicebus/how-to-pub-sub-with-nservicebus.md) using `Bus.Publish`.

```C#
var eventId = Guid.NewGuid();
switch (nextEventToPublish)
{
    case 0:
        bus.Publish<IMyEvent>(m =>
        {
            m.EventId = eventId;
            m.Time = DateTime.Now.Second > 30 ? (DateTime?) DateTime.Now : null;
            m.Duration = TimeSpan.FromSeconds(99999D);
        });
        nextEventToPublish = 1;
        break;
    case 1:
        var eventMessage = new EventMessage
        {
            EventId = eventId,
            Time = DateTime.Now.Second > 30 ? (DateTime?) DateTime.Now : null,
            Duration = TimeSpan.FromSeconds(99999D)
        };
        bus.Publish(eventMessage);
        nextEventToPublish = 2;
        break;
    default:
        var anotherEventMessage = new AnotherEventMessage
        {
            EventId = eventId,
            Time = DateTime.Now.Second > 30 ? (DateTime?) DateTime.Now : null,
            Duration = TimeSpan.FromSeconds(99999D)
        };
        bus.Publish(anotherEventMessage);
        nextEventToPublish = 0;
        break;
}
```

### Implementing subscribers

To receive messages from the publisher, the subscribers [must subscribe to the message types](/nservicebus/how-to-pub-sub-with-nservicebus.md) they are designed to handle. A subscriber must have a handler for the type of message and a [configuration](/nservicebus/publish-subscribe-configuration.md) that tells the bus where to send subscriptions for messages:

 * The "Subscriber1" process handles and subscribes to both the "EventMessage" and "AnotherEventMessage" types.
 * The "Subscriber2" handles and subscribes to any message implementing the interface "IMyEvent".

The handlers in each project are in files that end in with the word `Handler` for example `EventMessageHandler.cs`. Since both the
`EventMessage` and `AnotherEventMessage` classes in the `MyMessages` project implement the `IMyEvent` interface, when they are published both subscribers receive it. When the specific message types of `EventMessage` and `AnotherEventMessage` are published, only the handlers of that specific type in `Subscriber1` are invoked.

 * `Subscriber1` uses the default auto-subscription feature of the bus where the the bus automatically sends subscription messages to the configured publisher.
 * `Subscriber2` explicitly disables the auto-subscribe feature in the `Program.cs` file. The subscriptions are therefore done explicitly at startup.

## Run the sample

When running the sample, you'll see three open console applications and many log messages on each. Almost none of these logs represent messages sent between the processes.

Bring the `MyPublisher` process to the forground.

Click Enter repeatedly in the `MyPublisher` processes console window, and see how the messages appear in the other console windows.
`Subscriber2` handles every published message and `Subscriber2` only handles `EventMessage` and `AnotherEventMessage`.

Now let's see some of the other features of NServiceBus.

## Fault-tolerant messaging

Shut down `Subscriber1` by closing its console window. Return to the `MyPublisher` process and publish a few more messages by clicking Enter several more times. Notice how the publishing process does not change and there are no errors even though one of the subscribers is no longer running.

In Visual Studio, right click the project of the closed subscriber, and restart it by right clicking the `Subscriber1` project and selecting `Debug` and then `Start new instance`. 

Note how `Subscriber1` immediately receives the messages that were published while it was not running. The publisher safely places the message into the transport in this case MSMQ without knowledge of the running status of any subscriber. MSMQ safely places the message in the inbound queue of the subscriber where it awaits handling, so you can be sure that even when processes or machines restart, NServiceBus protects your messages so they won't get lost.

## InMemory subscriptions by default

Since this is a sample it uses InMemory persistence so as to not rely on any installed components. For durable subscriptions switch over 

To use durable subscriptions you will need to switch over to one of the other persistences (RavenDB, SqlServer, NHibernate etc).

## Subscriber authorization

A publisher has control over the subscriptions it receives. By implementing the authorization methods of the `IAuthorizeSubscriptions` interface the publisher can return a Boolean operator indicating to the framework whether a subscription should be accepted. See the `SubscriptionAuthorizer.cs` file in the `MyPublisher` project for a basic example of this feature.