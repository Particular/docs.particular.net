---
title: Feature Changes in Version 6
tags:
 - upgrade
 - migration
---



## Removed FeaturesReport

`FeaturesReport` exposed reporting information about features of a running endpoint instance. It has been internalized. Similarly to previous versions the information is still available by inspecting the `DisplayDiagnosticsForFeatures` logger when the endpoint runs with log level [`DEBUG`](/nservicebus/logging/#logging-levels).


## [Feature Dependencies](/nservicebus/pipeline/features.md#dependencies)

Feature Dependencies, using the string API, now need to be declared using the target feature's full type name (`Type.FullName`) which includes the namespace. Removing the `Feature` suffix is no longer required.

snippet: 5to6-DependentFeature


## ISatellite and IAdvancedSatellite interfaces are obsolete

Both the `ISatellite` and the `IAdvancedSatellite` interfaces are deprecated. The same functionality is available via the `AddSatelliteReceiver` method on the context passed to the [features](/nservicebus/pipeline/features.md#feature-api) `Setup` method. The [satellite documentation](/nservicebus/satellites/) shows this in detail.