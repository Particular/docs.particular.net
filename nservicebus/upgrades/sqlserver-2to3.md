---
title: Upgrade SQL Server Transport from Version 2 to Version 3
summary: Instructions on how to upgrade from SQL Server Transport Versions 2 to 3
tags:
 - upgrade
 - migration
related:
- nservicebus/upgrades/5to6
---


## SQL Server Transport


### Transactions

The transactions API was changed. The native transaction support has been split into two different levels: `ReceiveOnly` and `SendAtomicWithReceive`. SQL Server Transport supports both of them. `SendAtomicWithReceive` is equivalent to disabling distributed transactions in Version 5.

snippet:5to6-enable-native-transaction

As shown in the above snippet, transaction settings are now handled in the transport level configuration. 

For more details and examples refer to [Transaction configuration API](/nservicebus/upgrades/5to6.md#transaction-configuration-API) and [Transaction support](/nservicebus/messaging/transactions.md) pages.


### Connection factory

The custom connection factory method is now expected to be `async` and no parameters are passed to it by the framework:

snippet:sqlserver-custom-connection-factory


### Multi-schema support
 
The configuration API for enabling [multi-schema support](/nservicebus/sqlserver/multiple-databases.md#single-database-with-multiple schemas) has now changed. The `Queue Schema` parameter is no longer supported in the config file and the code configuration API. 

The schema for the configured endpoint can be specified using `DefaultSchema` method:

snippet:sqlserver-singledb-multischema

or by defining a custom connection factory:

snippet:sqlserver-singledb-multidb-pull 

The custom connection factory is also used for specifying schemas for other endpoints that the configured endpoint communicates with.
When using configuration file to specify schemas for other endpoints, their schemas should be placed in the `MessageEndpointMappings` section and follow `endpoint-name@schema-name` convention: 

snippet:sqlserver-singledb-multischema-config


### Circuit breaker

The parameter `PauseAfterReceiveFailure(TimeSpan)` is no longer supported. In Version 3, the pause value is hard-coded at 1 second.