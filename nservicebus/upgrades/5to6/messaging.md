---
title: Messaging Changes in NServiceBus Version 6
reviewed: 2020-05-11
component: Core
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---


## Using a custom correlation id

In NServiceBus version 6, custom [correlation IDs](/nservicebus/messaging/headers.md#messaging-interaction-headers-nservicebus-correlationid) for outgoing messages should be set using the new `Send`/`Reply` or `Publish` options instead of being passed into `bus.Send`.


## WinIdName header removed

The `WinIdName` existed to enable the Principal Replacement feature (`RunHandlersUnderIncomingPrincipal` in NServiceBus version 4 and `ImpersonateSender` in version 3).

See the [appending username using headers](/samples/username-header/) sample for usage of this API.

This feature was removed in version 5 and the `WinIdName` header will no longer be added to outgoing messages.

To re-add this header to outgoing messages a [mutator](/nservicebus/pipeline/message-mutators.md) can be used.

snippet: 5to6ReAddWinIdNameHeader

Another option is to use a custom header as illustrated in the [appending username using headers](/samples/username-header/) sample.


## Throttling

Requirements for throttling mechanisms are very different. While some third-party services (e.g. GitHub, Twitter, Google) enforce rate limits on certain time periods, other services may have entirely different usage limitations. The throttling API in NServiceBus version 5 offers a limited, messages-per-second-based throttling mechanism which works for very few scenarios. Therefore, the throttling API has been removed with version 6 without a built-in alternative.

The `MaximumMessageThroughputPerSecond` on the `TransportConfig` class has been marked as obsolete. Using a configuration-based approach, the endpoint will fail to start when using the `MaximumMessageThroughputPerSecond` attribute on the `<TransportConfig>` element.


## Immediate dispatch

Using a suppressed transaction scope to request sends to be dispatched immediately is still supported. However it is recommended to switch to the new explicit API for [immediate dispatch](/nservicebus/messaging/send-a-message.md#dispatching-a-message-immediately).


## Batched dispatch

NServiceBus version 6 introduced the concept of [batched dispatch](/nservicebus/messaging/batched-dispatch.md) which means that outgoing operations won't dispatch to the transport until all the handlers of the current message have completed successfully. This helps reduce inconsistencies, in the form of "ghost" messages being emitted due to exceptions during processing.


## Deprecated address

NServiceBus version 5 represents addresses with an `Address` class. The `Address` class maintains addresses in the *queue@host* format. This format was originally developed for the MSMQ transport but does not meet the needs of other transports. In version 6, addresses are represented as opaque strings.

Any usages of `Address` should be replaced by `string`.


## Native sends via MSMQ

`MsmqMessageSender` and `MsmqSettings` are no longer available. Refer to [native sends](/transports/msmq/operations-scripting.md#native-send) for other ways of sending raw messages via MSMQ.


## Delayed delivery

With the deprecation of `IBus`, message delivery can no longer be delayed with `bus.Defer()`. To delay a message, use the `DelayDeliveryWith(TimeSpan)` and `DoNotDeliverBefore(DateTime)` methods on `SendOptions` passed into `Send()`.

snippet: 5to6delayed-delivery


## Message forwarding

Forwarded messages no longer contain additional [auditing headers](/nservicebus/operations/auditing.md#message-headers), such as processing start and end times, processing host id and name, and processing endpoint.


## Audit InvokedSagas header

In NServiceBus version 5, the `InvokedSagas` header is added to audited messages and populated with the name of the saga classes invoked along with their unique identifiers.

This functionality has been moved from the NServiceBus core to the [SagaAudit plugin](/servicecontrol/plugins/saga-audit.md) compatible with version 6.
