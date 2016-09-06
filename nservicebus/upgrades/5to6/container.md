---
title: Container Changes
tags:
 - upgrade
 - migration
related:
 - nservicebus/containers
---


## Configure type removed

In Version 5 the `Configure` type was used to provide runtime access to the local endpoint address, scanned types etc via dependency injection. In Version 6 these values can now be accessed as follows.


### Settings

Settings can be accessed via the `FeatureConfigurationContext`, see [features](/nservicebus/pipeline/features.md) for more details. Runtime access via the container is provided by taking a dependency on the `ReadOnlySettings` type.


### Builder

This is no longer supported. It is advised to, instead of using `IBuilder` directly, use dependency injection via the [container of choice](/nservicebus/containers/).


### Scanned types

Access to types found during [assembly scanning](/nservicebus/hosting/assembly-scanning.md) is provided via `Settings.GetAvailableTypes()`.


### Local address

Access to the [endpoint address](/nservicebus/endpoints/) is provided via `Settings.LocalAddress()`.


## Encryption Service

It is no longer possible to access the builder to create an encryption service. If container access is required use the container directly in the factory delegate in the `RegisterEncryptionService` method.


## Conventions

[Conventions](/nservicebus/messaging/conventions.md) are no longer be injected into the [Container](/nservicebus/containers/). Conventions need to be retrieved with `Settings.Get<Conventions>()` over `ReadOnlySettings`.


## Dependency injection

Explicitly setting property values via `.ConfigureProperty<T>()` and `.InitializeHandlerProperty<T>()` has been deprecated. Instead configure the properties explicitly using:

snippet: 5to6-ExplicitProperties


## IConfigureComponents no longer registered in the container

To access it at runtime create a new [`Feature`](/nservicebus/pipeline/features.md) and put the following code in the `.Setup` method

snippet: 5to6-IConfigureComponentsNotInjected