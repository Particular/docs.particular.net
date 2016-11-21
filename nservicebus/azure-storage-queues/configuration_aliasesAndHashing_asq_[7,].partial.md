### Using aliases for connection strings to storage accounts

It is possible to accidentally leak sensitive information in the connection string for a storage account if it is not properly secured. For example the information can be leaked if an error occurs when communicating across untrusted boundaries, or if the error information is logged to an unsecured log file.

In order to prevent it only creating an alias for each connection string is allowed. The alias is mapped to the physical connection string, and connection strings are always referred to by their alias. In the event of an error or when logging only the alias can be accidentally leaked.

This feature can be enabled when configuring the `AzureStorageQueueTransport`:

snippet:AzureStorageQueueUseAccountAliasesInsteadOfConnectionStrings

See also [Using aliases for connection strings to storage accounts for Scale Out](/nservicebus/azure-storage-queues/multi-storageaccount-support.md#using-aliases-for-connection-strings-to-storage-accounts-for-scale-out)


## Hashing algorithms

If a queue name is longer than [63 characters](https://docs.microsoft.com/en-us/rest/api/storageservices/fileservices/Naming-Queues-and-Metadata), the Azure Storage Queues Transport uses a hashing algorithm to rename it. The default algorithm is `MD5`. In order to use `SHA1`, apply the following configuration:

snippet:AzureStorageQueueUseSha1