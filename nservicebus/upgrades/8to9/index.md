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

## Service collection extensions for backward compatibility removed

Service collection extensions to ease [the transition to Microsoft DI abstractions](/nservicebus/upgrades/7to8/dependency-injection.md#registercomponents-changes) have been removed. It is now required to use the registration APIs added in NServiceBus 8.

snippet: core-8to9-di-shims

## Endpoint addresses

In NServiceBus version 8 and earlier, the local transport-specific queue addresses are accessible via the `settings.LocalAddress()` and `settings.InstanceSpecificQueue()` settings extension methods. These extension methods have been replaced with a variety of new APIs, depending on the scenario of where the addresses are needed.

### Accessing logical addresses in features

Since endpoint addresses are translated to transport-specific ones later during endpoint startup, addresses are defined using a transport-agnostic `QueueAddress` type. The addresses can be accessed via the `FeatureConfigurationContext`:

snippet: core-8to9-addresses-features

### Accessing the endpoint's receive addresses

Inject the `ReceiveAddresses` type to access the endpoint's receive addresses.

snippet: core-8to9-receive-addresses

### Dynamic address translation

Use the new `ITransportAddressResolver` API to translate a `QueueAddress` to a transport-specific address at runtime.

snippet: core-8to9-address-translation

## Extensibility

This section describes changes to advanced extensibility API's.

### Subscription storages

The API to make features depend on [message-driven subscriptions](/nservicebus/messaging/publish-subscribe/#mechanics-message-driven-persistence-based) when implementing custom [subscription storages](/persistence/) has changed:

snippet: core-8to9-depend-on-subscriptions

### The extension point for non-task-based notifications has been removed

Instead, task-based notifications should be used, see the [new error notification events](/nservicebus/upgrades/7to8/#error-notification-events) for more details.
