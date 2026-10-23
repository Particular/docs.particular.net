---
title: Immutable Messages
reviewed: 2026-09-29
component: Core
related:
- samples/immutable-messages
---

Messages are usually designed as [DTOs](https://en.wikipedia.org/wiki/Data_transfer_object), i.e. a plain class with public properties that can be read and changed. This model is simple and will always work. An alternative is immutable messages, which follow the coding philosophy that a message should not change once it has been created.

> [!NOTE]
> Serialized messages are immutable once they have been sent. Changing a property value on the message object afterwards does not change the serialized copy that is forwarded to an error or audit queue.

Message objects can be made immutable at runtime by:

1. Using [record types](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/records).
2. Using properties with only public getters and initializing them via constructors.
3. Using a regular message class with public getters and setters on the sender side, which implements an interface that has only public getters. Receivers reference only the interface.


## Record types

Record types are the simplest way to create an immutable message. Their properties are set when the record is created and cannot be changed afterward.

```c#
public record CancelOrder(int OrderId);
```

## Properties with only public getters

Properties can be made read-only from outside the class by giving them a public getter and a private setter, with values set through the constructor.

> [!NOTE]
> Not all serializers [support deserialization to private setters](/nservicebus/serialization/#immutable-message-types).

```c#
public class CancelOrder : ICommand
{
    public CancelOrder(int orderId)
    {
        OrderId = orderId;
    }

    public int OrderId { get; private set; }
}
```

## Classes with public setters, interfaces with only getters

Using private setters is not supported by all serializers. An alternative is to make use of NServiceBus's support for [multiple inheritance and polymorphic dispatch](/nservicebus/messaging/messages-as-interfaces.md). With this approach, the message contract is defined as an interface that contains only getters, and the message handler uses that interface. The sender creates the message using a class that implements the interface and exposes public setters, then passes it to `Send` or `Publish`.

> [!NOTE]
> Not all transport configurations support polymorphic dispatch.

```c#
public class CancelOrder : ICancelOrder
{
    public CancelOrder(int orderId)
    {
        OrderId = orderId;
    }

    public int OrderId { get; set; } // Public setter
}

public interface ICancelOrder : IMessage
{
    int OrderId { get; } // Only getter
}
```
