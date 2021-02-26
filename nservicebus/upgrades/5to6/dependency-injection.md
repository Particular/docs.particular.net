---
title: Dependency Injection Changes in NServiceBus Version 6
reviewed: 2020-05-07
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

In NServiceBus version 5, the `Configure` type is used to provide runtime access to the local endpoint address, scanned types, etc, via dependency injection. In version 6, these values are accessed as follows:

### Immutable

The default container used internally is immutable once the endpoint is started.

### Settings

Settings can be accessed via the `FeatureConfigurationContext`, see [Features](/nservicebus/pipeline/features.md) for more details. Runtime access via dependency injection is provided by taking a dependency on the `ReadOnlySettings` type.


### Builder

This is no longer supported in NServiceBus version 6. Instead of using `IBuilder` directly, use a specific [dependency injection](/nservicebus/dependency-injection/) library.


### Scanned types

Access to types found during [assembly scanning](/nservicebus/hosting/assembly-scanning.md) is provided via `Settings.GetAvailableTypes()`.


### Local address

Access to the [endpoint address](/nservicebus/endpoints/) is provided via `Settings.LocalAddress()`.


## Encryption service

It is no longer possible to access the builder to create an encryption service. If dependency injection access is required, use it directly in the factory delegate in the `RegisterEncryptionService` method.


## Conventions

[Conventions](/nservicebus/messaging/conventions.md) are no longer [injected](/nservicebus/dependency-injection/). Conventions must be retrieved with `Settings.Get<Conventions>()` over `ReadOnlySettings`.


## Dependency injection

Explicitly setting property values via `.ConfigureProperty<T>()` and `.InitializeHandlerProperty<T>()` is deprecated in NServiceBus version 6. Instead configure the properties explicitly using:

snippet: 5to6-ExplicitProperties


## IConfigureComponents no longer registered

To access `IConfigureComponents` at runtime, create a new [feature](/nservicebus/pipeline/features.md) and put the following code in the `.Setup` method

snippet: 5to6-IConfigureComponentsNotInjected


## Instances passed to the configuration API are no longer disposed

```csharp
var builder = new ContainerBuilder();
builder.RegisterInstance(new MyService());
var container = builder.Build();
endpointConfiguration.UseContainer<AutofacBuilder>(
    customizations: customizations =>
    {
        customizations.ExistingLifetimeScope(container);
    });
```

While the above example shows how an existing DI instance can be passed into the configuration API using Autofac, the same behavior can be applied to all of the [currently supported dependency injection](/nservicebus/dependency-injection/). Previous versions of DI treated the externally passed-in DI instance as if it were owned by the DI adapter and disposed the DI when the bus was disposed. This behavior changed in NServiceBus version 6. When a DI customization is passed in (as in the above example), then the DI instance is no longer disposed. It is the responsibility of the DI owner to dispose the DI instance. 

```csharp
endpointConfiguration.UseContainer<AutofacBuilder>();
```

The above example shows how a new DI instance can be assigned using the configuration API. While the DI instance used in this example is Autofac, the same behavior can be applied to all of the [currently supported dependency injection](/nservicebus/dependency-injection/). When passing a new DI instance, the endpoint owns that instance and will also be responsible for disposing it when endpoint stopped.
