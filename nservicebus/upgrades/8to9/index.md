---
title: Upgrade Version 8 to 9
summary: Instructions on how to upgrade NServiceBus from version 8 to version 9.
reviewed: 2023-12-13
component: Core
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 8
 - 9
---

## SendOptions changes

In NServiceBus version 8 and earlier, the indication that a message was marked for [immediate dispatch](/nservicebus/messaging/send-a-message.md#dispatching-a-message-immediately) was provided by `SendOptions.RequiredImmediateDispatch()`. In version 9, this method has been renamed to `SendOptions.IsImmediateDispatchSet()`.

## DataBus interface changes

Starting with NServiceBus version 8, it is mandatory to provide a serializer to the data bus configuration API. The `BinaryFormatterDataBusSerializer` has been removed and `SystemJsonDataBusSerializer` is the default option that's built-in.

## TODO

- https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eL520 (TBD remove)
- https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eR487 (TBD remove)
- https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eL739 (TBD remove)
- https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eR606 (TBD remove)
- https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eR854 (TBD remove)
- https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eR885 (TBD remove)
- https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eR887 (TBD remove)
- https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eR1211 (TBD remove)
- https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eR1238 (TBD remove)
- https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eR1460 (TBD remove)
- https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eR1574 (TBD remove)
- https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eR2070 (TBD remove)
- https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eR2356 (TBD remove)

## Extensionpoint for non task based notifications has been deprecated

https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eR250 (TBD remove)

Propose action: Don't mention this since the only use case was already deprecated in v8, see https://docs.particular.net/nservicebus/upgrades/7to8/#error-notification-events and we haven't found any downstreams using this API (there could be end users extending this though but that feels very unlikely?)

## Databus property is no longer marked as serializable

`[System.Serializable]` was removed

https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eR162 (TBD remove)

Proposed action: Do not mention since this api is only for internal use

## Audit context no longer accept time to be received

https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eR79 (TBD remove)

public static NServiceBus.Pipeline.IAuditContext CreateAuditContext(this NServiceBus.Pipeline.ForkConnector<NServiceBus.Pipeline.IIncomingPhysicalMessageContext, NServiceBus.Pipeline.IAuditContext> forkConnector, NServiceBus.Transport.OutgoingMessage message, string auditAddress, NServiceBus.Pipeline.IIncomingPhysicalMessageContext sourceContext) { }
public static NServiceBus.Pipeline.IAuditContext CreateAuditContext(this NServiceBus.Pipeline.ForkConnector<NServiceBus.Pipeline.IIncomingPhysicalMessageContext, NServiceBus.Pipeline.IAuditContext> forkConnector, NServiceBus.Transport.OutgoingMessage message, string auditAddress, System.TimeSpan? timeToBeReceived, NServiceBus.Pipeline.IIncomingPhysicalMessageContext sourceContext) { }

Proposed action: Do not mention since this api is only for internal use