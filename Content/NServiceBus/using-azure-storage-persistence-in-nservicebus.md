---
title: Using Azure Storage Persistence In NServiceBus
summary: Using Windows Azure Storage for persistence features of NServiceBus including timeouts, sagas, and subscription storage.
tags: []
---

Various features of NServiceBus require persistence. Among them are subscription storage, sagas, and timeouts. Various storage options are available including, Windows Azure Storage Services.

How To enable persistence with windows azure storage services
-----------------

First you need to reference the assembly that contains the azure storage persisters. The recommended way of doing this is by adding a nuget package reference to the  `NServiceBus.Azure` package to your project.

If self hosting, you can configure the persistence technology using the fluent configuration API and the extension methods found in the `NServiceBus.Azure` assembly

    static void Main()
    {
        Configure.With()
        ...
			.AzureSubscriptionStorage()
			.AzureSagaPersister()
			.UseAzureTimeoutPersister()
		...
        .Start();
    }

When hosting in the Windows azure role entrypoint provided by `NServiceBus.Hosting.Azure`, these persistence strategies will be enabled by default.

But when hosting in a different NServiceBus provided host, you can enable them by implementing `INeedInitialization`, like this:

    public class EnableStorage : INeedInitialization
    {
        public void Init()
        {
            Configure.Instance
                .AzureSubscriptionStorage()
                .AzureSagaPersister()
                .UseAzureTimeoutPersister();
        }
    }

Detailed configuration
----------------------

You can get more control on the behavior of each persister by specifying one of the respective configuration sections in your app.config and changing one of the available properties.

	<configSections>
	    <section name="AzureSubscriptionStorageConfig" type="NServiceBus.Config.AzureSubscriptionStorageConfig, NServiceBus.Azure" />
	    <section name="AzureSagaPersisterConfig" type="NServiceBus.Config.AzureSagaPersisterConfig, NserviceBus.Azure" />
	    <section name="AzureTimeoutPersisterConfig" type="NServiceBus.Config.AzureTimeoutPersisterConfig, NserviceBus.Azure" />
	</configSections>
	<AzureSagaPersisterConfig ConnectionString="UseDevelopmentStorage=true" />
	<AzureTimeoutPersisterConfig ConnectionString="UseDevelopmentStorage=true" />
	<AzureSubscriptionStorageConfig ConnectionString="UseDevelopmentStorage=true" />

The following settings are available for changing the behavior of subscription persistence through the `AzureSubscriptionStorageConfig` section:

- `ConnectionString`: Allows you to set the connectionstring to the storage account for storing subscription information, defaults to `UseDevelopmentStorage=true`
- `CreateSchema`: Instructs the persister to create the table automatically, defaults to true
- `TableName`: Lets you choose the name of the table for storing subscriptions, defaults to `Subscription`.


The following settings are available for changing the behavior of saga persistence through the `AzureSagaPersisterConfig`section:

- `ConnectionString`: Allows you to set the connectionstring to the storage account for storing saga information, defaults to `UseDevelopmentStorage=true`
- `CreateSchema`: Instructs the persister to create the table automatically, defaults to true


The following settings are available for changing the behavior of timeout persistence through the `AzureTimeoutPersisterConfig` section:

- `ConnectionString`: Allows you to set the connectionstring to the storage account for storing tiemout information, defaults to `UseDevelopmentStorage=true`
- `TimeoutManagerDataTableName`: Allows you to set the name of the table where the timeout manager stores it's internal state, defaults to `TimeoutManagerDataTable`
- `TimeoutDataTableName`: Allows you to set the name of the table where the timeouts themselves are stored, defaults to `TimeoutDataTableName`
- `CatchUpInterval`: When a node hosting a timeout manager would go down, it needs to catch up with missed timeouts faster than it normally would (1 sec), this value allows you to set the catchup interval in seconds. Defaults to 3600, meaning it will process one hour at a time.
- `PartitionKeyScope`: The time range used as partitionkey value for all timeouts. For optimal performance, this should be in line with the catchup interval so it should come to no surprise that the default value also represents an hour: yyyMMddHH. 

Sample
------

Want to see these persisters in action? Checkout the [Video store sample.](https://github.com/Particular/NServiceBus.Azure.Samples/tree/master/VideoStore.AzureStorageQueues.Cloud) and more specifically, the `VideoStore.Sales` endpoint