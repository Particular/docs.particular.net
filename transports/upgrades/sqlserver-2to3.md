---
title: SQL Server Transport Upgrade Version 2 to 3
reviewed: 2016-09-13
component: SqlTransport
related:
 - nservicebus/upgrades/5to6
redirects:
 - nservicebus/upgrades/sqlserver-2to3
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---


## Namespace changes

The primary namespace is `NServiceBus`. Advanced APIs have been moved to `NServiceBus.Transport.SqlServer`. A `using NServiceBus` directive should be sufficient to find all basic options. A `using NServiceBus.Transport.SqlServer` is needed to access these configuration options.


### Transactions

The native transaction support has been split into two different levels: `ReceiveOnly` and `SendAtomicWithReceive`. Both are supported. `SendAtomicWithReceive` is equivalent to disabling distributed transactions in NServiceBus Version 5.

snippet: 2to3-enable-native-transaction

As shown in the above snippet, transaction settings are now handled in the transport level configuration.

For more details and examples refer to [Transaction configuration API](/nservicebus/upgrades/5to6/transaction-configuration.md) and [Transaction support](/transports/transactions.md) pages.


### Connection factory

The custom connection factory method is now expected to be `async` and no parameters are passed to it by the framework:

snippet: sqlserver-custom-connection-factory


### Accessing transport connection

Accessing transport connection can be done in Version 2 by injecting `SqlServerStorageContext` into a handler. This way the handler can access the data residing in the same database as the queue tables within the message receive transaction managed by the transport.

In Version 3 this API has been removed. The same goal can be achieved in Version 3 by using ambient transaction mode.

snippet: 2to3-enable-ambient-transaction

The handler needs to open a connection to access the data but, assuming both handler and the transport are configured to use the same connection string, there is no DTC escalation. SQL Server 2008 and later can detect that both connections target the same resource and merges the two transaction into a single lightweight transaction.

NOTE: The above statement is true only when not using the NHibernate persistence. NHibernate persistence opens its own connection within the context of the ambient transaction. See [access business data](/persistence/nhibernate/accessing-data.md) in context of processing a message with NHibernate persistence.


### Multi-schema support

The configuration API for [multi-schema support](/transports/sql/deployment-options.md#modes-overview-multi-schema) has now changed. The `Queue Schema` parameter is no longer supported in the config file and the code configuration API.

The schema for the configured endpoint can be specified using `DefaultSchema` method:

snippet: sqlserver-non-standard-schema

or by defining a custom schema per endpoint or queue:

snippet: sqlserver-multischema-config-for-endpoint-and-queue

This enables configuring custom schema both for local endpoint as well as for other endpoints that the configured endpoint communicates with. When using configuration file to specify schemas for other endpoints, their schemas should be placed in the `MessageEndpointMappings` section and follow `endpoint-name@schema-name` convention:

snippet: sqlserver-multischema-config


### Multi-instance support

The configuration API for [multi-instance support](/transports/sql/deployment-options.md#modes-overview-multi-instance) has now changed. Multiple connection strings have to be provided by connection factory method passed to `EnableLagacyMultiInstanceMode` method.

Note that `EnableLagacyMultiInstanceMode` method replaces both [pull and push modes](/transports/sql/connection-settings.md#multiple-connection-strings) from Version 2.x.

snippet: sqlserver-multiinstance-upgrade

Note that `multi-instance` mode has been deprecated and is not recommended for new projects.


#### Multi-instance support and Outbox

Version 3 does not support Outbox in `multi-instance` mode.


### Circuit breaker

The method `PauseAfterReceiveFailure(TimeSpan)` is no longer supported. In Version 3, the pause value is hard-coded at 1 second.


### Indexes

Queue tables created by version 2.2.1 or lower require manual creation of a non-clustered index on the `[Expires]` column. The following SQL statement can be used to create the missing index:

snippet: sql-2.2.2-ExpiresIndex

A warning will be logged when a missing index is detected.
