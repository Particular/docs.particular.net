---
title: Messages, events, and commands
summary: Messages as commands or events are the the unit of communication for message-based distributed systems. NServiceBus ensures they are used correctly.
component: Core
reviewed: 2026-09-30
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

A message is the unit of communication for NServiceBus. There are two types of messages: commands and events. This distinction enables users to express the intent of messages and to follow messaging best-practices.

## Commands

A command tells a service to do something. Typically, a command should only be consumed by a single consumer. For example, if there is a command, called `SubmitOrder`, then there should only be one handler or saga that implements `IHandleMessages<SubmitOrder>`.

Commands should be expressed in a verb-noun sequence, following the tell style:

- UpdateCustomerAddress
- UpgradeCustomerAccount
- SubmitOrder

## Events

An event signifies that something has happened. Events can be consumed by multiple consumers that are interested in reacting to the event occurring.

Events should be expressed in a noun-verb (past tense) sequence, indicating that something happened. Some example event names may include:

- CustomerAddressUpdated
- CustomerAccountUpgraded
- OrderSubmitted
- OrderAccepted
- OrderRejected
- OrderShipped

## Commands vs Events

Command | Event
-- | --
Used to make a request to perform an action | Used to communicate that an action has been performed
Has one logical owner | Has one logical owner
Should be sent to the logical owner | Should be published by the logical owner
Cannot be published | Cannot be sent
Cannot be subscribed to or unsubscribed from | Can be subscribed to and unsubscribed from
Can be sent using the [gateway](/nservicebus/gateway) | Cannot be sent using the [gateway](/nservicebus/gateway)

> [!NOTE]
> In a request and response pattern, reply messages are neither a command nor an event.

### Validation

There are checks in place to ensure best practices are followed. Violations of the above guidelines generate the following exceptions:

 * `Pub/sub is not supported for commands, so they should be be sent to their logical owner instead.`
   * Thrown when attempting to publish a command or subscribe to/unsubscribe from a command
 * `Events can have multiple recipients, so they should be published.`
   * Thrown when attempting to use `Send()` to send an event
 * `Reply is not supported for commands or events. Commands should be sent to their logical owner. Events should be published.`
   * Thrown when attempting to reply with a command or an event
 * `Cannot configure routing for type {name} because it is not considered a message. Message types have to either implement NServiceBus.IMessage interface or match a defined message convention.`
   * Thrown when configuring the destination endpoint for a non-message type
 * `Cannot configure routing for assembly {name} because it contains no types considered as messages. Message types have to either implement NServiceBus.IMessage interface or match a defined message convention.`
   * Thrown when configuring the destination endpoint for an assembly that contains no types considered messages
 * `Cannot configure routing for namespace {name} because it contains no types considered as messages. Message types have to either implement NServiceBus.IMessage interface or match a defined message convention.`
   * Thrown when configuring the destination endpoint for a namespace that contains no types considered messages
 * `Cannot configure publisher for type {name} because it is not considered a message. Message types have to either implement NServiceBus.IMessage interface or match a defined message convention.`
   * Thrown when configuring the publisher for a type that is not a message
 * `Cannot configure publisher for type {name} because it is not considered an event. Event types have to either implement NServiceBus.IEvent interface or match a defined event convention.`
   * Thrown when configuring the publisher for a type that is not an event
 * `Cannot configure publisher for type {name} because it is a command.`
   * Thrown when configuring the publisher for a command

 This enforcement is enabled by default but can be [disabled](best-practice-enforcement.md).

## Designing messages

A message can be defined using a [class](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/classes), [record](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/records), or an [interface](/nservicebus/messaging/messages-as-interfaces.md). Messages should focus on data only and avoid including methods or other business logic. Treating messages as simple contracts makes them easier to version and evolve over time.

Ideally, a good message type will:

* Be as small as possible
* Satisfy the [Single Responsibility Principle](https://en.wikipedia.org/wiki/Single_responsibility_principle)
* Favor simplicity and redundancy over object-oriented practices like inheritance
* Not be re-used for other purposes (e.g., domain objects, data access objects, or UI binding objects)

Generic message definitions (e.g., `MyMessage<T>`) are not supported. It is recommended to use dedicated, simple types for each message.

Messages define the data contracts between endpoints. More details are available in the [sharing message contracts](sharing-contracts.md) documentation.

By following these guidelines, message types are generally more compatible with [serializers](/nservicebus/serialization) and tend to be more evolvable over time.

## Identifying messages

Endpoints will process any message that can be deserialized into a .NET type but requires message contracts to be identified upfront to support:

* [Automatic subscriptions](/nservicebus/messaging/publish-subscribe/controlling-what-is-subscribed.md) for event types
* [Routing based on `namespace` or `assembly`](/nservicebus/messaging/routing.md) for commands

Messages can be defined by implementing a marker interface or specifying a custom convention.

### Marker interfaces

The simplest way to identify messages is to use interfaces.

* `NServiceBus.ICommand` for a command
* `NServiceBus.IEvent` for an event
* `NServiceBus.IMessage` for any other message type (e.g., a reply in a request/response pattern)

```csharp
public class MyCommand : ICommand { }

public class MyEvent : IEvent { }

public class MyMessage : IMessage { }
```

The interfaces are available in the [NServiceBus.MessageInterfaces](https://www.nuget.org/packages/NServiceBus.MessageInterfaces) package. The package targets `netstandard2.0` and has a stable version number which is highly unlikely to change. Using these well-defined interfaces should be preferred over conventions because the `NServiceBus.MessageInterfaces` package can be used to create a shared message assembly that can be used by multiple major versions of NServiceBus while still relying on the `ICommand` and `IEvent` marker interfaces.

### Conventions

[Custom conventions](/nservicebus/messaging/conventions.md) can be used to identify the types used as contracts for messages, commands, and events. This is known as [unobtrusive mode](unobtrusive-mode.md).
