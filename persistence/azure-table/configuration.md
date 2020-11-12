---
title: Configuration
component: ASP
summary: Configuring Azure Storage as persistence
reviewed: 2019-12-05
redirects:
 - nservicebus/azure-storage-persistence/configuration
 - persistence/azure-storage/configuration
---

partial: sections

partial: code

### Configuration properties

Each area of the persister (sagas, subscriptions and timeouts) have values that can be set or changed.

#### Saga configuration
 
The following settings are available for changing the behavior of saga persistence section:

 * `ConnectionString`: Sets the connection string for the storage account to be used for storing saga information.
  * NServiceBus.Azure defaults to `UseDevelopmentStorage=true`.
  * NServiceBus.Persistence.AzureStorage version 1 and above defaults to `null`.
 * `CreateSchema`: Instructs the persister to create the table automatically. Defaults to `true`.
 * `AssumeSecondaryIndicesExist` (Added in version 1.4): Disables scanning for secondary index records when checking if a new saga should be created. A secondary index record was not created by the persister contained in the `NServiceBus.Azure` package. To provide backward compatibilty, the `NServiceBus.Persistence.AzureStorage` package performs a full table scan, across all partitions, for secondary index records before creating a new saga. For systems that have only used the `NServiceBus.Persistence.AzureStorage` library, or have verified that all saga instances have a secondary index record, full table scans can be safely disabled by using this configuration setting.

#### Subscription configuration

The following settings are available for changing the behavior of subscription persistence:

 * `ConnectionString`: Sets the connection string for the storage account to be used for storing subscription information.
  * NServiceBus.Azure defaults to `UseDevelopmentStorage=true`.
  * NServiceBus.Persistence.AzureStorage version 1 and above defaults to `null`.
 * `CreateSchema`: Instructs the persister to create the table automatically. Defaults to `true`.
 * `TableName`: Specifies the name of the table for storing subscriptions. Defaults to `Subscription`.
 * `CacheFor` (Added in Version 1.3): By default every time a message is published the subscription storage is queried. In scenarios where the list of subscribers rarely changes, this query is often redundant and can slow down message processing. `CacheFor` allows subscriptions to be cached for a given period of time, hence helping reduce the impact of redundant queries to the subscription store.


partial: timeouts

For more information on connection string configuration see [Configuring Azure Connection Strings](https://docs.microsoft.com/en-us/azure/storage/storage-configure-connection-string).