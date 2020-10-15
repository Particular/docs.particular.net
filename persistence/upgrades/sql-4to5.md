---
title: SQL Persistence Upgrade Version 4 to 5
summary: Instructions on how to upgrade to SQL Persistence version 5
reviewed: 2020-06-29
component: SqlPersistence
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---

## Microsoft.Data.SqlClient compatibility

In order to add support for `Microsoft.Data.SqlClient`, the persister no longer references `System.Data.SqlClient`. The persister is compatible with both clients, but a package reference to `System.Data.SqlClient` or `Microsoft.Data.SqlClient` will need to be added to any projects using the persister.

NOTE: `System.Data.SqlClient` is in maintenance mode. Microsoft will be bringing new features and improvements to [`Microsoft.Data.SqlClient`](https://www.nuget.org/packages/Microsoft.Data.SqlClient/) only. For more information, read [Introduction to the new Microsoft.Data.SqlClient](https://devblogs.microsoft.com/dotnet/introducing-the-new-microsoftdatasqlclient/). It is recommended to switch to the new client if possible.

## Compatibility with NServiceBus.SqlServer and NServiceBus.Transport.SqlServer

Regardless of the client used, the persister is compatible with NServiceBus.SqlServer and NServiceBus.Transport.SqlServer. It is recommended to use the same client in the transport as well as the persister. When gradually migrating from `System.Data.SqlClient` to `Microsoft.Data.SqlClient`, the transport and the persister can operate in a mixed mode as long as the transport transaction mode is either [`ReceiveOnly` or `SendsWithAtomicReceive`](/transports/sql/transactions.md). If the transport operates with transport transaction mode, `TransactionScope`, using both clients will lead to DTC escalation in all cases, which might not be desirable.