---
title: Messages, events, and commands
summary: Messages as commands or events are the the unit of communication for message-based distributed systems. NServiceBus ensures they are used correctly.
component: Core
reviewed: 2025-02-19
related:
 - nservicebus/messaging/conventions
 - nservicebus/messaging/unobtrusive-mode
 - samples/message-assembly-sharing
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

A _message_ is the unit of communication for NServiceBus. There are two types of messages, _commands_ and _events_, that capture more of the intent and users to follow messaging best-practices. 

## Commands

A command tells a service to do something, and typically a command should only be consumed by a single consumer. Commands are sent via either message handler context within a message handler, a saga, a pipeline behavior, a message or transactional session. For example if there is a command, such as SubmitOrder, then there should only be one handler or saga that implements `IHandleMessages<SubmitOrder>`.

Commands should be expressed in a verb-noun sequence, following the _tell_ style:

- UpdateCustomerAddress
- UpgradeCustomerAccount
- SubmitOrder

## Events

An event signifies that something has happened. Events are published via either message handler context within a message handler, a saga, a pipeline behavior, a message or transactional session.

Events should be expressed in a noun-verb (past tense) sequence, indicating that something happened. Some example event names may include:

- CustomerAddressUpdated
- CustomerAccountUpgraded
- OrderSubmitted, OrderAccepted, OrderRejected, OrderShipped

## Commands vs Events 

Command | Event
-- | --
Used to _make a request to perform an action_. | Used to _communicate that an action has been performed_.
Has one logical owner. | Has one logical owner.
Should be _sent to_ the logical owner. | Should be _published by_ the logical owner.
Cannot be _published_. | Cannot be _sent_.
_Cannot_ be subscribed to or unsubscribed from. | _Can_ be subscribed to and unsubscribed from.
_Can_ be sent using the [gateway](/nservicebus/gateway). | _Cannot_ be sent using the [gateway](/nservicebus/gateway).

> [!NOTE]
> In a request and response pattern, _reply_ messages are neither a command nor an event.

### Validation

There are checks in place to ensure best practices are followed. Violations of the above guidelines generate the following exceptions:

 * _"Pub/Sub is not supported for Commands. They should be sent directly to their logical owner."_ — thrown when attempting to publish a Command or subscribe to/unsubscribe from a Command.
 * _"Events can have multiple recipients so they should be published."_ — thrown when attempting to use `Send()` to send an event.
 * _"Reply is not supported for commands or events. Commands should be sent to their logical owner. Events should be published."_ — thrown when attempting to reply with a Command or an Event.
 * _"Cannot configure routing for type {name} because it is not considered a message. Message types have to either implement NServiceBus.IMessage interface or match a defined message convention."_ — thrown when configuring the destination endpoint for a non-message type.
 * _"Cannot configure routing for assembly {name} because it contains no types considered as messages. Message types have to either implement NServiceBus.IMessage interface or match a defined message convention."_ — thrown when configuring the destination endpoint for an assembly that contains no types considered messages.
 * _"Cannot configure routing for namespace {name} because it contains no types considered as messages..."_ — thrown when configuring the destination endpoint for a namespace that contains no types considered messages.
 * _"Cannot configure publisher for type {name} because it is not considered a message. Message types have to either implement NServiceBus.IMessage interface or match a defined message convention."_ — thrown when configuring the publisher for a type that is not a message.
 * _"Cannot configure publisher for type {name} because it is not considered an event. Event types have to either implement NServiceBus.IEvent interface or match a defined event convention."_ — thrown when configuring the publisher for a type that is not an event.
 * _"Cannot configure publisher for type {name} because it is a command."_ — thrown when configuring the publisher for a command.

 This enforcement is enabled by default but can be [disabled](best-practice-enforcement.md).

## Designing messages

A message can be defined using a [class](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/classes), [record](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/records), or an [interface](/nservicebus/messaging/messages-as-interfaces.md). Messages should focus on _data only_ and avoid including methods or other business logic. Treating messages as simple contracts makes them easier to version and evolve over time.

Ideally, a good message type will:

* Be as small as possible
* Satisfy the [Single Responsibility Principle](https://en.wikipedia.org/wiki/Single_responsibility_principle)
* Favor simplicity and redundancy over object-oriented practices like inheritance
* Not be re-used for other purposes (e.g., domain objects, data access objects, or UI binding objects)

Generic message definitions (e.g., `MyMessage<T>`) are not supported. It is recommended to use dedicated, simple types for each message.

Messages define the data contracts between endpoints. More details are available in the [sharing message contracts documentation](sharing-contracts.md).

By following these guidelines, message types are generally more compatible with [serializers](/nservicebus/serialization) and tend to be more evolvable over time.

## Identifying messages

Endpoints will process any message that can be deserialized into a .NET type but requires message contracts to be identified upfront to support:

* [Automatic subscriptions](/nservicebus/messaging/publish-subscribe/controlling-what-is-subscribed.md) for event types
* [Routing based on `namespace` or `assembly`](/nservicebus/messaging/routing.md) for commands

Messages can be defined by implementing a marker interface or specifying a custom convention.

### Marker interfaces

The simplest way to identify messages is to use interfaces.

* `NServiceBus.ICommand` for a command.
* `NServiceBus.IEvent` for an event.
* `NServiceBus.IMessage` for any other message type (e.g., a _reply_ in a request/response pattern).

```csharp
public class MyCommand : ICommand { }

public class MyEvent : IEvent { }

public class MyMessage : IMessage { }
```

Those interfaces are available in [NServiceBus.MessageInterfaces](https://www.nuget.org/packages/NServiceBus.MessageInterfaces). The project targets `netstandard2.0` has a stable version number which is highly unlikely to change. Using these well-defined interfaces should be prefered over conventions since `NServiceBus.MessageInterfaces` package can be used to create a shared message assembly that can be used by multiple major versions of NServiceBus, and in projects using different target frameworks, while still relying on the `ICommand` and `IEvent` marker interfaces.

### Conventions

[Custom conventions](/nservicebus/messaging/conventions.md) can be used to identify the types used as contracts for messages, commands, and events. This is known as [unobtrusive mode](unobtrusive-mode.md).
