WARNING: The `IWantToRunWhenBusStartsAndStops` interface is no longer available.

When self-hosting, there are several options for equivalent behavior:

 * Writing code in the endpoint class after start and stop
 * [FeatureStartupTask](/nservicebus/pipeline/features.md#feature-startup-tasks)
 * [Using MEF or Reflection](/samples/plugin-based-config) to run code at startup and shutdown in a pluggable way.


## Achieving the IWantToRunWhenBusStartsAndStops when using the Hosts

 * In the [NServiceBus Host](/nservicebus/hosting/nservicebus-host/#when-endpoint-instance-starts-and-stops).
 * In the [AzureCloudService Host](/nservicebus/hosting/cloud-services-host/#when-endpoint-instance-starts-and-stops).
