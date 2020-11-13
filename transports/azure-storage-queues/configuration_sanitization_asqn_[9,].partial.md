

## Sanitization

If a queue name is longer than [63 characters](https://docs.microsoft.com/en-us/rest/api/storageservices/naming-queues-and-metadata), the Azure Storage Queues Transport can be configured to sanitize queue names using a custom algorithm. By default, the transport does not sanitize queue names.

For more details refer to [Sanitization](/transports/azure-storage-queues/sanitization.md) document.