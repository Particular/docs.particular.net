---
title: Deprecated TransportMessage in Version 6
reviewed: 2016-10-26
component: Core
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---

Previous versions used `TransportMessage` as a generic holder both for outgoing and incoming messages. For a better separation of concerns that class has been split into `IncomingMessage` and `OutgoingMessage`. So all code paths related to outgoing messages will use `OutgoingMessage` and all code paths related to incoming messages will use `IncomingMessages`. The class `TransportMessage` has been deprecated entirely. Here are a few common scenarios related to `TransportMessage` and how they can be addressed with either `IncomingMessage` or `OutgoingMessage`.


## Body

Both `IncomingMessage` and `OutgoingMessage` provide a body byte array to get access to the underlying payload of the property `Body`.

When setting the body, raw sending is the most likely scenario. See section [Raw sending](#raw-sending).


## Headers

Both `IncomingMessage` and `OutgoingMessage` provide a headers dictionary to get or set headers of the property `Headers`.


## Id

Both `IncomingMessage` and `OutgoingMessage` provide a message ID of the property `MessageId`.


## CorrelationId

The correlation ID is no longer a strongly typed property exposed. To get access to the correlation ID of a message use the `Headers.CorrelationId` key.


## ReplyAddress

The `ReplyAddress` can only be accessed on an incoming message. Use the extension method `GetReplyAddress` on `IncomingMessage` to acquire the reply address.


## MessageIntent

The `MessageIntent` can only be accessed on an incoming message. Use the extension method `GetMessageIntent` on `IncomingMessage` to acquire the message intent.


## TimeToBeReceived

From the perspective of an outgoing message the `TimeToBeReceived` is a delivery concern and needs to be specified over the newly introduced `DeliveryConstraint`.

Set the `TimeToBeReceived`

snippet: SetDeliveryConstraintDiscardIfNotReceivedBefore

Read the `TimeToBeReceived`

snippet: ReadDeliveryConstraintDiscardIfNotReceivedBefore

From the perspective of an incoming message the `TimeToBeReceived` can be acquired by using the `Headers.TimeToBeReceived` on the `IncomingMessage.Headers` dictionary.


## Recoverable

From the perspective of an outgoing message the `Recoverable` flag is a delivery concern and needs to be specified over the newly introduced `DeliveryConstraint`.

Set the `Recoverable`

snippet: SetDeliveryConstraintNonDurable

Read the `Recoverable`

snippet: ReadDeliveryConstraintNonDurable

From the perspective of an incoming message, the `Recoverable` flag can be acquired by using the `Headers.NonDurableMessage` on the `IncomingMessage.Headers` dictionary.


## Raw sending

In Version 5 it was possible  to use `ISendMessages` to do raw sends. In Version 6 `IDispatchMessages` was introduced. See the following snippet how raw sending could look like

snippet: DispatcherRawSending