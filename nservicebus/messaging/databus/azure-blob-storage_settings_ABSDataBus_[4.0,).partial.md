 * `ConnectionString()`: The connection string to the storage account for storing databus properties; defaults to `UseDevelopmentStorage=true`.
 * `Container()`: Container name; defaults to `databus`.
 * `BasePath()`: The blobs' base path in the container; defaults to an empty string.
 * `MaxRetries`: Number of upload/download retries; defaults to 5 retries.
 * `NumberOfIOThreads`: Number of blocks that will be simultaneously uploaded; defaults to 1 thread.
 * `BackOffInterval`: The back-off time between retries; defaults to 30 seconds.