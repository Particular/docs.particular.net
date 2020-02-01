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

The persistence no longer references the System.Data.SqlClient and thus no longer brings that as a package dependency. Customers using SQL Server as a transport need to choose an appropriate SqlClient library. For example if compatibility with System.Data.SqlClient is desired it is recommended to add [`System.Data.SqlClient`](https://www.nuget.org/packages/System.Data.SqlClient/) version 4.4.3 or higher. 

NOTE: `System.Data.SqlClient` is in maintenance mode. Microsoft is comitted to bring new features and improvements to [`Microsoft.Data.SqlClient`](https://www.nuget.org/packages/Microsoft.Data.SqlClient/) only. For more information read [Introduction to the new Microsoft.Data.SqlClient](https://devblogs.microsoft.com/dotnet/introducing-the-new-microsoftdatasqlclient/). It is recommended to switch to the new sql client if possible.

The persistence is fully compatible with System.Data.SqlClient and Microsoft.Data.SqlClient

## Compatibility with NServiceBus.SqlServer and NServiceBus.Transport.SqlServer

Regardless of the SqlClient used the persistence is compatible with NServiceBus.SqlServer and NServiceBus.Transport.SqlServer. It is recommended to use the same SqlClient in the transport as well as the persistence. When gradually migrating from System.Data.SqlClient to Microsoft.Data.SqlClient the transport and the persistence can operate in a mixed mode as long as the transport transaction is either [`ReceiveOnly` or `SendsWithAtomicReceive`](/transports/sql/transactions.md). If the transport operates with transport transaction mode `TransactionScope` using both clients will lead to DTC escalation in all cases which might not be desirable.