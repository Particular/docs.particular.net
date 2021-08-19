---
title: Immutable Messages
reviewed: 2019-07-23
component: Core
related:
- samples/immutable-messages
---

Usually messages are designed as [DTOs](https://en.wikipedia.org/wiki/Data_transfer_object), i.e. a plain class with public properties. This model is simple and will always work. Immutable message are sometimes considered from a coding philosophy that messages should not be able to be changed after they are created.

Note: Serialized messages are immutable once on the wire, changing property values will not result in a message that is forwarded to an error or audit queue to contain a different value. 

Message objects can be made immutable at runtime by:

1. Creating properties with only public getters and initializing these properties via constructor initialization.
2. Having regular message classes with public getters/setters at the sender, where these classes implement an interface with only public getters. Receivers reference only the interface.


## Properties with only public getters

Note: [Not all serializers support deserialization to private setters](/nservicebus/serialization/).

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

Using private setters is not supported by all serializers. An alternative is to have a class that contains public setters/getters so that any serializer can deserialize and make use of NServiceBus its support for [multiple inheritance and polymorphic dispatch](/nservicebus/messaging/messages-as-interfaces.md). Interfaces are used only by message handlers and classes only to create the message and pass it to `Send` or `Publish`.

Note: Not all transport configurations support polymorphic dispatch.

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
