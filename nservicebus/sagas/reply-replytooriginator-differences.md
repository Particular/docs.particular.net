---
title: Reply and ReplyToOriginator Differences
summary: Outlines the different behaviors of the Reply and ReplyToOriginator methods of the IMessageHandlerContext/IBus instance.
reviewed: 2024-11-19
redirects:
- nservicebus/sagas/reply-replaytooriginator-differences
---

When building systems using the [request/response pattern](/nservicebus/messaging/reply-to-a-message.md), the `Reply` method exposed by the `IMessageHandlerContext` is used to reply to the sender of the incoming message.

The same `Reply` method can be used inside a [saga](/nservicebus/sagas/) and it is important to understand that the `Reply` method always routes the message to the sender of the incoming message, _not the endpoint that started the saga_.

> [!NOTE]
> The `Reply` method always delivers the message to the sender address of the incoming message.

The following diagram details a scenario where two sagas and an integration endpoint utilize the request/response pattern to communicate. The replies are highlighted in red.

![Sample sequence diagram](reply-replytooriginator-differences.png)

The reason a call to `Reply(new ShipOrder())` sends a message to the `Shipment Gateway` is that it is invoked in the context of handling the `ShipmentReserved` message, and the return address of `ShipmentReserved` is `Shipment Gateway`.

In the context of a `Saga` it is not always clear at first glance who the sender of a message is. In the above example, when handling the expired `ShipmentReservation` timeout the sender of the message is the `Delivery Manager` endpoint. In this case a `Reply` would be delivered to the `Delivery Manager`, and that is not necessarily the desired behavior.

Calling `ReplyToOriginator` makes it clear to NServiceBus that the message has to be delivered to the endpoint that was the originator of the saga.
