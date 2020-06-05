---
title: RavenDB Persistence Upgrade from 6.2 to 6.3
summary: Instructions on how to upgrade NServiceBus.RavenDB 6.2 to 6.3
component: Raven
related:
 - nservicebus/upgrades/7to8
reviewed: 2020-05-25
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---

Starting with NServiceBus.RavenDB version 6.3.0, outbox-related extension methods for `EndpointConfiguration` are obsolete. Using them will produce the following messages

> 'RavenDBOutboxExtensions.SetTimeToKeepDeduplicationData(EndpointConfiguration, TimeSpan)' is obsolete: 'Use `SetTimeToKeepDeduplicationData` available on the `OutboxSettings` instead. Will be removed in version 7.0.0.'

> 'RavenDBOutboxExtensions.SetFrequencyToRunDeduplicationDataCleanup(EndpointConfiguration, TimeSpan)' is obsolete: 'Use `SetFrequencyToRunDeduplicationDataCleanup` available on the `OutboxSettings` instead. Will be removed in version 7.0.0.'

To migrate outbox settings, use:

snippet: OutboxSettingsUpgrade

NOTE: Starting with NServiceBus.RavenDB version 6.3, it is recommended to disable cleanup and rely on [document expiration](https://ravendb.net/docs/article-page/latest/csharp/server/extensions/expiration) instead. For more information, refer to the [outbox cleanup guidance](/persistence/ravendb/outbox.md?version=raven_6.3#deduplication-record-lifespan).