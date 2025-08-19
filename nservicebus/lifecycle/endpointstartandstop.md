---
title: When Endpoint Instance Starts and Stops
summary: How to hook into the startup and shutdown sequence of an endpoint instance.
reviewed: 2025-07-22
component: core
related:
 - samples/startup-shutdown-sequence
redirects:
 - nservicebus/lifecycle/iwanttorunwhenbusstartsandstops
---

There are several options available to execute custom code as part of the endpoint's startup/shutdown sequence:

* Writing code directly after calling `Endpoint.Start` or `endpointInstance.Stop`.
* Adding a custom [FeatureStartupTask](/nservicebus/pipeline/features.md#feature-startup-tasks).
* [Using the Generic Host lifecycle](https://learn.microsoft.com/en-us/dotnet/core/extensions/generic-host?tabs=appbuilder#ihostapplicationlifetime) to run tasks during the application's lifecycle.

> [!WARNING]
> The `IWantToRunWhenBusStartsAndStops` interface is no longer available as part of the `NServiceBus` package. It has been replaced by [`IWantToRunWhenEndpointStartsAndStops`](/nservicebus/hosting/nservicebus-host/#when-endpoint-instance-starts-and-stops).
