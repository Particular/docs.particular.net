 * `ConnectionString()`: The connection string to the storage account for storing DataBus properties; defaults to `UseDevelopmentStorage=true`.
 * `Container()`: Container name; defaults to `databus`.
 * `BasePath()`: The blobs' base path in the container; defaults to an empty string.
 * `DefaultTTL`: Time in seconds to keep a blob in storage before it is removed; defaults to [Int64.MaxValue](https://msdn.microsoft.com/en-us/library/system.int64.maxvalue.aspx) seconds.
 * `MaxRetries`: Number of upload/download retries; defaults to 5 retries.
 * `NumberOfIOThreads`: Number of blocks that will be simultaneously uploaded; defaults to 5 threads.
 * `BackOffInterval`: The back-off time between retries; defaults to 30 seconds.
 * `BlockSize`: The size of a single block for upload when the number of I/O threads is more than 1; defaults to 4MB.
 * `CleanupInterval`: The default time interval to perform periodic clean up of blobs for expired messages with specific TTL; defaults to 5 minutes.
