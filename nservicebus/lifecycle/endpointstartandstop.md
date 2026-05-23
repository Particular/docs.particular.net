---
title: When Endpoint Instance Starts and Stops
summary: How to hook into the startup and shutdown sequence of an endpoint instance.
reviewed: 2026-05-23
component: core
redirects:
 - nservicebus/lifecycle/iwanttorunwhenbusstartsandstops
---

There are several options available to execute custom code as part of the endpoint's startup/shutdown sequence:

* [Using the Generic Host lifecycle](https://learn.microsoft.com/en-us/dotnet/core/extensions/generic-host?tabs=appbuilder#ihostapplicationlifetime) to run tasks during the application's lifecycle.
* Adding a custom [FeatureStartupTask](/nservicebus/pipeline/features.md#feature-startup-tasks).

> [!WARNING]
> The `IWantToRunWhenBusStartsAndStops` interface is no longer available as part of the `NServiceBus` package. It has been replaced by [`IWantToRunWhenEndpointStartsAndStops`](/nservicebus/hosting/nservicebus-host/#when-endpoint-instance-starts-and-stops).
