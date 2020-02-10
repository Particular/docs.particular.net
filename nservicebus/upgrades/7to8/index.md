---
title: Upgrade Version 7 to 8
summary: Instructions on how to upgrade NServiceBus from version 7 to version 8.
reviewed: 2020-02-20
component: Core
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
 - 8
---

NOTE: This is a working document, as there is currently no plan or timeline for the release of NServiceBus version 8.0.

## New gateway persistence API

The NServiceBus Gateway has been moved to a separate `NServiceBus.Gateway` package and all Gateway public API in NServiceBus has been obsoleted with the following warning:

> Gateway persistence has been moved to the NServiceBus.Gateway dedicated package. Will be treated as an error from version 8.0.0. Will be removed in version 9.0.0.

### How to upgrade

- Select the storage type to use in production. Supported storage types are [Microsoft SQL Server](/nservicebus/gateway/sql/) and [RavenDB](/nservicebus/gateway/ravendb/).
- Depending on the selected storage type, add a reference to the respective NuGet package:
  - [NServiceBus.Gateway.Sql](https://www.nuget.org/packages/NServiceBus.Gateway.Sql)
  - [NServiceBus.Gateway.RavenDB](https://www.nuget.org/packages/NServiceBus.Gateway.RavenDB)
- Configure the Gateway API by invoking the `Gateway()` method on the endpoints configuration instance passing as an argument the selected storage configuration instance.

Refer to the [NServiceBus.Gateway.Sql](/nservicebus/gateway/sql) or [NServiceBus.Gateway.RavenDB](/nservicebus/gateway/ravendb) documentation for further details.
