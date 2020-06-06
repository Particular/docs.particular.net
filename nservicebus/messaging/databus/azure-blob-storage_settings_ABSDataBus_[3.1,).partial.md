 * `ConnectionString()`: The connection string to the storage account for storing databus properties; defaults to `UseDevelopmentStorage=true`.
 * `Container()`: Container name; defaults to `databus`.
 * `BasePath()`: The blobs' base path in the container; defaults to an empty string.
 * `DefaultTTL`: Time in seconds to keep a blob in storage before it is removed; defaults to [Int64.MaxValue](https://docs.microsoft.com/en-us/dotnet/api/system.int64.maxvalue) seconds.
 * `MaxRetries`: Number of upload/download retries; defaults to 5 retries.
 * `NumberOfIOThreads`: Number of blocks that will be simultaneously uploaded; defaults to 1 thread.
 * `BackOffInterval`: The back-off time between retries; defaults to 30 seconds.
 * `BlockSize`: The size of a single block for upload when the number of I/O threads is more than 1; defaults to 4MB.
 * `CleanupInterval`: The default time interval to perform periodic clean-up of blobs for expired messages with specific TTL; disabled by defaults.
 * `AuthenticateWithManagedIdentity(storageAccountName, renewalTimeBeforeTokenExpires, endpointSuffix)`: Authenticate with Azure Managed Identity instead of connection string.
   * `storageAccountName`: The storage account name used for the data bus.
   * `renewalTimeBeforeTokenExpires`: How long before the current token expires a token renewal request should be be issued.
   * `endpointSuffix`: [Endpoint suffix](https://docs.microsoft.com/en-us/azure/active-directory/managed-identities-azure-resources/services-support-managed-identities#azure-storage-blobs-and-queues) used for the storage account. The default is set to public Azure cloud (`core.windows.net`).
