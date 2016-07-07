---
title: Azure Storage Persistence Configuration
summary: Configuring Azure Storage as persistence
tags:
- Azure
- Cloud
- Persistence
- Configuration
---

In NServiceBus.Azure the behavior of the `AzureStoragePersister` can be controlled by working with the appropriate configuration section(s) in the `app.config` or by using the code via the [`IProvideConfiguration` adapter](/nservicebus/hosting/custom-configuration-providers.md). Both approaches to configuring the persister can access the same configuration options.


In NServiceBus.Persistence.AzureStorage Version 1 XML-based configuration is no longer available. Configuring the behavior of the persister is done using the code configuration API.


### Configuration with Configuration Section

In NServiceBus.Azure Version 6 and lower configuration can be performed using configuration sections and properties in the `app.config` file.

snippet:AzurePersistenceFromAppConfig


### Configuration with Code

Configuration of the persister via the code API is available to NServiceBus.Azure Version 6 and below as well as NServiceBus.Persistence.AzureStorage Version 1.

For Sagas, Subscriptions and for Timeouts:

snippet:AzurePersistenceSubscriptionsAllConnectionsCustomization

For Sagas:

snippet:AzurePersistenceSagasCustomization

For Subscriptions:

snippet:AzurePersistenceSubscriptionsCustomization

For Timeouts:

snippet:AzurePersistenceTimeoutsCustomization


### Configuration Properties

Each area of the persister (Sagas, Subscriptions and Timeouts) have values that can be set or changed.

NOTE: Subscriptions and Timeouts persistence configurations have no effect when used with NServiceBus.Azure Version 5 and lower.


#### Saga Configuration
 
When using XML-based configuration, the following properties can be set through the `AzureSagaPersisterConfig` section:

 * `ConnectionString`: Sets the connectionstring for the storage account to be used for storing saga information.
  * NServiceBus.Azure Versions 6 and below defaults to `UseDevelopmentStorage=true`.
  * NServiceBus.Persistence.AzureStorage Version 1 defaults to `null`.
 * `CreateSchema`: Instructs the persister to create the table automatically.
  * defaults to `true`.


#### Subscription Configuration

The following settings are available for changing the behavior of subscription persistence through the `AzureSubscriptionStorageConfig` section:

 * `ConnectionString`: Sets the connection string for the storage account to be used for storing subscription information.
  * NServiceBus.Azure Versions 6 and below defaults to `UseDevelopmentStorage=true`.
  * NServiceBus.Persistence.AzureStorage Version 1 defaults to `null`.
 * `CreateSchema`: Instructs the persister to create the table automatically.
  * defaults to `true`.
 * `TableName`: Specifies the name of the table for storing subscriptions
  * defaults to `Subscription`.


#### Timeout Configuration

The following settings are available for changing the behavior of timeout persistence through the `AzureTimeoutPersisterConfig` section:

 * `ConnectionString`: Sets the connectionstring for the storage account to be used for storing timeout information.
  * NServiceBus.Azure Versions 6 and below Defaults to `UseDevelopmentStorage=true`.
  * NServiceBus.Persistence.AzureStorage Version 1 defaults to `null`.
 * `TimeoutManagerDataTableName`: Sets the name of the table where the timeout manager stores it's internal state.
  * defaults to `TimeoutManagerDataTable`.
 * `TimeoutDataTableName`: Sets the name of the table where the timeouts themselves are stored.
  * defaults to `TimeoutDataTableName`.
 * `CatchUpInterval`: When a node hosting a timeout manager goes down, it needs to catch up with missed timeouts faster than it normally would (1 sec), this value  sets the catchup interval in seconds.
  * defaults to 3600, meaning it will process one hour at a time.
 * `PartitionKeyScope`: The time range used as partition key value for all timeouts. For optimal performance this should be in line with the catchup interval. Data in the table defined by `TimeoutDataTableName` will need to be migrated when modifying `PartitionKeyScope`.
  * defaults to `yyyMMddHH`.
 * `TimeoutStateContainerName`: Sets the name of the container where the timeout state is stored - **Added in NServiceBus.Persistence.AzureStorage Version 1.0**.
  * defaults to `timeoutstate`.

For more information on connection string configuration see [Configuring Azure Connection Strings](https://azure.microsoft.com/en-us/documentation/articles/storage-configure-connection-string/).