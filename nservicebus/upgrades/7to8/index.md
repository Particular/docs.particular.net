---
title: Upgrade Version 7 to 8
summary: Instructions on how to upgrade NServiceBus from version 7 to version 8.
component: Core
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
 - 8
---

## New gateway persistence API

NServiceBus Gateway has been moved to a separate `NServiceBus.Gateway` package and all Gateway public API in NServiceBus obsoleted with the following warning:

> Gateway persistence has been moved to the NServiceBus.Gateway dedicated package. Will be treated as an error from version 8.0.0. Will be removed in version 9.0.0.

### How to upgrade

- Select the storage type to use in production. Supported storage types are SqlServer and RavenDB
- Depending on the selected storage type add a reference to the respective Nuget package:
  - NServiceBus.Gateway.Sql
  - NServiceBus.Gateway.RavenDB
- Configure the Gateway api by invoking the `Gateway()` method on the endpoints configuration instance passing as an argument the selected storage configuration instance

Refer to the [NServiceBus.Gateway.Sql](/nservicebus/gateway/sql) or [NServiceBus.Gateway.RavenDB](/nservicebus/gateway/ravendb) documentation for further details.
