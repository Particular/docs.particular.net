---
title: NHibernate Persistence Upgrade Version 8 to 9
summary: Instructions on how to upgrade NHibernate Persistence Version 8 to 9.
reviewed: 2020-02-11
component: NHibernate
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
 - 8
---

Starting with NServiceBus.NHibernate version 8.3.0, the built-in gateway API has been obsoleted with the following message:

> NHibernate gateway persistence is deprecated. Use the new NServiceBus.Gateway.Sql dedicated package. Will be treated as an error from version 9.0.0. Will be removed in version 10.0.0.

To migrate to the new gateway API:

- Add a reference to the `NServiceBus.Gateway.Sql` NuGet package and configure the gateway feature following the instructions available in the [SQL Gateway Storage documentation](/nservicebus/gateway/sql/).
- Remove any configuration that uses the legacy gateway API.
