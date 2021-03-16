

### Using aliases for connection strings to storage accounts

It is possible to accidentally leak sensitive information in the connection string for a storage account if it is not properly secured. For example, the information can be leaked if an error occurs when communicating across untrusted boundaries, or if the error information is logged to an unsecured log file.

In order to prevent this, only creating an alias for each connection string is allowed. The alias is mapped to the physical connection string, and connection strings are always referred to by their alias. In the event an alias is leaked, the connection strings are not exposed.

This feature can be enabled when configuring the `AzureStorageQueueTransport`:

snippet: AzureStorageQueueUseAccountAliasesInsteadOfConnectionStrings

NOTE: For the default connection string without an additional storage account specified, an empty string is the default alias.

See also [Using aliases instead of connection strings](/transports/azure-storage-queues/multi-storageaccount-support.md#cross-namespace-routing-aliases-instead-of-connection-strings) for multi-account support.
