---
title: RavenDB Persistence Upgrade from 6 to 7
summary: Instructions on how to upgrade NServiceBus.RavenDB 6 to 7
component: Raven
related:
 - nservicebus/upgrades/7to8
reviewed: 2020-02-06
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 8
---

Starting with `NServiceBus.RavenDB` V6.2.0 the built-in gateway API has been obsoleted with the following message:

> RavenDB gateway persistence has been moved to the NServiceBus.Gateway.RavenDB dedicated package. Will be treated as an error from version 7.0.0. Will be removed in version 8.0.0.

To migrate to the new gateway API:

- Add a reference to the `NServiceBus.Gateway.RavenDB` Nuget package and configure the gateway feature following the instructions available in the [RavenDB Gateway Storage documenation](/nservicebus/gateway/ravendb/).
- Remove any configuration that uses the legacy gateway API.
