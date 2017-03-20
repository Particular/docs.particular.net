---
title: Delayed Delivery
summary: Delay delivery of messages until a later time.
component: core
reviewed: 2016-12-05
tags:
 - Defer
related:
 - samples/delayed-delivery
---

The message doesn't have to be dispatched immediately after sending, it can be delivered at a later time if necessary.

NOTE: Only send operations can be deferred. Publish and reply operations cannot be deferred.


## Delaying messages using HandleCurrentMessageLater

The `HandleCurrentMessageLater()` method was primarily used to defer messages when using Versions 2.x and below, before the Defer functionality was introduced in Versions 3 and above. While this API is still supported in Versions 6 and below, there are significant caveats in using this API.

 * Calling this method would create a copy of the message that has the same identifier, header, and body. This message would then be put at the end of the queue. The endpoint will eventually pick up this message once all the other messages in its queue have been processed. To make this work, the message pipeline will not abort which means any business transaction that's part of calling this method will also get committed.
 * If the endpoint's queue is empty, or the condition to put the message back into the queue is incorrect, the message goes back into the queue immediately causing the endpoint to process the same message without any delay. This behavior can cause an endless loop which will manifest itself as a high system resource utilization by the endpoint.

WARNING: This method will be deprecated in Version 7.0. It is recommended to use either [Delayed Retries](/nservicebus/recoverability/#delayed-retries) or one of the deferring mechanisms below, depending on the scenario.


## Delaying message dispatching

partial:intro

Note: Similar to `SendLocal`, `Defer` will also change the message's reply-to address to the endpoint deferring the message. Calling `Reply` on a deferred message will send the reply to itself.


## Using a TimeSpan

Delays delivery of a message for a specified duration.

snippet: delayed-delivery-timespan


## Using a DateTime

Delays delivery of a message until a specified point in time.

snippet: delayed-delivery-datetime


## Caveats

Delayed delivery of messages is supported when the one following requirements are met:

 * The transport supports delayed delivery natively.
 * The Timeout Manager feature is enabled and the endpoint is not a send-only endpoint.

When deferring a message, it is sent to the Timeout Manager requesting it to deliver the message at a later time or deferred by using the transports native capability to defer messages.

partial:caveatsnote

NOTE: When relying on Timeout Manager, the sending endpoint must be running when the timeout is reached in order for the message to be sent. If the endpoint is not running when the timeout is reached then the message will be sent when the endpoint is next started.

NOTE: If specifying a negative timeout or a time that is in the past then the message will still be slightly delayed. The message will not be sent until the Timeout Manager has processed the request.


## How it works

NServiceBus provides delayed deliver feature for transports that don't have native support for delayed message delivery, i.e. for MSMQ and SQL Server transports. Transports that support delayed message delivery natively don't require persistence. To learn more about NServiceBus delayed message store refer to the [Timeout Manager](/nservicebus/messaging/timeout-manager.md) article.
