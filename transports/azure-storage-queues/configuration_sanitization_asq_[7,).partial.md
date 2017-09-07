

## Shortening

If a queue name is longer than [63 characters](https://docs.microsoft.com/en-us/rest/api/storageservices/naming-queues-and-metadata), the Azure Storage Queues Transport uses a hashing algorithm to rename it. The default algorithm is `MD5`. In order to use `SHA1`, apply the following configuration:

snippet: AzureStorageQueueUseSha1