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

snippet: AzurePersistenceTimeoutsCustomization

The following settings are available for changing the behavior of timeout persistence:

 * `ConnectionString`: Sets the connection string for the storage account to be used for storing timeout information.
  * NServiceBus.Persistence.AzureStorage version 1 and above defaults to `null`.
 * `TimeoutManagerDataTableName`: Sets the name of the table where the timeout manager stores its internal state.
  * defaults to `TimeoutManagerDataTable`.
 * `TimeoutDataTableName`: Sets the name of the table where the timeouts themselves are stored. Defaults to `TimeoutDataTableName`.
 * `CatchUpInterval`: When a node hosting a timeout manager goes down, it needs to catch up with missed timeouts faster than it normally would (1 sec); this value sets the catchup interval in seconds.
  * defaults to 3600, i.e. it will process one hour at a time.
 * `PartitionKeyScope`: The time range used as partition key value for all timeouts. For optimal performance this should be in line with the catchup interval. Data in the table defined by `TimeoutDataTableName` must be migrated when modifying `PartitionKeyScope`.
  * defaults to `yyyMMddHH`.
 * `TimeoutStateContainerName`: Sets the name of the container where the timeout state is stored - **Added in NServiceBus.Persistence.AzureStorage version 1.0**.
  * defaults to `timeoutstate`.