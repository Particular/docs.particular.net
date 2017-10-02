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

partial: handlecurrentmessagelater


## Delaying message dispatching

partial: intro

Note: Similar to `SendLocal`, `Defer` will also change the message's reply-to address to the endpoint deferring the message. Calling `Reply` on a deferred message will send the reply to itself.


## Using a TimeSpan

Delays delivery of a message for a specified duration.

snippet: delayed-delivery-timespan


## Using a DateTime

Delays delivery of a message until a specified point in time.

snippet: delayed-delivery-datetime


## Caveats

Delayed delivery of messages is supported when one of the following requirements are met:

 * The transport supports delayed delivery natively.
 * The Timeout Manager feature is enabled and the endpoint is not a send-only endpoint.

When deferring a message, it is sent to the Timeout Manager requesting it to deliver the message at a later time or deferred by using the transports native capability to defer messages.

partial: caveatsnote

NOTE: When relying on Timeout Manager, the sending endpoint must be running when the timeout is reached in order for the message to be sent. If the endpoint is not running when the timeout is reached then the message will be sent when the endpoint is next started.

NOTE: If specifying a negative timeout or a time that is in the past then the message will still be slightly delayed. The message will not be sent until the Timeout Manager has processed the request.


## How it works

NServiceBus provides delayed deliver feature for transports that don't have native support for delayed message delivery, i.e. for MSMQ and SQL Server transports. Transports that support delayed message delivery natively don't require persistence. To learn more about NServiceBus delayed message store refer to the [Timeout Manager](/nservicebus/messaging/timeout-manager.md) article.
