A short explanation of each:

 * `AzureConfigurationSource`: overrides any settings known to the NServiceBus Azure configuration section within the app.config file with settings from the service configuration file.
 *  Logs will be sent to the `Trace` infrastructure, which should have been configured with Azure diagnostic monitor trace listener by the Visual Studio tooling.
 * `UseTransport<AzureStorageQueueTransport>`: Sets [Azure storage queues](/transports/azure-storage-queues/) as the [transport](/transports).
 * `UsePersistence<AzureStoragePersistence>`: Configures [Azure storage](/persistence/azure-table/) for [persistence](/persistence).
