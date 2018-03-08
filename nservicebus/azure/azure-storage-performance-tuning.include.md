The Azure Storage persistence and transport are network IO intensive. Every operation performed against the storage implies one or more network hops, most of which are small HTTP requests to a single IP address (of the storage cluster). By default the .NET framework has been configured to be very restrictive when it comes to this kind of communication.


## Network optimizations

Performance can be improved by overriding the settings exposed by the `ServicePointManager` class:

NOTE: Changes to settings must be applied before the application makes an outbound connection, ideally very early in the application's startup routine.


### Default connection limit

	ServicePointManager.DefaultConnectionLimit = 100;

The .NET Framework is configured to allow only 2 simultaneous connections to the same resource by default. A higher connection limit allows more parallel requests and therefore results in a higher network throughput. Setting the connection limit too high bypasses the built-in connection reuse mechanism which may result in a sub-optimal resource usage.

The optimal value depends on the physical properties of the host machine and the endpoint's expected workload. The ideal number is lower than the average amount of parallel storage operations. It is recommended to start with a value of 10 and adjust the value based on the observed performance impact.


### Disable Nagle's algorithm

	ServicePointManager.UseNagleAlgorithm = false;

Nagle's algorithm is a performance optimization for TCP/IP based networks but it has a negative impact on performance of requests when using Azure Storage services. See Microsoft's blog post [Nagle’s Algorithm is Not Friendly towards Small Requests](https://blogs.msdn.microsoft.com/windowsazurestorage/2010/06/25/nagles-algorithm-is-not-friendly-towards-small-requests/).


### Disable Expect100Continue

	ServicePointManager.Expect100Continue = false;

Setting the [Expect100Continue property](https://msdn.microsoft.com/en-us/library/system.net.servicepointmanager.expect100continue.aspx) to `false` configures the client not to wait for a 100-Continue response from the server before transmitting data. Waiting for 100-Continue is an optimization to avoid sending larger payloads when the server rejects the request. That optimization isn't necessary for Azure Storage operations and disabling it may result in faster requests.

## Saga storage optimizations

### Disabled secondary index scanning when creating new sagas

A secondary index record was not created by the persister contained in the `NServiceBus.Azure` package. To provide backward compatibilty, the `NServiceBus.Persistence.AzureStorage` package performs a full table scan across all partitions for secondary index records before creating a new saga. For systems that have only used the `NServiceBus.Persistence.AzureStorage` library, or have verified that all saga instances have a secondary index record, full table scans can be safely disabled by using the [AssumeSecondaryIndicesExist](/persistence/azure-storage/configuration.md#configuration-properties-saga-configuration) setting.

## Azure Storage performance checklist

Refer to Microsoft's [Azure Storage Performance Checklist](https://docs.microsoft.com/en-us/azure/storage/storage-performance-checklist) for more performance tips and design guidelines to optimize Azure Storage performance.
