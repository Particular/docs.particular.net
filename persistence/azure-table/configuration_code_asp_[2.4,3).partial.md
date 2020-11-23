For sagas, subscriptions and for timeouts:

snippet: AzurePersistenceAllConnectionsCustomization

### Saga configuration

snippet: AzurePersistenceSagasCustomization

The following settings are available for changing the behavior of saga persistence section:

 * `ConnectionString`: Sets the connection string for the storage account to be used for storing saga information.
 * NServiceBus.Persistence.AzureStorage version 1 and above defaults to `null`.
 * `CreateSchema`: Instructs the persister to create the table automatically. Defaults to `true`.
 * `AssumeSecondaryIndicesExist` (Added in version 1.4): Disables scanning for secondary index records when checking if a new saga should be created. A secondary index record was not created by the persister contained in the `NServiceBus.Azure` package. To provide backward compatibility, the `NServiceBus.Persistence.AzureStorage` package performs a full table scan, across all partitions, for secondary index records before creating a new saga. For systems that have only used the `NServiceBus.Persistence.AzureStorage` library, or have verified that all saga instances have a secondary index record, full table scans can be safely disabled by using this configuration setting.


### Subscription configuration

snippet: AzurePersistenceSubscriptionsCustomization

The following settings are available for changing the behavior of subscription persistence:

 * `ConnectionString`: Sets the connection string for the storage account to be used for storing subscription information.
 * NServiceBus.Persistence.AzureStorage version 1 and above defaults to `null`.
 * `CreateSchema`: Instructs the persister to create the table automatically. Defaults to `true`.
 * `TableName`: Specifies the name of the table for storing subscriptions. Defaults to `Subscription`.
 * `CacheFor` (Added in Version 1.3): By default every time a message is published the subscription storage is queried. In scenarios where the list of subscribers rarely changes, this query is often redundant and can slow down message processing. `CacheFor` allows subscriptions to be cached for a given period of time, hence helping reduce the impact of redundant queries to the subscription store.


### Timeout configuration

Timeout persistence has been deprecated as of version 2.4.0 and above.
For Azure Storage Queues use [Delayed Delivery API](/transports/azure-storage-queues/delayed-delivery.md).
