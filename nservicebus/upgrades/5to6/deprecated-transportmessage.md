---
title: Deprecated TransportMessage in NServiceBus Version 6
summary: How to migrate away from TransportMessage in NServiceBus 6
reviewed: 2020-05-07
component: Core
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---

Previous versions used `TransportMessage` as a generic holder both for outgoing and incoming messages. For a better separation of concerns that class has been split into `IncomingMessage` and `OutgoingMessage`. All code paths related to outgoing messages use `OutgoingMessage` and all code paths related to incoming messages use `IncomingMessage`. The class `TransportMessage` has been deprecated entirely. Here are a few common scenarios related to `TransportMessage` and how they can be addressed with either `IncomingMessage` or `OutgoingMessage`.


## Body

Both `IncomingMessage` and `OutgoingMessage` provide a body byte array to access the underlying payload of the property `Body`.

When setting the body, raw sending is the most likely scenario. See [Raw sending](#raw-sending) below.


## Headers

Both `IncomingMessage` and `OutgoingMessage` provide a headers dictionary to get or set headers of the property `Headers`.


## ID

Both `IncomingMessage` and `OutgoingMessage` provide a message ID of the property `MessageId`.


## CorrelationId

The correlation ID is no longer a strongly-typed property exposed. To get access to the correlation ID of a message use the `Headers.CorrelationId` key.


## ReplyAddress

The `ReplyAddress` can only be accessed on an incoming message. Use the extension method `GetReplyAddress` on `IncomingMessage` to acquire the reply address.


## MessageIntent

The `MessageIntent` can only be accessed on an incoming message. Use the extension method `GetMessageIntent` on `IncomingMessage` to acquire the message intent.


## TimeToBeReceived

From the perspective of an outgoing message, the `TimeToBeReceived` is a delivery concern and must be specified over the newly introduced `DeliveryConstraint`.

Set the `TimeToBeReceived`

```csharp
var timeToBeReceived = TimeSpan.FromSeconds(25);
var deliveryConstraint = new DiscardIfNotReceivedBefore(timeToBeReceived);
context.Extensions.AddDeliveryConstraint(deliveryConstraint);
```

Read the `TimeToBeReceived`

```csharp
context.Extensions.TryGetDeliveryConstraint(out DiscardIfNotReceivedBefore constraint);
timeToBeReceived = constraint.MaxTime;
```

From the perspective of an incoming message, the `TimeToBeReceived` can be acquired by using the `Headers.TimeToBeReceived` on the `IncomingMessage.Headers` dictionary.


## Recoverable

From the perspective of an outgoing message, the `Recoverable` flag is a delivery concern and must be specified over the newly introduced `DeliveryConstraint`.

Set the `Recoverable`

```csharp
context.Extensions.AddDeliveryConstraint(new NonDurableDelivery());
```

Read the `Recoverable`

```csharp
context.Extensions.TryGetDeliveryConstraint(out NonDurableDelivery constraint);
```

From the perspective of an incoming message, the `Recoverable` flag can be acquired by using the `Headers.NonDurableMessage` on the `IncomingMessage.Headers` dictionary.


## Raw sending

In NServiceBus version 5, it was possible  to use `ISendMessages` to do raw sends. In version 6 `IDispatchMessages` was introduced. The following snippet shows an example of how to send raw messages:

```csharp
var headers = new Dictionary<string, string>();
var outgoingMessage = new OutgoingMessage("MessageId", headers, new byte[]
{
});
var constraints = new List<DeliveryConstraint>
{
    new NonDurableDelivery()
};
var address = new UnicastAddressTag("Destination");
var operation = new TransportOperation(
    message: outgoingMessage,
    addressTag: address,
    requiredDispatchConsistency: DispatchConsistency.Default,
    deliveryConstraints: constraints);
var operations = new TransportOperations(operation);
await dispatcher.Dispatch(operations, new TransportTransaction(), new ContextBag())
    .ConfigureAwait(false);
```
