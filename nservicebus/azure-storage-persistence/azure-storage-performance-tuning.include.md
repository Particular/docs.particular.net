Azure Storage Persistence and Transport are network IO intensive. Every operation performed against the storage implies one or more network hops, most of which are small HTTP requests to a single IP address (of the storage cluster). By default the .NET framework has been configured to be very restrictive when it comes to this kind of communication.

	
## Network Optimizations

Performance can be drastically improved by overriding the following settings. The `ServicePointManager` class can be used for this purpose by changing its settings. The changes must be done before the application makes any outbound connection, ideally very early in the application's startup routine:


### Default Connection Limit

	ServicePointManager.DefaultConnectionLimit = 100;

The .NET Framework is configured to only allow 2 simultaneous connections to the same resource by default. The throughput to the storage can be highly improved by allowing more connections. 

The optimal value depends on the physical properties of the host machine. Creating new connections is an expensive operation. The Framework will try to avoid creating new connections by reusing connections from completed requests. Setting the connection limit too high may also result in a drop in performance because all requests will be issued in parallel instead of reusing existing connections. The ideal number is lower than the average amount of parallel storage operations. We recommend to start with a value of 10 and adjusting the value based on the observed performance impact.


### Disable Nagle

	ServicePointManager.UseNagleAlgorithm = false;

The Nagle algorithm is a performance optimization for TCP/IP based networks but it has a negative impact on performance of requests when using Azure Storage services. See Microsoft's blog post [Nagle’s Algorithm is Not Friendly towards Small Requests](https://blogs.msdn.microsoft.com/windowsazurestorage/2010/06/25/nagles-algorithm-is-not-friendly-towards-small-requests/).


### Disable Expect100Continue

	ServicePointManager.Expect100Continue = false; 

Configures the client to not wait for a 100-Continue response from the server before transmitting data. This is an optimization to avoid sending larger payloads when the server rejects the request which isn't necessary for Azure Storage operations.
[ServicePointManager.Expect100Continue](https://msdn.microsoft.com/en-us/library/system.net.servicepointmanager.expect100continue.aspx)


## Azure Storage Performance Checklist

Check Microsoft's [Azure Storage Performance Checklist](https://azure.microsoft.com/en-us/documentation/articles/storage-performance-checklist/) which provides performance tips and design guidelines to optimize Azure Storage performance.