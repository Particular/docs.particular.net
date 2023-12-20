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

- .NET8 is the only target
- Means that the MSMQ transport is not supported
  - MSMQ is supported for another X years
  - The bridge will support msmq and can be used to migrate endpoints

## SendOptions changes

The indication that [immediate dispatch](/nservicebus/messaging/send-a-message.md#dispatching-a-message-immediately) has been requested for a message has been renamed.

snippet: core-8to9-immediate-dispatch

## IManageUnitsOfWork has been deprecated

The `IManageUnitsOfWork` API has been deprecated in favour of [using a pipeline behavior to implement custom units of work](/nservicebus/pipeline/unit-of-work.md#implementing-custom-unit-of-work).

snippet: core-8to9-uow

## API to override machine name has changed

[Overriding machine name](/nservicebus/hosting/override-machine-name.md) via NServiceBus.Support.RuntimeEnvironment has been deprecated.

snippet: core-8to9-machinename

## API to set additional Audit metadata has changed

The API to [set additional audit metadata](/nservicebus/operations/auditing.md#additional-audit-information) has been changed.

snippet: core-8to9-audit-metadata

## Access to transport addresses via settings has been removed

Transport addresses are now accessed via the `FeatureConfigurationContext`.

snippet: core-8to9-audit-transportadresses-features

## Dependency registration access in features renamed

https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eR1212

Access to the container registrations has been renamed, instead of:

snippet: core-8to9-di-features-old

Use:

snippet: core-8to9-di-features-new

## Service collection extensions for backwards compatibility removed

https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eL1099

Service collection extensions to ease [the transition to Microsoft DI abstraction](/nservicebus/upgrades/7to8/dependency-injection.md#registercomponents-changes) have been removed, instead of:

snippet: core-8to9-di-shims-old

Use:

snippet: core-8to9-di-shims-new

## Message interfaces moved to a separate package

https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eL520 (TBD remove)

Proposed action: Nothing to write from an upgrade guide perspective

## MessageDrivenSubscriptions feature deprecated

https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eR1238 (TBD remove)

Use .DisablePublishing instead

Proposed action: Already documented in /nservicebus/upgrades/7to8/#disabling-subscriptions is more needed?

## Restricted type argument for .UsePersistence<T>

https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eL854

Proposed action: Exclude since it doesn't impact users

## Exceptions no longer marked as serializable

https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eL739

Proposed action: Exclude since it doesn't impact users

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
