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

A Message is the unit of communication for NServiceBus. There are two sub-types of messages that capture more of the intent and help NServiceBus enforce messaging best practices. This enforcement is enabled by default unless disabled in configuration [configuration](configure-best-practice-enforcement.md). 

### Command

Used to request that an action should be taken. Characteristics include

-   Are not allowed to be published since all commands should have one logical owner and should be sent to the endpoint responsible for processing
-   Cannot be subscribed and unsubscribed to
-   Cannot implement `IEvent`

### Event

Used to communicate that some action has taken place. Characteristics include

-   Can be published
-   Can be subscribed and unsubscribed to
-   Cannot be sent using Bus.Send() since all events should be published
-   Cannot implement `ICommand`
-   Cannot be sent using the gateway, i.e., `bus.SendToSites()`

Note: For reply messages in a request and response pattern, you may want to use `IMessage` since these replies are neither a Command or an Event.

## Defining Messages

Messages can be defined via two mechanisms

### Marker interfaces

The simplest way to define a message is to use a marker interfaces. 

 * `NServiceBus.IMessage` for defining a Message
 * `NServiceBus.ICommand` for defining a Command
 * `NServiceBus.IEvent` for defining an Event


```C#
public class MyMessage : IMessage { }

public class MyComman : ICommand { }

public class MyEvent : IEvent { }
public interface MyEvent : IEvent { }
```

### Conventions 

Message Conventions is a way of defining what classes are Messages/Events or commands without the use of interfaces.



Note: It is important to include the `.Namespace != null`; otherwise a null reference exception will occur during the type scanning.

Encryption, databus, express messages TimeToBeReceived

When Message Conventions are combined with avoiding an reference to any NServiceBus assemblies this is referred to as [Unobtrusive Mode](unobtrusive-mode.md)