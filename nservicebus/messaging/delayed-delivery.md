---
title: Delayed Delivery
summary: Delay delivery of messages until a later time.
tags:
- Defer
---

When a message is sent it does not need to be dispatched immediately. You can specify a later time for the message to be delivered using `bus.Defer(...)` instead of `bus.Send(...)`.

NOTE: Only send operations can be deferred. You cannot defer a publish or a reply. 

## Using a TimeSpan

Delays delivery of a message for a specified duration.

snippet:delayed-delivery-timespan

## Using a DateTime

Delays delivery of a message until a specified point in time.

snippet:delayed-delivery-datetime

## Caveats

In order to use `bus.Defer(...)` the [Timeout Manager feature](/nservicebus/persistence/index.md) must be enabled. When you call `bus.Defer(...)` a message is sent to the timeout manager requesting it to deliver the message at a later time. 

NOTE: The sending endpoint must be running when the timeout is reached in order for the message to be sent. If the endpoint is not running when the timeout is reached then the message will be sent when the endpoint is next started. 

NOTE: If you specify a negative timeout or a time that is in the past then the message will still be slightly delayed. The message will not be sent until the timeout manager has processed the request.