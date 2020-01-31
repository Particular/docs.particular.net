---
title: SQL Persistence Upgrade Version 4 to 5
summary: Instructions on how to upgrade to SQL Persistence version 4.3
reviewed: 2018-10-15
component: SqlPersistence
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---

## Move to .NET 4.6.1

The minimum .NET version for NServiceBus.SqlServer and NServiceBus.Transport.SqlServer version 6 is [.NET 4.6.1](https://dotnet.microsoft.com/download/dotnet-framework/net461).

**All projects (that reference NServiceBus.SqlServer) must be updated to .NET 4.6.1 before updating to NServiceBus.SqlServer version 6.**

It is recommended to update to .NET 4.6.1 and perform a full migration to production **before** updating to NServiceBus.SqlServer version 6. This will help isolate any issues that may occur.

For solutions with many projects, the [Target Framework Migrator](https://marketplace.visualstudio.com/items?itemName=PavelSamokha.TargetFrameworkMigrator) Visual Studio extension can reduce the manual effort required in performing an upgrade.

## Dropped Reference to System.Data.SqlClient

TBD

## Compatibility with System.Data.SqlClient and Microsoft.Data.SqlClient

The persistence is fully compatible with System.Data.SqlClient and Microsoft.Data.SqlClient