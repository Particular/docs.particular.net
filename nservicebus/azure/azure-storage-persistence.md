---
title: Azure Storage Persistence
summary: Using Azure Storage for persistence features of NServiceBus including timeouts, sagas, and subscription storage.
tags:
- Azure
- Cloud
- Persistence
- Performance
- Hosting
redirects:
 - nservicebus/using-azure-storage-persistence-in-nservicebus
related:
 - samples/azure/storage-queues
reviewed: 2016-03-07
---

Certain features of NServiceBus require persistence to permanently store data. Among them are subscription storage, sagas, and timeouts. Various storage options are available including Azure Storage Services.


## How to enable persistence with Azure Storage Services

First add a reference to the assembly that contains the Azure storage persisters. The recommended way of doing this is by adding a NuGet package reference to the `NServiceBus.Azure` package.

If self hosting, the persistence technology could be configured using the configuration API and the extension method found in the `NServiceBus.Azure` assembly.

snippet:PersistanceWithAzure


## Hosting

When hosting in the Azure role entrypoint provided by `NServiceBus.Hosting.Azure`, or any other NServiceBus provided host, the Azure storage persistence can be enabled by specifying the `UsePersistence<AzureStoragePersistence>` on the endpoint config.

snippet:PersistenceWithAzureHost

NOTE: In Version 4, when hosting in the Azure role entrypoint provided by `NServiceBus.Hosting.Azure`, these persistence strategies will be enabled by default.


## Detailed Configuration

The behavior of each persister can be controlled by specifying one of the respective configuration sections in the `app.config` and changing available properties, or using code API. The exact same settings are exposed via `app.config` file and code configuration API.

### Detailed Configuration with Configuration Section

snippet:AzurePersistenceFromAppConfig

The following settings are available for changing the behavior of saga persistence through the `AzureSagaPersisterConfig` section:

- `ConnectionString`: Sets the connectionstring for the storage account to be used for storing saga information.  Defaults to `UseDevelopmentStorage=true` in Versions 6 and below, and defaults to `null` in Versions 7 and above.
- `CreateSchema`: Instructs the persister to create the table automatically, defaults to `true`

The following settings are available for changing the behavior of subscription persistence through the `AzureSubscriptionStorageConfig` section:

- `ConnectionString`: Sets the connection string for the storage account to be used for storing subscription information.  Defaults to `UseDevelopmentStorage=true` in Versions 6 and below, and defaults to `null` in Versions 7 and above.
- `CreateSchema`: Instructs the persister to create the table automatically, defaults to `true`
- `TableName`: Specifies the name of the table for storing subscriptions, defaults to `Subscription`.

The following settings are available for changing the behavior of timeout persistence through the `AzureTimeoutPersisterConfig` section:

- `ConnectionString`: Sets the connectionstring for the storage account to be used for storing timeout information.  Defaults to `UseDevelopmentStorage=true` in Versions 6 and below, and defaults to `null` in Versions 7 and above.
- `TimeoutManagerDataTableName`: Sets the name of the table where the timeout manager stores it's internal state, defaults to `TimeoutManagerDataTable`
- `TimeoutDataTableName`: Sets the name of the table where the timeouts themselves are stored, defaults to `TimeoutDataTableName`
- `CatchUpInterval`: When a node hosting a timeout manager would go down, it needs to catch up with missed timeouts faster than it normally would (1 sec), this value  sets the catchup interval in seconds. Defaults to 3600, meaning it will process one hour at a time.
- `PartitionKeyScope`: The time range used as partition key value for all timeouts. For optimal performance, this should be in line with the catchup interval so it should come to no surprise that the default value also represents an hour: `yyyyMMddHH`. Data in the table defined by `TimeoutDataTableName` will need to be migrated When modifying `PartitionKeyScope`.
- `TimeoutStateContainerName`: Sets the name of the container where the timeout state is stored, defaults to `timeoutstate` - **Added in v7.0**

For more information see [Configuring Azure Connection Strings](https://azure.microsoft.com/en-us/documentation/articles/storage-configure-connection-string/)

### Detailed Configuration with Code

For Sagas, Subscriptions and for Timeouts:

snippet:AzurePersistenceSubscriptionsAllConnectionsCustomization

For Sagas:

snippet:AzurePersistenceSagasCustomization

For Subscriptions:

snippet:AzurePersistenceSubscriptionsCustomization

For Timeouts:

snippet:AzurePersistenceTimeoutsCustomization

NOTE: Subscriptions and Timeouts persistence configuration only has an effect when used with Azure Storage Queues transport from NServiceBus Azure Version 6 and later.

## Additional performance tips

Azure storage persistence is network IO intensive. Every operation performed against storage implies one or more network hops, most of which are small http requests to a single IP address (of the storage cluster). By default the .NET framework has been configured to be very restrictive when it comes to this kind of communication:
- It only allows 2 simultaneous connections to a single IP address by default
- It doesn't trust the remote servers by default, so it verifies for revoked certificates on every request
- The algorithms were optimized for larger payload exchanges, not for small requests

Performance can be drastically improved by overriding these settings. The `ServicePointManager` class can be used for this purpose by changing its settings. The changes must be done before the application makes any outbound connection, ideally very early in the application's startup routine:

	ServicePointManager.DefaultConnectionLimit = 5000; // default settings only allows 2 concurrent requests per process to the same host
	ServicePointManager.UseNagleAlgorithm = false; // optimize for small requests
	ServicePointManager.Expect100Continue = false; // reduces number of http calls
	ServicePointManager.CheckCertificateRevocationList = false; // optional, only disable if all dependencies are trusted	