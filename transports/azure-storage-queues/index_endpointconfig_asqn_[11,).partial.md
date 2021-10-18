To use Azure Storage Queues as the underlying transport configure it as follows:

snippet: AzureStorageQueueTransportWithAzure

Then set up appropriate [connection strings](/transports/azure-storage-queues/configuration.md#connection-strings).

{{NOTE: `UseTransport(transport)` is a new style of message transport configuration in NServiceBus version 8 which makes it more explicit which transport configuration properties are required.

This differs in style from the `UseSerialization<T>()` and `UseSerialization<T>()` APIs, which will eventually be updated to the new style as well. While these APIs are being changed, the older `UseTransport<T>()` style of transport configuration that uses extension methods to configure transport settings can still be used. For details of these settings, refer to the documentation for the previous version.
}}