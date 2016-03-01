---
title: Upgrade from Version 5 to Version 6
summary: Instructions on how to upgrade from NServiceBus Versions 5 to 6
tags:
 - upgrade
 - migration
related:
- nservicebus/sagas/concurrency
---


## SQL Server Transport

### Transactions
Transaction settings are now handled in the transport level configuration. For details refer to [Transaction configuraton API](/nservicebus/upgrades/5to6.md#transaction-configuration-API) section.

The native transaction support has been split into two different levels: `ReceiveOnly` and `SendAtomicWithReceive`. SQL Server Transport supports both of them. `SendAtomicWithReceive` is equivalent to disabling distributed transactions in Version 5. For more details refer to [Transaction configuraton API](/nservicebus/upgrades/5to6.md#transaction-configuration-API) section and [Transaction support](/nservicebus/messaging/transactions.md) page.

### Connection factory

The custom connection factory method is now expected to be `async` and no parameters are passed to it by the framework:

snippet:sqlserver-custom-connection-factory

### Multi-schema support
 
The API and config structure for multi-schema configuration [were changed](/nservicebus/sqlserver/multiple-databases.md#single-database-with-multiple schemas). The `Queue Schema` parameter is no longer supported in the config file and the code configuration API. 

The schema for the current endpoint can be specified using `DefaultSchema` method:

snippet:sqlserver-singledb-multischema

or by defining a custom connection factory:

snippet:sqlserver-singledb-multidb-pull 

The latter is also used for specifying schemas for other endpoints that the configured endpoint communicates with.
When using configuration file to specify schemas for other endpoints, their schemas should be placed in the `MessageEndpointMappings` section and follow `endpoint-name@schema-name` convention: 

snippet:sqlserver-singledb-multischema-config

### Circuit breaker

The parameter `PauseAfterReceiveFailure(TimeSpan)` is no longer supported. In Version 3, the pause value is hard-coded at 1 second.
