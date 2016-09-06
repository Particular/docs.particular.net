---
title: Configure type remove in Version 6
tags:
 - upgrade
 - migration
---


In Version 5 the `Configure` type was used to provide runtime access to the local endpoint address, scanned types etc via dependency injection. In Version 6 these values can now be accessed as follows.


## Settings

Settings can be accessed via the `FeatureConfigurationContext`, see [features](/nservicebus/pipeline/features.md) for more details. Runtime access via the container is provided by taking a dependency on the `ReadOnlySettings` type.


## Builder

This is no longer supported. It is advised to, instead of using `IBuilder` directly, use dependency injection via the [container of choice](/nservicebus/containers/).


## Scanned types

Access to types found during [assembly scanning](/nservicebus/hosting/assembly-scanning.md) is provided via `Settings.GetAvailableTypes()`.


## Local address

Access to the [endpoint address](/nservicebus/endpoints/) is provided via `Settings.LocalAddress()`.