### Using the built-in clean-up method

Specify a value for the `TimeToBeReceived` property. For more details on how to specify this, see [Discarding Old Messages](/nservicebus/messaging/discard-old-messages.md).

WARN: The built-in method uses continuous blob scanning which can add to the cost of the storage operations. It is **not** recommended for multiple endpoints that are scaled out. If this method is not used, be sure to disable the built-in cleanup by setting the `CleanupInterval` to `0`. In versions 3 and above built-in cleanup is disabled by default.

### Using an Azure Durable Function

Review the [sample](/samples/azure/blob-storage-databus-cleanup-function/) to see how to use a durable function to clean up attachments.

### Using the Blob Lifecycle Management policy

Attachment blobs can be cleaned up using the [Blob Storage Lifecycle feature](https://docs.microsoft.com/en-us/azure/storage/blobs/storage-lifecycle-management-concepts). This method allows configuring a single policy for all data bus-related blobs. Those blobs can be either deleted or archived. The policy does not require custom code and is deployed directly to the storage account. This feature can only be used on GPv2 and Blob storage accounts, not on GPv1 accounts. 
