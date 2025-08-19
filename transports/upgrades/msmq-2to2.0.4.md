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
> If the endpoint being upgraded is not using delayed delivery or is using the `SqlServerDelayedMessageStore` that ships with the MSMQ transport package, no further action is required.

Versions 2.0.0-2.0.3 of the transport include support for a custom delayed delivery message store which implements `IDelayedMessageStore`. This interface contains an `Initialize()` method which may be used to initialize the store when the endpoint starts. `Initialize()` is called when two conditions are satisfied:

- the endpoint is configured to run installers
- the transport is configured to create queues

Version 2.0.4 preserves this behavior for backwards compatibility. If at least one of these conditions is not satisfied, `Initialize()` is not called, which may cause problems in a minimal-access environment.

To ensure an endpoint can run in a minimal access environment, a new interface named `IDelayedMessageStoreWithInfrastructure` has been added. This new interface extends `IDelayedMessageStore` with a `SetupInfrastructure()` method.

If a custom delayed delivery message store implements `IDelayedMessageStoreWithInfrastructure`, `Initialize()` is always called when the endpoint starts and `SetupInfrastructure()` is only called if both the above conditions are satisfied.

If a custom delayed delivery message store is used in an endpoint which satisfies both the above conditions, no further action is required.

If a custom delayed delivery message store is used in an endpoint where at least one of the above conditions is not satisfied, these actions must be taken:

1. Update the reference to the transport package to the latest version 2 release
2. Change the custom delayed delivery message store class to implement `IDelayedMessageStoreWithInfrastructure` instead of `IDelayedMessageStore`.
3. Move any code that creates infrastructure from `Initialize()` to `SetupInfrastructure()`.
   - `Initialize()` is always called first. If `SetupInfrastructure()` requires any references created by `Initialize()`, they may be assigned to private fields on the class.
