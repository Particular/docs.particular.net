---
title: Messages, events and commands
summary: Messages, events, and commands and how to define them.
component: Core
reviewed: 2020-09-16
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

A *message* is the unit of communication for NServiceBus. There are two types of messages, _commands_ and _events_, that capture more of the intent and help NServiceBus enforce messaging best practices. This enforcement is enabled by default, but can be [disabled](best-practice-enforcement.md).

Command | Event
-- | --
Used to _make a request to perform an action_. | Used to _communicate that an action has been performed_.
Has one logical owner. | Has one logical owner.
Should be _sent to_ the logical owner. | Should be _published by_ the logical owner.
Cannot be _published_. | Cannot be _sent_.
_Cannot_ be subscribed to or unsubscribed from. | _Can_ be subscribed to and unsubscribed from.
_Can_ be sent using the [gateway](/nservicebus/gateway). | _Cannot_ be sent using the [gateway](/nservicebus/gateway).

Note: In a request and response pattern, _reply_ messages are neither a command nor an event.


### Validation

There are checks in place to ensure best practices are followed. Violations of the above guidelines generate the following exceptions:

partial: errors


## Designing messages

Messages represent data contracts between endpoints. They should be designed according to the following guidelines.

Messages should:

* be simple [POCO](https://en.wikipedia.org/wiki/Plain_old_CLR_object) types.
* be as small as possible.
* satisfy the [Single Responsibility Principle](https://en.wikipedia.org/wiki/Single_responsibility_principle). Types used for other purposes (e.g. domain objects, data access objects, or UI binding objects) should not be used as messages.

Note: Prior to NServiceBus version 7.2, messages had to be defined as a `class`. Defining them as a `struct` would result in a runtime exception.

Generic message definitions (e.g. `MyMessage<T>`) are not supported. It is recommended to use dedicated, simple types for each message or to use inheritance to reuse shared message characteristics.

### Solution structure

Messages define the data contract between two endpoints.

It's recommended to use a dedicated assembly for message contracts. By keeping message contracts in a separate assembly, the amount of information and dependencies shared between services is minimized. It is recommended to have a separate message assembly for every service. When doing so, a service can [evolve its contracts](/nservicebus/messaging/evolving-contracts.md) without impacting other services in the system. Every message contract should be declared in the contracts assembly of the service owning that message contract.

Consideration should be given to how the contracts assembly will be used by services. When certain events are subscribed to by multiple endpoints managed by other teams, it might make sense to extract those contracts into a separate NuGet package. Depending on the usage and the frequency of changes, separating contracts into multiple assemblies might decouple these contracts and minimize the impact of the changes.

It's also possible to share messages as C# source files without packaging them into an assembly. One advantage of this approach is that messages don't need to be compiled against specific NServiceBus versions, which means that assembly redirects are redundant. This can also be accomplished through the use of [unobstrusive mode](/nservicebus/messaging/unobtrusive-mode.md).

## Identifying messages

Endpoints will process any message that can be deserialized into a .NET type but requires message contracts to be identified up front to support:

* [Automatic subscriptions](/nservicebus/messaging/publish-subscribe/controlling-what-is-subscribed.md) for event types
* [Routing based on `namespace` or `assembly`](/nservicebus/messaging/routing.md) for commands

Messages can be defined either by implementing a marker interface or by specifying a custom convention.

### Marker interfaces

The simplest way to identify messages is to use interfaces.

* `NServiceBus.ICommand` for a command.
* `NServiceBus.IEvent` for an event.
* `NServiceBus.IMessage` for any other type of message (e.g. a _reply_ in a request response pattern).

```cs
public class MyCommand : ICommand { }

public class MyEvent : IEvent { }

public class MyMessage : IMessage { }
```

### Conventions

To avoid having message contract assemblies reference the NServiceBus assembly, [custom conventions](/nservicebus/messaging/conventions.md) can be used to identify the types used as contracts for messages, commands, and events. This is known as [unobtrusive mode](unobtrusive-mode.md).
