### Using aliases for connection strings to storage accounts

It is possible to accidentally leak sensitive information in the connection string for a storage account if it's not properly secured. E.g. the information can be leaked if an error occurs when communicating across untrusted boundaries, or if the error information is logged to an unsecured log file.

In order to prevent it, `NServiceBus.Azure.Transports.WindowsAzureStorageQueues` Versions 7 and above allow for creating an alias for each connection string. The alias is mapped to the physical connection string, and connection strings are always referred to by their alias. In the event of an error or when logging only the alias can be accidentally leaked.

This feature can be enabled when configuring the `AzureStorageQueueTransport`:

snippet:AzureStorageQueueUseAccountAliasesInsteadOfConnectionStrings

See also [Using aliases for connection strings to storage accounts for Scale Out](/nservicebus/azure-storage-queues/multi-storageaccount-support.md#using-aliases-for-connection-strings-to-storage-accounts-for-scale-out)

## Hashing algorithms

If a queue name is longer than [63 characters](https://msdn.microsoft.com/en-us/library/azure/dd179349.aspx), the Azure Storage Queues Transport uses a hashing algorithm to rename it. The default algorithm is `MD5`. In order to use `SHA1`, apply the following configuration:

snippet:AzureStorageQueueUseSha1

NOTE: This feature is available in `NServiceBus.Azure.Transports.WindowsAzureStorageQueues` Versions 7 and above.