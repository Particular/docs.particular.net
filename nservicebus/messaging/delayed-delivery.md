---
title: Delayed Delivery
summary: Delay delivery of messages until a later time.
component: core
reviewed: 2020-07-22
related:
 - samples/delayed-delivery
---

Messages don't need to be dispatched immediately. Delayed delivery is a feature that sends messages into the future to be delivered and processed at a later time.

Delayed delivery is used for:

* [Timeout messages](/nservicebus/sagas/timeouts.md) sent by [sagas](/nservicebus/sagas/)
* [Delayed retries](/nservicebus/recoverability/#delayed-retries), to retry a message after successive delays when [immediate retries](/nservicebus/recoverability/#immediate-retries) don't result in successful processing
* Explicitly sending a message with a delay, as described below

NOTE: Only send operations can be deferred. Publish and reply operations cannot be deferred.

partial: handlecurrentmessagelater


## Delaying message dispatching

partial: intro


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

NOTE: If specifying a time that is in the past then the message will still be slightly delayed. The message will not be sent until the Timeout Manager has processed the request.


## How it works

NServiceBus provides delayed delivery feature for transports that don't have native support for delayed message delivery. All Transports except MSMQ support delayed message delivery natively and therefore don't require persistence to store timeouts. To learn more about NServiceBus delayed message store refer to the [Timeout Manager](/nservicebus/messaging/timeout-manager.md) article.