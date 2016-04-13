---
title: SQL Server Transport Upgrade Version 2 to 3
summary: Instructions on how to upgrade SQL Server Transport Version 2 to 3.
reviewed: 2016-03-24
tags:
 - upgrade
 - migration
related:
- nservicebus/sqlserver
- nservicebus/upgrades/5to6
---


## SQL Server Transport


### Transactions

The native transaction support has been split into two different levels: `ReceiveOnly` and `SendAtomicWithReceive`. SQL Server Transport supports both. `SendAtomicWithReceive` is equivalent to disabling distributed transactions in NServiceBus Version 5.

snippet:2to3-enable-native-transaction

As shown in the above snippet, transaction settings are now handled in the transport level configuration. 

For more details and examples refer to [Transaction configuration API](/nservicebus/upgrades/5to6.md#transaction-configuration-api) and [Transaction support](/nservicebus/messaging/transactions.md) pages.


### Connection factory

The custom connection factory method is now expected to be `async` and no parameters are passed to it by the framework:

snippet:sqlserver-custom-connection-factory


### Multi-schema support
 
The configuration API for [multi-schema support](/nservicebus/sqlserver/deployment-options.md#multi-schema) has now changed. The `Queue Schema` parameter is no longer supported in the config file and the code configuration API. 

The schema for the configured endpoint can be specified using `DefaultSchema` method:

snippet:sqlserver-singledb-multischema

or by defining a custom connection factory:

snippet:sqlserver-singledb-multidb-pull 

The custom connection factory is also used for specifying schemas for other endpoints that the configured endpoint communicates with.
When using configuration file to specify schemas for other endpoints, their schemas should be placed in the `MessageEndpointMappings` section and follow `endpoint-name@schema-name` convention:

snippet:sqlserver-singledb-multischema-config


### Multi-instance support

The configuration API for [multi-instance support](/nservicebus/sqlserver/deployment-options.md#multi-instance) has now changed. Multiple connection strings have to be provided by connection factory method passed to `EnableLagacyMultiInstanceMode` method.

snippet:sqlserver-multiinstance-upgrade

Note that this method has been introduced only for backwards compatibility in Version 3. In Versions 4 and higher it will be replaced by `multi-catalog` support on the transport level, and appropriate guidance on how to achieve benefits of `multi-instance` by using features provided by SQL Server.


### Circuit breaker

The parameter `PauseAfterReceiveFailure(TimeSpan)` is no longer supported. In Version 3, the pause value is hard-coded at 1 second.


### Indexes

Queue tables created by the SQL Server transport version 2.2.1 or lower require manual creation of a non-clustered index on the `[Expires]` column. The following SQL statement can be used to create the missing index:

snippet:sql-2.2.2-ExpiresIndex

The SQL Server transport will log a warning when it finds that the index is missing.