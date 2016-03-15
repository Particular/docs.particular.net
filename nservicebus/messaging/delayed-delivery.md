---
title: Delayed Delivery
summary: Delay delivery of messages until a later time.
tags:
- Defer
---
The message doesn't have to be dispatched immediately after sending, it can be delivered at a later time if necessary. In Version 5 and below this can be achieved by using `bus.Defer(...)` instead of `bus.Send(...)`. In Version 6, this is accomplished by calling methods on an instance of `SendOptions` that is passed into `Send(...)` calls.  

NOTE: Only send operations can be deferred. You cannot defer a publish or a reply. 

## Delaying message dispatching
### Version 5 and below: Defer message handling
Calling `bus.Defer(...)` the handling of the message is deferred by certain amount of time. After that time the message is passed to be handled by the same endpoint. Semantically `bus.Defer(...)` is equivalent to calling `bus.SendLocal(...)` after time condition is met. Read more about [sending local](/nservicebus/messaging/send-a-message.md#sending-to-self)  

Using this mechanism delayed delivery can be achieved by calling `bus.Defer(...)` which can be seen in [Delayed Delivery Sample](/samples/delayed-delivery).

### Version 6 and above: Delay message delivery
Different approach was taken to extend `SendOptions` by adding `DelayDeliveryWith` method. This allows to defer the actual sending of a message to any endpoint. The behavior of delayed handling using `DelayDeliveryWith` can be seen in [Delayed Delivery Sample](/samples/delayed-delivery). 

## Using a TimeSpan

Delays delivery of a message for a specified duration.

snippet:delayed-delivery-timespan

## Using a DateTime

Delays delivery of a message until a specified point in time.

snippet:delayed-delivery-datetime

## Caveats

In order to use message deferral the Timeout Manager feature must be enabled. When you defer a message, it is sent to the timeout manager requesting it to deliver the message at a later time. 

snippet:configure-timeout-manager

NOTE: In Version 4, 5 and 6 Timeout Manager is enabled by default.

NOTE: The sending endpoint must be running when the timeout is reached in order for the message to be sent. If the endpoint is not running when the timeout is reached then the message will be sent when the endpoint is next started. 

NOTE: If you specify a negative timeout or a time that is in the past then the message will still be slightly delayed. The message will not be sent until the timeout manager has processed the request.

## Persistance
Delayed messages are persisted (stored in a durable storage) in a location specified for `Timeouts`. The messages will be stored for the specified delay time. Sample persistence configuration for Timeouts can be seen below. 

snippet:configure-persistence-timeout

To learn more about persistence refer to [Persistence In NServiceBus](/nservicebus/persistence/) article.