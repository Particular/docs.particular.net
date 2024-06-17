---
title: MSMQ Transport Upgrade Version 2 to 2.0.4
summary: Migration instructions on how to upgrade the MSMQ transport from version 2 to 2.0.4
reviewed: 2024-06-13
component: MsmqTransport
related:
  - transports/msmq
  - nservicebus/upgrades/7to8
isUpgradeGuide: true
upgradeGuideCoreVersions:
  - 7
  - 8
---

> [!NOTE]
> If the endpoint being upgraded is not using delayed delivery or is using the `SqlServerDelayedMessageStore` that ships with the MSMQ transport package, no further action is needed. This class has already been updated as part of the release.

Versions 2.0.0-2.0.3 of the transport include support for a custom delayed delivery message store by implementing `IDelayedMessageStore`. This interface contains a method to initialize the store when the endpoint starts. For this method to be called, two configuration options must be set:

- the endpoint must be configured to run installers
- the transport must be configured to create queues

Version 2.0.4 preserves this behavior for backwards compatability. This can cause a problem in a minimal-access environment when either of the above conditions is not met, as `Initialize()` will not be called.

To enable the endpoint to run in a minimal access environment, a new interface (`IDelayedMessageStoreWithInfrastructure`) has been added. This new interface extends the original `IDelayedMessageStore` interface.

If a custom delayed delivery message store implements `IDelayedMessageStoreWithInfrastructure` then `Initialize()` is always called when the endpoint starts. The new `SetupInfrastructure()` method is only called if the above criteria are met.

If the custom delayed delivery message store is used in an endpoint which meets both criteria above, no action is needed during an upgrade.

If the custom delayed delivery message store is used in an endpoint where one or both of the criteria above are not met, follow this process to update it:

1. Update the reference to the transport package to the latest version 2 release
2. Update the class definition of the custom delayed delivery message store to use `IDelayedMessageStoreWithInfrastructure` (instead of `IDelayedMessageStore`)
3. Move any code that creates infrastructure from the `Initialize()` method to the `SetupInfrastructure()` method.
   - If there is information in the `Initialize()` method which is needed in `SetupInfrastructure()`, it can be stored in fields on the class. `Initialize()` is always called first.
