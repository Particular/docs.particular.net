---
title: Dependency Injection changes in Version 6
reviewed: 2016-10-26
component: Core
related:
 - nservicebus/dependency-injection
redirects:
 - nservicebus/upgrades/5to6/container
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---


## Configure type removed

In Version 5 the `Configure` type was used to provide runtime access to the local endpoint address, scanned types etc via dependency injection. In Version 6 these values can now be accessed as follows:


### Settings

Settings can be accessed via the `FeatureConfigurationContext`, see [features](/nservicebus/pipeline/features.md) for more details. Runtime access via the container is provided by taking a dependency on the `ReadOnlySettings` type.


### Builder

This is no longer supported. It is advised to, instead of using `IBuilder` directly, use a specific [dependency injection](/nservicebus/dependency-injection/) library.


### Scanned types

Access to types found during [assembly scanning](/nservicebus/hosting/assembly-scanning.md) is provided via `Settings.GetAvailableTypes()`.


### Local address

Access to the [endpoint address](/nservicebus/endpoints/) is provided via `Settings.LocalAddress()`.


## Encryption Service

It is no longer possible to access the builder to create an encryption service. If container access is required use the container directly in the factory delegate in the `RegisterEncryptionService` method.


## Conventions

[Conventions](/nservicebus/messaging/conventions.md) are no longer be injected into the [Container](/nservicebus/dependency-injection/). Conventions need to be retrieved with `Settings.Get<Conventions>()` over `ReadOnlySettings`.


## Dependency injection

Explicitly setting property values via `.ConfigureProperty<T>()` and `.InitializeHandlerProperty<T>()` has been deprecated. Instead configure the properties explicitly using:

snippet: 5to6-ExplicitProperties


## IConfigureComponents no longer registered in the container

To access it at runtime create a new [`Feature`](/nservicebus/pipeline/features.md) and put the following code in the `.Setup` method

snippet: 5to6-IConfigureComponentsNotInjected


## Containers passed to the configuration API are no longer disposed

snippet: 5-to-6-autofac-existing

While the above example shows how an existing container can be passed into the configuration API using Autofac, the same behavior can also be applied to all of the [currently supported container adapters](/nservicebus/dependency-injection/). Previous versions of the containers treated the externally passed in container as if it was owned by the container adapter and disposed the container when the bus was disposed. This behavior changed. When a container customization is passed like above, then the container is no longer disposed. It is the responsibility of the container owner to dispose the container. 

snippet: 5-to-6-autofac

The above example shows how a new container can be assigned using the Configuration API. While the container used in this example is Autofac, the same behavior can also be applied to all of the [currently supported container adapters](/nservicebus/dependency-injection/). When passing a new container, the endpoint owns the container and will also be responsible for disposing the container when it is stopped.