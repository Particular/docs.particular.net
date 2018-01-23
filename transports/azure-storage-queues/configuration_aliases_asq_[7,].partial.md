

### Using aliases for connection strings to storage accounts

It is possible to accidentally leak sensitive information in the connection string for a storage account if it is not properly secured. For example the information can be leaked if an error occurs when communicating across untrusted boundaries, or if the error information is logged to an unsecured log file.

In order to prevent it only creating an alias for each connection string is allowed. The alias is mapped to the physical connection string, and connection strings are always referred to by their alias. In the event accidentally leaked aliases, not connection string are exposed.

This feature can be enabled when configuring the `AzureStorageQueueTransport`:

snippet: AzureStorageQueueUseAccountAliasesInsteadOfConnectionStrings

NOTE: For the default connection string without additional storage account specified, the default alias used is an empty string.

See also [Using aliases instead of connection strings](/transports/azure-storage-queues/multi-storageaccount-support.md#cross-namespace-routing-using-aliases-instead-of-connection-strings) for multi-account support.