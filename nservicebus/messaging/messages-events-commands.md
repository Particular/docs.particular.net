---
title: Messages, Events and Commands
summary: What are Messages, Events and Commands and how to define them.
tags: 
- Unobtrusive
redirects:
- nservicebus/introducing-ievent-and-icommand
- nservicebus/messaging/introducing-ievent-and-icommand
- nservicebus/how-do-i-define-a-message
- nservicebus/define-a-message
- nservicebus/messaging/how-do-i-define-a-message
- nservicebus/definingmessagesas-and-definingeventsas-when-starting-endpoint
- nservicebus/messaging/definingmessagesas-and-definingeventsas
- nservicebus/how-do-i-centralize-all-unobtrusive-declarations
- nservicebus/invalidoperationexception-in-unobtrusive-mode
- nservicebus/messaging/invalidoperationexception-in-unobtrusive-mode
---

A *Message* is the unit of communication for NServiceBus. There are two sub-types of messages that capture more of the intent and help NServiceBus enforce messaging best practices. This enforcement is enabled by default unless disabled in [configuration](best-practice-enforcement.md). 


### Command

Used to request that an action should be taken. A *Command* is intended to be _sent to a receiver_ (all commands should have one logical owner and should be sent to the endpoint responsible for processing). As such, commands ...

-   are not allowed to be _published_. 
-   cannot be _subscribed_ to or _unsubscribed_ from.
-   cannot implement `IEvent`.


### Event

Used to communicate that some action has taken place. An *Event* should be _published_. An event ...

-   can be _subscribed_ to and _unsubscribed_ from.
-   cannot be sent using `Bus.Send()` (since _all events should be published_).
-   cannot implement `ICommand`.
-   cannot be sent using the gateway, i.e., `bus.SendToSites()`.

Note: For reply messages in a request and response pattern, you may want to use `IMessage` since these replies are neither a Command nor an Event.


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

A *message convention* is a way of defining what a certain type is instead of using a marker interface or an attribute.

We currently have conventions that can identity:

- Commands
- Events
- Messages
- [Encryption](/nservicebus/security/encryption.md)
- [DataBus](/nservicebus/messaging/databus.md)
- [Express messages](/nservicebus/messaging/non-durable-messaging.md)
- [TimeToBeReceived](/nservicebus/messaging/discard-old-messages.md)


When Message Conventions are combined with avoiding an reference to any NServiceBus assemblies this is referred to as [Unobtrusive Mode](unobtrusive-mode.md). This makes it also ideal to use in cross platform environments. Messages can be defined in a *Portable Class Library* (PCL) and shared across multiple platform even though not all platforms use NServiceBus for message processing.
