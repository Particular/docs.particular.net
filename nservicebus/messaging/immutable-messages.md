---
title: Immutable Messages
reviewed: 2019-07-23
component: Core
---

Usually messages are designed as [DTO](https://en.wikipedia.org/wiki/Data_transfer_object)'s which would mean POCO's. This model is simple and will always work. Immutable message are sometimes considered from a coding philopsy.

Note: Serialized messages are immutable once on the wire, changing property values will not result in a message that is forwarded to an error or audit queue to contain a different value. 

Messages objects can be made immutable at runtime by:

1. Creating properties that only have public getters and to initialized these properties via contructor initialization.
2. Have a class with public getters/setters for message sending, have it inherit an interface with only public getters for message processing


## Properties with only public getters

Note: [Not all serializers support deserialization to private getters](/nservicebus/serialization/index.md)

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

Using private settersis not supported by all serializers. An alternative is to have a class that contains public setters/getters so that any serializer can deserialize and make use of NServiceBus its support for [multiple inheritance and polymorphic dispatch](messages-as-interfaces.md). Interfaces are used only by message handlers and classes only to create the message and pass it to `Send` or `Publish`.

Note: Not all transport configurations support polymorphic dispatch.

```c#
public class CancelOrder : ICreateOrder
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