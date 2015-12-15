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
---

Various features of NServiceBus require persistence. Among them are subscription storage, sagas, and timeouts. Various storage options are available including, Azure Storage Services.


## How To enable persistence with Azure storage services

First you need to reference the assembly that contains the Azure storage persisters. The recommended way of doing this is by adding a NuGet package reference to the `NServiceBus.Azure` package to your project.

If self hosting, you can configure the persistence technology using the configuration API and the extension method found in the `NServiceBus.Azure` assembly

snippet:PersistanceWithAzure


## Hosting

When hosting in the Azure role entrypoint provided by `NServiceBus.Hosting.Azure`, or any other NServiceBus provided host, the Azure storage persistence can be enabled by specifying the `UsePersistence<AzureStoragePersistence>` on the endpoint config.

snippet:PersistenceWithAzureHost

NOTE: In Version 4, when hosting in the Azure role entrypoint provided by `NServiceBus.Hosting.Azure`, these persistence strategies will be enabled by default.


## Detailed Configuration

You can get more control on the behavior of each persister by specifying one of the respective configuration sections in your app.config and changing one of the available properties, or through code.


### Detailed Configuration with Configuration Section

snippet:AzurePersistenceFromAppConfig

The following settings are available for changing the behavior of saga persistence through the `AzureSagaPersisterConfig`section:

- `ConnectionString`: Allows you to set the connectionstring to the storage account for storing saga information, defaults to `UseDevelopmentStorage=true`
- `CreateSchema`: Instructs the persister to create the table automatically, defaults to true

The following settings are available for changing the behavior of subscription persistence through the `AzureSubscriptionStorageConfig` section:

- `ConnectionString`: Allows you to set the connection string to the storage account for storing subscription information, defaults to `UseDevelopmentStorage=true`
- `CreateSchema`: Instructs the persister to create the table automatically, defaults to true
- `TableName`: Lets you choose the name of the table for storing subscriptions, defaults to `Subscription`.

The following settings are available for changing the behavior of timeout persistence through the `AzureTimeoutPersisterConfig` section:

- `ConnectionString`: Allows you to set the connectionstring to the storage account for storing timeout information, defaults to `UseDevelopmentStorage=true`
- `TimeoutManagerDataTableName`: Allows you to set the name of the table where the timeout manager stores it's internal state, defaults to `TimeoutManagerDataTable`
- `TimeoutDataTableName`: Allows you to set the name of the table where the timeouts themselves are stored, defaults to `TimeoutDataTableName`
- `CatchUpInterval`: When a node hosting a timeout manager would go down, it needs to catch up with missed timeouts faster than it normally would (1 sec), this value allows you to set the catchup interval in seconds. Defaults to 3600, meaning it will process one hour at a time.
- `PartitionKeyScope`: The time range used as partition key value for all timeouts. For optimal performance, this should be in line with the catchup interval so it should come to no surprise that the default value also represents an hour: `yyyyMMddHH`.

For more information see [Configuring Azure Connection Strings](https://msdn.microsoft.com/en-us/library/azure/ee758697.aspx)


## Additional performance tips

Azure storage persistence is network IO intensive, every operation performed against storage implies one or more network hops, most of which are small http requests to a single IP address (of your storage cluster). By default the .NET framework has been configured to be very restrictive when it comes to this kind of communication:
- It only allows 2 simultaneous connections to a single IP address by default
- It's algorithm stack has been optimized for larger payload exchanges, not for small requests
- It doesn't trust the remote servers by default, so it verifies for revoked certificates on every request

You can drastically improve performance by overriding these settings. You can leverage the ServicePointManager class for this end and change it's settings, but this must be done before your application makes any outbound connection, so ideally it's done very early in your application's startup routine.

	ServicePointManager.DefaultConnectionLimit = 5000; // default settings only allows 2 concurrent requests per process to the same host
	ServicePointManager.UseNagleAlgorithm = false; // optimize for small requests
	ServicePointManager.Expect100Continue = false; // reduces number of http calls
	ServicePointManager.CheckCertificateRevocationList = false; // optional, only if you trust all your dependencies	


### Detailed Configuration with Code

For Sagas:

snippet:AzurePersistenceSagasCustomization

For Subscriptions:

snippet:AzurePersistenceSubscriptionsCustomization

For Timeouts:

snippet:AzurePersistenceTimeoutsCustomization

NOTE: Subscriptions and Timeouts persistence configuration only has an effect when used with Azure Storage Queues transport from NServiceBus Azure Version 6 and later.
