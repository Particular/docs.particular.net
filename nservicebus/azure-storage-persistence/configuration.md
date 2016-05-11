---
title: Azure Storage Persistence Configuration
summary: Configuring Azure Storage as persistence
tags:
- Azure
- Cloud
- Persistence
- Configuration
---

In NServiceBus.Azure the behavior of the AzureStoragePersister can be controlled by specifying one of the respective configuration sections in the `app.config` and changing the available properties, or by using the code API. The exact same settings are exposed via the `app.config` and the code configuration API.

In NServiceBus.Persistence.AzureStorage v1 and above XML based configuration is no longer available. Configuring the behavior of the persister is done using the code configuration API.

### Configuration with Configuration Section

snippet:AzurePersistenceFromAppConfig

The following settings are available for changing the behavior of saga persistence through the `AzureSagaPersisterConfig` section:

- `ConnectionString`: Sets the connectionstring for the storage account to be used for storing saga information.  Defaults to `UseDevelopmentStorage=true` in NServiceBus.Azure Versions 6 and below, and defaults to `null` in NServiceBus.Persistence.AzureStorage Version 1 and above.
- `CreateSchema`: Instructs the persister to create the table automatically, defaults to `true`

The following settings are available for changing the behavior of subscription persistence through the `AzureSubscriptionStorageConfig` section:

- `ConnectionString`: Sets the connection string for the storage account to be used for storing subscription information.  Defaults to `UseDevelopmentStorage=true` in NServiceBus.Azure Versions 6 and below, and defaults to `null` in NServiceBus.Persistence.AzureStorage Version 1 and above.
- `CreateSchema`: Instructs the persister to create the table automatically, defaults to `true`
- `TableName`: Specifies the name of the table for storing subscriptions, defaults to `Subscription`.

The following settings are available for changing the behavior of timeout persistence through the `AzureTimeoutPersisterConfig` section:

- `ConnectionString`: Sets the connectionstring for the storage account to be used for storing timeout information.  Defaults to `UseDevelopmentStorage=true` in NServiceBus.Azure Versions 6 and below, and defaults to `null` in NServiceBus.Persistence.AzureStorage Version 1 and above.
- `TimeoutManagerDataTableName`: Sets the name of the table where the timeout manager stores it's internal state, defaults to `TimeoutManagerDataTable`
- `TimeoutDataTableName`: Sets the name of the table where the timeouts themselves are stored, defaults to `TimeoutDataTableName`
- `CatchUpInterval`: When a node hosting a timeout manager would go down, it needs to catch up with missed timeouts faster than it normally would (1 sec), this value  sets the catchup interval in seconds. Defaults to 3600, meaning it will process one hour at a time.
- `PartitionKeyScope`: The time range used as partition key value for all timeouts. For optimal performance, this should be in line with the catchup interval so it should come to no surprise that the default value also represents an hour: `yyyyMMddHH`. Data in the table defined by `TimeoutDataTableName` will need to be migrated When modifying `PartitionKeyScope`.
- `TimeoutStateContainerName`: Sets the name of the container where the timeout state is stored, defaults to `timeoutstate` - **Added in NServiceBus.Persistence.AzureStorage v1.0**

For more information see [Configuring Azure Connection Strings](https://azure.microsoft.com/en-us/documentation/articles/storage-configure-connection-string/)

### Configuration with Code

For Sagas, Subscriptions and for Timeouts:

snippet:AzurePersistenceSubscriptionsAllConnectionsCustomization

For Sagas:

snippet:AzurePersistenceSagasCustomization

For Subscriptions:

snippet:AzurePersistenceSubscriptionsCustomization

For Timeouts:

snippet:AzurePersistenceTimeoutsCustomization

NOTE: Subscriptions and Timeouts persistence configuration only has an effect when used with Azure Storage Queues transport from NServiceBus.Azure Version 6.x and NServiceBus.Persistence.AzureStorage Version 1.x and above.