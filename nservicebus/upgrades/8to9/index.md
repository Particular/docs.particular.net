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

## Removed support for .NET Framework

NServiceBus 9 no longer supports any version of the .NET Framework. Instead, it targets .NET 8 only (Read more about the [supported frameworks and platforms](/nservicebus/upgrades/supported-platforms.md)). Any component in NServiceBus 8 that is .NET Framework only (for example, the MSMQ transport)will not have a version that is compatible with NServiceBus 9. NServiceBus 8 will continue to be supported for use on the .NET Framework.

## SendOptions immediate dispatch changes

The method used to determine if [immediate dispatch](/nservicebus/messaging/send-a-message.md#dispatching-a-message-immediately) has been requested for a message has been renamed.

snippet: core-8to9-immediate-dispatch

## IManageUnitsOfWork has been removed

The `IManageUnitsOfWork` API has been removed. Instead, a [pipeline behavior should be used to implement custom units of work](/nservicebus/pipeline/unit-of-work.md#implementing-custom-unit-of-work).

snippet: core-8to9-uow

## API to override machine name has changed

The `RuntimeEnvironment.MachineNameAction` API [Override the machine name](/nservicebus/hosting/override-machine-name.md) has been removed. The replacement API is the `HostInfoSettings.UsingHostName` method.

snippet: core-8to9-machinename

## API to set additional audit metadata has changed

The API to [set additional audit metadata](/nservicebus/operations/auditing.md#additional-audit-information) has been changed.

snippet: core-8to9-audit-metadata

## Dependency registration access in features renamed

The property used to access container registrations in features has been renamed from `Container` to `Services`.

snippet: core-8to9-di-features

## Service collection extensions for backwards compatibility removed

Service collection extensions to ease [the transition to Microsoft DI abstractions](/nservicebus/upgrades/7to8/dependency-injection.md#registercomponents-changes) have been removed. It is now required to use the registration APIs added in NServiceBus 8.

snippet: core-8to9-di-shims

## Making features depend on message driven subscriptions

The API to make features depend on [message driven subscriptions](/nservicebus/messaging/publish-subscribe/#mechanics-message-driven-persistence-based) being in use has changed.

snippet: core-8to9-depend-on-subscriptions

## Endpoint addresses

### Access to logical addresses in features

Since endpoint addresses are translated to transport-specific ones later during endpoint startup, addresses are defined using a transport-agnostic `QueueAddress` type. The addresses can be accessed via the `FeatureConfigurationContext`:

snippet: core-8to9-adresses-features

### Access to the physical receive addresses at runtime

Inject the `ReceiveAddresses` type to access the endpoint receive addresses at runtime.

snippet: core-8to9-adresses-runtime

### Dynamic address translation

Inject the new `ITransportAddressResolver` to translate `QueueAddress`'s to a physical transport addresses at runtime.

snippet: core-8to9-adresses-translation

## Restricted type argument for .UsePersistence<T>

https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eL854

Proposed action: Exclude since it doesn't impact users

## Exceptions no longer marked as serializable

https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eL739

Proposed action: Exclude since it doesn't impact users

## Extensionpoint for non task based notifications has been deprecated

https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eR250 (TBD remove)

Propose action: Don't mention this since the only use case was already deprecated in v8, see /nservicebus/upgrades/7to8/#error-notification-events and no downstream is using this API (there could be end users extending this though but that feels very unlikely?)

## Audit context no longer accept time to be received

https://github.com/Particular/NServiceBus/compare/release-8.1...master#diff-2c08aef8335f8f17ba3dc362fe939f8a5bdddde4f411d06a067882ac204fa43eR79 (TBD remove)

public static NServiceBus.Pipeline.IAuditContext CreateAuditContext(this NServiceBus.Pipeline.ForkConnector<NServiceBus.Pipeline.IIncomingPhysicalMessageContext, NServiceBus.Pipeline.IAuditContext> forkConnector, NServiceBus.Transport.OutgoingMessage message, string auditAddress, NServiceBus.Pipeline.IIncomingPhysicalMessageContext sourceContext) { }
public static NServiceBus.Pipeline.IAuditContext CreateAuditContext(this NServiceBus.Pipeline.ForkConnector<NServiceBus.Pipeline.IIncomingPhysicalMessageContext, NServiceBus.Pipeline.IAuditContext> forkConnector, NServiceBus.Transport.OutgoingMessage message, string auditAddress, System.TimeSpan? timeToBeReceived, NServiceBus.Pipeline.IIncomingPhysicalMessageContext sourceContext) { }

Proposed action: Do not mention since this api is only for internal use