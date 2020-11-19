

## Sanitization

If a queue name is longer than [63 characters](https://docs.microsoft.com/en-us/rest/api/storageservices/naming-queues-and-metadata), the Azure Storage Queues Transport will fail to start. The endpoint name would needs to be shortened following the sanitization rules enforced by the Storage queue service.

For more details refer to [Sanitization](/transports/azure-storage-queues/sanitization.md) document.