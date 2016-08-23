---
title: Delayed Delivery
summary: Delay delivery of messages until a later time.
component: core
reviewed: 2016-08-22
tags:
- Defer
related:
- samples/delayed-delivery
---

The message doesn't have to be dispatched immediately after sending, it can be delivered at a later time if necessary.

NOTE: Only send operations can be deferred. Publish and reply operations cannot be deferred.


## Delaying message dispatching

partial:intro

Note: Similar to `SendLocal`, `Defer` will also change the message's reply-to address to the endpoint deferring the message. Calling `Reply` on a deferred message will send the reply to itself.

## Using a TimeSpan

Delays delivery of a message for a specified duration.

snippet:delayed-delivery-timespan


## Using a DateTime

Delays delivery of a message until a specified point in time.

snippet:delayed-delivery-datetime


## Caveats

Delayed delivery of messages is only supported when the following requirements are met:

* The Timeout Manager feature is enabled or
* The transport supports delayed delivery natively
* The transport supports transactions
* The endpoint is not a send-only endpoint.

When deferring a message, it is sent to the timeout manager requesting it to deliver the message at a later time or deferred by using the transports native capability to defer messages.

partial:caveatsnote

NOTE: The sending endpoint must be running when the timeout is reached in order for the message to be sent. If the endpoint is not running when the timeout is reached then the message will be sent when the endpoint is next started.

NOTE: If specifying a negative timeout or a time that is in the past then the message will still be slightly delayed. The message will not be sent until the timeout manager has processed the request.


## How it works

Some transports support delayed message delivery natively and don't require using persistence. NServiceBus provides this feature for those that don't offer native support for delayed message delivery, i.e. for MSMQ and SQL Server transports. Using NServiceBus implementation requires using persistence.

The timeout data is stored in three different locations at various stages of processing: `[endpoint_queue_name].Timeouts` queue, timeouts storage location specific for the chosen persistence (e.g. dedicated table or document type) and `[endpoint_queue_name].TimeoutsDipatcher` queue.

After the `RequestTimeout<T>` method is called, NServiceBus endpoint sends the timeout message to the `[endpoint_queue_name].Timeouts` queue. That queue is monitored by NServiceBus internal receiver. It picks up timeout messages and stores them using the selected NServiceBus persistence. NHibernate persistence stores timeout messages in a table called `TimeoutEntity`, RavenDB persistence stores them as documents of a type `TimeoutData`.

Delayed messages are persisted (stored in a durable storage) in a location specified for `Timeouts`. The messages will be stored for the specified delay time. Sample persistence configuration for Timeouts can be seen below.

snippet:configure-persistence-timeout

NServiceBus periodically retrieves expiring timeouts from persistence. When a timeout expires, then a message with that timeout ID is sent to `[endpoint_queue_name].TimeoutsDipatcher` queue. That queue is monitored by NServiceBus internal receiver. When it picks up a message, it looks up the corresponding timeout in the storage. If it finds it, it dispatches the timeout message to the destination queue. 
