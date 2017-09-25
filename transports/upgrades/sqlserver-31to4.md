---
title: SQL Server Transport Upgrade Version 3 to 4
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

To run the upgraded project on .NET Core the transport needs to be switched to one of the native transactions modes. Consider using the [Outbox](/nservicebus/outbox) to maintain the same *exactly-once processing* guarantees.

NOTE: Transaction scope is supposed to be supported by `SqlConnection` in future versions of .NET Core. 


## Multi-instance mode

The multi-instance mode has been deprecated in Version 4. So the following is no longer supported:

snippet: 31to4-legacy-multi-instance

NServiceBus topologies with queues distributed between multiple catalogs and SQL Server instances can be migrated using a combination of [Transport Bridge](/nservicebus/bridge/) and multi-catalog [addressing](/transports/sql/addressing.md).


### Multi-catalog

The multi-instance configuration above can be replaced with a multi-catalog addressing configuration, provided that all the databases are hosted on a single SQL Server instance:

snippet: 31to4-multi-catalog


### Bridge

When communicating with an endpoint that connects to a different SQL Server instance, send messages through a bridge. The [multi-instance migration sample](/samples/sqltransport/multi-instance-migration) demonstrates this approach.

Both of these features are available for NServiceBus 6 (and SQL Server transport 3.1) so the topology migration can take place before switching to NServiceBus 7.