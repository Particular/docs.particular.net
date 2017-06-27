A short explanation of each:

 *  Logs will be sent to the `Trace` infrastructure, which should have been configured with Azure diagnostic monitor trace listener by the Visual Studio tooling.
 * `UseTransport<AzureStorageQueueTransport>`: Sets [Azure storage queues](/transports/azure-storage-queues/) as the [transport](/transports).
 * `UsePersistence<AzureStoragePersistence>`: Configures [Azure storage](/persistence/azure-storage/) for [persistence](/persistence).