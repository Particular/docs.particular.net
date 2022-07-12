There are several options available to execute custom code as part of the endpoint's startup/shutdown sequence:

* Writing code directly after calling `Endpoint.Start` or `endpointInstance.Stop`.
* Adding a custom [FeatureStartupTask](/nservicebus/pipeline/features.md#feature-startup-tasks).
* [Using MEF or Reflection](/samples/plugin-based-config) to run code at startup and shutdown in a pluggable way.

WARNING: The `IWantToRunWhenBusStartsAndStops` interface is no longer available as part of the `NServiceBus` package.


## Hooking into endpoint lifecycle when using the hosts

Some of the described approaches are not available when hosting an endpoint via the `NServiceBus.Host` or `NServiceBus.Hosting.Azure` packages. Visit their respective documentation pages for more information on how to hook into the startup/shutdown sequence:

 * In the [NServiceBus Host](/nservicebus/hosting/nservicebus-host/#when-endpoint-instance-starts-and-stops).
 * In the [AzureCloudService Host](/nservicebus/hosting/cloud-services-host/#when-endpoint-instance-starts-and-stops).
