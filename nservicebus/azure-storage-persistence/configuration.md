---
title: Configuration
component: ASP
summary: Configuring Azure Storage as persistence
tags:
 - Azure
 - Persistence
reviewed: 2016-11-07
---

partial: sections

partial: code

### Configuration Properties

Each area of the persister (Sagas, Subscriptions and Timeouts) have values that can be set or changed.

#### Saga Configuration
 
The following settings are available for changing the behavior of saga persistence section:

 * `ConnectionString`: Sets the connectionstring for the storage account to be used for storing saga information.
  * NServiceBus.Azure defaults to `UseDevelopmentStorage=true`.
  * NServiceBus.Persistence.AzureStorage Version 1 defaults to `null`.
 * `CreateSchema`: Instructs the persister to create the table automatically.
  * defaults to `true`.


#### Subscription Configuration

The following settings are available for changing the behavior of subscription persistence:

 * `ConnectionString`: Sets the connection string for the storage account to be used for storing subscription information.
  * NServiceBus.Azure defaults to `UseDevelopmentStorage=true`.
  * NServiceBus.Persistence.AzureStorage Version 1 defaults to `null`.
 * `CreateSchema`: Instructs the persister to create the table automatically.
  * defaults to `true`.
 * `TableName`: Specifies the name of the table for storing subscriptions
  * defaults to `Subscription`.
 * `CacheFor`: (Added in Version 1.3) By default every time a message is published the subscription storage is queried. In scenarios where the list of subscribers rarely changes this query is often redundant and can slow down message processing. `CacheFor` allows subscriptions to be cached for a given period of time, hence helping reduce the impact of redundant queries to the subscription store.


#### Timeout Configuration

The following settings are available for changing the behavior of timeout persistence:

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

For more information on connection string configuration see [Configuring Azure Connection Strings](https://docs.microsoft.com/en-us/azure/storage/storage-configure-connection-string).