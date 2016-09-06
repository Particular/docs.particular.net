---
title: Messaging changes in Version 6
tags:
 - upgrade
 - migration
---


## Using a custom correlation id

Custom [correlation Id's](/nservicebus/messaging/message-correlation.md) for outgoing messages should now be set using the new `Send`/`Reply` or `Publish` options instead of being passed into `bus.Send`.


## Remove WinIdName Header

The `WinIdName` existed to enable the Principal Replacement feature (`RunHandlersUnderIncomingPrincipal` in Version 4 and `ImpersonateSender` in Version 3).

See the [Appending username using headers](/samples/username-header/) sample for usage of this API.

This feature was removed in Version 5 and the `WinIdName` header will no longer be added to outgoing messages.

To re-add this header to outgoing messages a [mutator](/nservicebus/pipeline/message-mutators.md) can be used.

snippet: 5to6ReAddWinIdNameHeader

Another option is to use a custom header as illustrated in [Appending username using headers](/samples/username-header/) sample.


## Throttling

Requirements to throttling mechanisms are very different. While some 3rd party services (e.g. GitHub, Twitter, Google, etc.) enforce rate limits on certain time periods, other services may have entirely different usage limitations. The previous throttling API offered a very limited, messages per second based, throttling mechanism which only works for very few scenarios. Therefore, the throttling API has been removed with Version 6 without a built-in alternative. [Tuning NServiceBus](/nservicebus/operations/tuning.md) contains more information about implementing a custom throttling mechanism.

The `MaximumMessageThroughputPerSecond` on the `TransportConfig` class has been marked as obsolete. Using a configuration based approach, the endpoint will fail to start when using the `MaximumMessageThroughputPerSecond` attribute on the `<TransportConfig>` element.


## Immediate dispatch

Using a suppressed transaction scope to request sends to be dispatched immediately is still supported. However it is recommend to switch to the new explicit API for [immediate dispatch](/nservicebus/messaging/send-a-message.md#immediate-dispatch).


## Batched dispatch

Version 6 introduced the concept of [Batched dispatch](/nservicebus/messaging/batched-dispatch.md) which means that outgoing operations won't dispatch to the transport until all the handlers of the current message have completed successfully. This helps users inconsistencies in the form of "ghost" messages being emitted due to exceptions during processing.


## Deprecated Address

Version 5 of NServiceBus represents addresses with an `Address` class. The `Address` class maintains addresses in the *queue@host* format. This format was originally developed for the MSMQ transport but does not meet the needs of other transports. In Version 6, addresses are represented as opaque strings.

Any usages of `Address` should be replaced by `string`.


## Native sends via MSMQ

`MsmqMessageSender` and `MsmqSettings` are no longer available. Refer to [native sends](/nservicebus/msmq/operations-scripting.md#native-send) for other ways of sending raw messages via MSMQ.


## Delayed Delivery

With the deprecation of `IBus`, message delivery can no longer be delayed with `bus.Defer()`. To delay a message, use the `DelayDeliveryWith(TimeSpan)` and `DoNotDeliverBefore(DateTime)` methods on `SendOptions` passed into `Send()`.

snippet: 5to6delayed-delivery


## Message forwarding

The forwarded messages no longer contain additional [auditing headers](/nservicebus/operations/auditing.md#message-headers), such as processing start and end times, processing host id and name and processing endpoint.