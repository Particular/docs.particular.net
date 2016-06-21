---
title: Messages, Events and Commands
summary: What are Messages, Events and Commands and how to define them.
tags:
 - Unobtrusive
 - Conventions
related:
 - nservicebus/messaging/conventions
 - nservicebus/messaging/unobtrusive-mode
redirects:
 - nservicebus/introducing-ievent-and-icommand
 - nservicebus/messaging/introducing-ievent-and-icommand
 - nservicebus/how-do-i-define-a-message
 - nservicebus/define-a-message
 - nservicebus/messaging/how-do-i-define-a-message
 - nservicebus/definingmessagesas-and-definingeventsas-when-starting-endpoint
 - nservicebus/messaging/definingmessagesas-and-definingeventsas
 - nservicebus/messaging/invalidoperationexception-in-unobtrusive-mode
---

A *Message* is the unit of communication for NServiceBus. There are two sub-types of messages that capture more of the intent and help NServiceBus enforce messaging best practices. This enforcement is enabled by default unless disabled in [configuration](best-practice-enforcement.md).

Note: Messages in NServiceBus have to be C# classes. Using C# structs instead will generate exceptions at runtime when attempting to send a message.

### Command

Used to request that an action should be taken. A *Command* is intended to be _sent to a receiver_ (all commands should have one logical owner and should be sent to the endpoint responsible for processing). As such, commands ...

 * Are not allowed to be _published_.
 * Cannot be _subscribed_ to or _unsubscribed_ from.
 * Cannot implement `IEvent`.


### Event

Used to communicate that some action has taken place. An *Event* should be _published_. An event:

 * Can be _subscribed_ to and _unsubscribed_ from.
 * Cannot be sent using `Bus.Send()` (since _all events should be published_).
 * Cannot implement `ICommand`.
 * Cannot be sent using the gateway, i.e., `bus.SendToSites()`.

Note: For reply messages in a request and response pattern, use `IMessage` since these replies are neither a Command nor an Event.


### Validation Messages (Version 3 - Version 6)

There are checks in place to ensure following of the best practices. While violating above rules the following exceptions can be seen:

 * "Pub/Sub is not supported for Commands. They should be sent direct to their logical owner." - this exception is being thrown when attempting to publish a Command or subscribe to/unsubscribe from a Command.
 * "Events can have multiple recipient so they should be published." - this exception will occur when attempting to use 'Bus.Send()' to send an event.
 * "Reply is neither supported for Commands nor Events. Commands should be sent to their logical owner using bus.Send and bus. Events should be Published with bus.Publish." - this exception is thrown when attempting to reply with a Command or an Event.

Note: In Version 3 and Version 4 there are 2 additional messages that can be seen:

 * "Reply is not supported for Commands. Commands should be sent to their logical owner using bus.Send and bus." - this exception is thrown when one use reply with a Command.
 * "Reply is not supported for Events. Events should be Published with bus.Publish." - this exception will occur when one tries to use reply with an Event.


## Defining Messages

Messages can be defined via *marker interfaces* or via *conventions*.


### Marker interfaces

The simplest way to define a message is to use marker interfaces.

 * `NServiceBus.IMessage` for defining a Message.
 * `NServiceBus.ICommand` for defining a Command.
 * `NServiceBus.IEvent` for defining an Event.

```C#
public class MyMessage : IMessage { }

public class MyCommand : ICommand { }

public class MyEvent : IEvent { }

public interface MyEvent : IEvent { }
```


 ### Conventions

 Conventions are described in the dedicated [Conventions](/nservicebus/messaging/conventions.md) article.


 ## Designing Messages

 Messages represent data contract between the endpoints. When creating messages one should follow the following guidelines:

 * Messages should be simple POCO objects.
 * Messages should be as small as possible.
 * Messages should satisfy a [Single Responsibility Principle](https://en.wikipedia.org/wiki/Single_responsibility_principle).

 Note that in order to satisfy the [Single Responsibility Principle](https://en.wikipedia.org/wiki/Single_responsibility_principle) classes used for other purposes (e.g. domain objects, data access objects and UI binding objects) should not be used as messages.
