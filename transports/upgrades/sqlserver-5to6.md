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

The minimum .NET Framework version for version 6 is [.NET Framework 4.6.1](https://dotnet.microsoft.com/download/dotnet-framework/net461).

**All projects must be updated to .NET Framework 4.6.1 before upgrading to version 6.**

It is recommended to update to .NET Framework 4.6.1 and perform a full migration to production **before** updating to version 6. This will help isolate any issues that may occur.

For solutions with many projects, the [Target Framework Migrator](https://marketplace.visualstudio.com/items?itemName=PavelSamokha.TargetFrameworkMigrator) Visual Studio extension can reduce the manual effort required in performing an upgrade.

## Microsoft.Data.SqlClient compatibility offered with new NServiceBus.Transport.SqlServer package

The transport is now compatible with both `System.Data.SqlClient` and `Microsoft.Data.SqlClient`. The existing NServiceBus.SqlServer package references `System.Data.SqlClient`, and it is the package that should be used as long as compatibility with `System.Data.SqlClient` is required.
The new NServiceBus.Transport.SqlServer package references `Microsoft.Data.SqlClient`, and it is recommended to use this package for new projects, or if compatibility with `Microsoft.Data.SqlClient` is required in an existing project.

NOTE: `System.Data.SqlClient` is in maintenance mode. Microsoft will be bringing new features and improvements to [`Microsoft.Data.SqlClient`](https://www.nuget.org/packages/Microsoft.Data.SqlClient/) only. For more information, read [Introduction to the new Microsoft.Data.SqlClient](https://devblogs.microsoft.com/dotnet/introducing-the-new-microsoftdatasqlclient/). It is recommended to switch to the new SqlClient if possible. Having both transport packages available allows migrating each endpoint gradually to the new SqlClient.

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

In version 6, the timeout manager compatibility mode is disabled by default.

The `DisableTimeoutManagerCompatibility` API has been deprecated:

```
var delayedDelivery = transport.NativeDelayedDelivery();
delayedDelivery.DisableTimeoutManagerCompatibility();
```

To enable the timeout manager compatibility mode use:

snippet: 5to6-enable-timeout-manager-compatibility

## Compatibility with NServiceBus.Persistence.Sql

Regardless of the SqlClient used, the transport is compatible with NServiceBus.Persistence.Sql. It is recommended to use the same SqlClient in the transport as well as the persistence. When gradually migrating from System.Data.SqlClient to Microsoft.Data.SqlClient, the transport and the persistence can operate in a mixed-mode as long as the transport transaction mode is either [`ReceiveOnly` or `SendsWithAtomicReceive`](/transports/sql/transactions.md). If the transport operates with transport transaction mode, `TransactionScope`, using both clients will lead to DTC escalation in all cases, which might not be desirable.