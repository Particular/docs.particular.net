---
title: SQL Server Transport Upgrade Version 5 to 6
reviewed: 2020-04-02
component: SqlTransport
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---

Upgrading from SQL Server transport version 5 to version 6 is a major upgrade and requires careful planning. Read the entire upgrade guide before beginning the upgrade process.

## Move to .NET 4.6.1

The minimum .NET version for NServiceBus.SqlServer and NServiceBus.Transport.SqlServer version 6 is [.NET 4.6.1](https://dotnet.microsoft.com/download/dotnet-framework/net461).

**All projects (that reference NServiceBus.SqlServer) must be updated to .NET 4.6.1 before updating to NServiceBus.SqlServer version 6.**

It is recommended to update to .NET 4.6.1 and perform a full migration to production **before** updating to NServiceBus.SqlServer version 6. This will help isolate any issues that may occur.

For solutions with many projects, the [Target Framework Migrator](https://marketplace.visualstudio.com/items?itemName=PavelSamokha.TargetFrameworkMigrator) Visual Studio extension can reduce the manual effort required in performing an upgrade.

## Moved types from namespace `NServiceBus.Transport.SQLServer` to `NServiceBus.Transport.SqlServer`

Certain advanced configuration APIs have been moved from the namespace `NServiceBus.Transport.SQLServer` to `NServiceBus.Transport.SqlServer`. The code has to be adjusted accordingly. A straight forward way is to search and replace

```
using NServiceBus.Transport.SQLServer;
```

with

```
using NServiceBus.Transport.SqlServer;
```

## Timeout Manager Compatibility

In version 6, the timeout manager compability mode is disabled by default.

The following API has been deprecated:

snippet: 5to6-disable-timeout-manager-compatibility

To enable the timeout manager compatibility mode use:

snippet: 5to6-enable-timeout-manager-compatibility
