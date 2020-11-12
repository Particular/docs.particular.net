#### Timeout configuration

The following settings are available for changing the behavior of timeout persistence:

 * `ConnectionString`: Sets the connection string for the storage account to be used for storing timeout information.
  * NServiceBus.Azure version 6 and below defaults to `UseDevelopmentStorage=true`.
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