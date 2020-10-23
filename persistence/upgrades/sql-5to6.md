---
title: SQL Persistence Upgrade Version 5 to 6
summary: Instructions on how to upgrade to SQL Persistence version 6
reviewed: 2020-10-22
component: SqlPersistence
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---

## Move to .NET 4.6.1

The minimum .NET Framework version for NServiceBus.SqlServer version 6 is [.NET 4.6.1](https://dotnet.microsoft.com/download/dotnet-framework/net461).

**All projects must be updated to .NET Framework 4.6.1 before upgrading to version 6.**

It is recommended to update to .NET Framework 4.6.1 and perform a full migration to production **before** updating to version 6. This will help isolate any issues that may occur.

For solutions with many projects, the [Target Framework Migrator](https://marketplace.visualstudio.com/items?itemName=PavelSamokha.TargetFrameworkMigrator) Visual Studio extension can reduce the manual effort required in performing an upgrade.
