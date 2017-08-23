---
title: SQL Server Transport Upgrade Version 3.1 to 4
reviewed: 2017-08-23
component: SqlTransport
related:
 - nservicebus/upgrades/6to7
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---

## TransactionScope

The Transaction scope [transaction mode](/transports/sql/transactions.md) is not available in .NET Core 2.0 because the implementation of `SqlConnection` does not support enlisting in an ambient transaction. 

To run the upgraded project on .NET Core the transport needs to be switched to one of the native transactions modes. Consider using the [Outbox](/nservicebus/outbox.md) to maintain the same *exactly-once processing* guarantees.

NOTE: Transaction scope is supposed to be supported by `SqlConnection` in future versions of .NET Core. 
