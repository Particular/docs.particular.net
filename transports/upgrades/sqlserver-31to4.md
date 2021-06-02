---
title: SQL Server Transport Upgrade Version 3 to 4
reviewed: 2021-06-02
component: SqlTransport
related:
 - nservicebus/upgrades/6to7
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---


## TransactionScope

[TransactionScope transaction mode](/transports/sql/transactions.md#transaction-scope) is not available in .NET Core 2.0 because the implementation of `SqlConnection` does not support enlisting in an ambient transaction. 

To run the upgraded project on .NET Core the transport needs to be switched to one of the native transactions modes. Consider using the [Outbox](/nservicebus/outbox) to maintain the same *exactly-once processing* guarantees.

NOTE: Transaction scope is supposed to be supported by `SqlConnection` in future versions of .NET Core. 


## Multi-instance mode

The multi-instance mode has been deprecated in Version 4. So the following is no longer supported:

snippet: 31to4-legacy-multi-instance

NServiceBus topologies with queues distributed between multiple catalogs hosted in a single instance of SQL Server can now be configured using multi-catalog [addressing](/transports/sql/addressing.md):

snippet: 31to4-multi-catalog

If catalogs are hosted in different instances of SQL Server, use [NServiceBus.Router](/nservicebus/router/) to construct a bridge. The [multi-instance migration sample](/samples/sqltransport/multi-instance-migration) demonstrates this approach.
