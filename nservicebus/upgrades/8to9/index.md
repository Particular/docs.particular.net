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

## .NET Framework no longer supported

TBD

## SendOptions changes

https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eR487 (TBD remove)

The indication that a message was marked for [immediate dispatch](/nservicebus/messaging/send-a-message.md#dispatching-a-message-immediately) has been renamed, instead of:

snippet: core-8to9-immediate-dispatch-old

Use:

snippet: core-8to9-immediate-dispatch-new

## IManageUnitsOfWork has been deprecated

https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eR2356 (TBD remove)

The `IManageUnitsOfWork` API has been deprecated in favour of [using a pipeline behavior to implement custom units of work](/nservicebus/pipeline/unit-of-work.md#implementing-custom-unit-of-work), instead of:

snippet: core-8to9-uow-old

Use:

snippet: core-8to9-uow-new

## API to override machine name has changed

https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eR2070 (TBD remove)

[Overriding machine name](/nservicebus/hosting/override-machine-name.md) via NServiceBus.Support.RuntimeEnvironment has been deprecated, instead of:

snippet: core-8to9-machinename-old

Use:

snippet: core-8to9-machinename-new

## API to set additional Audit metadata has changed

 https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eR1574 (TBD remove)

The API to [set additional audit metadata](/nservicebus/operations/auditing.md#additional-audit-information) has been changed, instead of:

snippet: core-8to9-audit-metadata-old

Use:

snippet: core-8to9-audit-metadata-new

## Access to transport addresses via settings has been removed

- https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eR885 (TBD remove)
- https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eR887 (TBD remove)

Transport addresses are now accessed via the `FeatureConfigurationContext`, instead of:

snippet: core-8to9-audit-transportadresses-features-old

Use:

snippet: core-8to9-audit-transportadresses-features-new

## Dependency registration access in features renamed

https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eR1212

Access to the container registrations has been renamed, instead of:

snippet: core-8to9-di-features-old

Use:

snippet: core-8to9-di-features-new

## Service collection extensions for backwards compatibility removed

https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eL1099

Proposed action: document needed code changes and link to /upgrades/7to8/dependency-injection#registercomponents-changes

## MessageDrivenSubscriptions feature deprecated

https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eR1238 (TBD remove)

Use .DisablePublishing instead

Proposed action: document needed code changes

## Restricted type argument for .UsePersistence<T>

https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eL854

Proposed action: Exclude since it doesn't impact users

## Exceptions no longer marked as serializable

https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eL739

Proposed action: Exclude since it doesn't impact users

## Message interfaces moved to a separate package

https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eL520 (TBD remove)

Proposed action: Write something

## Extensionpoint for non task based notifications has been deprecated

https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eR250 (TBD remove)

Propose action: Don't mention this since the only use case was already deprecated in v8, see /nservicebus/upgrades/7to8/#error-notification-events and no downstream is using this API (there could be end users extending this though but that feels very unlikely?)

## Databus property is no longer marked as serializable

`[System.Serializable]` was removed

https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eR162 (TBD remove)

Proposed action: Do not mention since this api is only for internal use

## Audit context no longer accept time to be received

https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eR79 (TBD remove)

public static NServiceBus.Pipeline.IAuditContext CreateAuditContext(this NServiceBus.Pipeline.ForkConnector<NServiceBus.Pipeline.IIncomingPhysicalMessageContext, NServiceBus.Pipeline.IAuditContext> forkConnector, NServiceBus.Transport.OutgoingMessage message, string auditAddress, NServiceBus.Pipeline.IIncomingPhysicalMessageContext sourceContext) { }
public static NServiceBus.Pipeline.IAuditContext CreateAuditContext(this NServiceBus.Pipeline.ForkConnector<NServiceBus.Pipeline.IIncomingPhysicalMessageContext, NServiceBus.Pipeline.IAuditContext> forkConnector, NServiceBus.Transport.OutgoingMessage message, string auditAddress, System.TimeSpan? timeToBeReceived, NServiceBus.Pipeline.IIncomingPhysicalMessageContext sourceContext) { }

Proposed action: Do not mention since this api is only for internal use
