---
title: NServiceBus.Extensions.DependencyInjection
summary: Provides integration with the Microsoft.Extensions.DependencyInjection abstraction
reviewed: 2020-06-18
component: Extensions.DependencyInjection
related:
 - samples/dependency-injection/extensions-dependency-injection
 - samples/hosting/generic-host
 - samples/netcore-reference
redirects:	
 - nservicebus/dependency-injection/property-injection
 - nservicebus/property-injection-in-handlers
---

WARN: Starting with NServiceBus version 8, the `NServiceBus.Extensions.DependencyInjection` package is no longer required. NServiceBus directly supports the `Microsoft.Extensions.DependencyInjection` model via the [externally managed container mode](/nservicebus/dependency-injection/#externally-managed-mode). Visit the [dependency injection upgrade guide](/nservicebus/upgrades/7to8/dependency-injection.md) for further information.

The `NServiceBus.Extensions.DependencyInjection` package provides integration with the `Microsoft.Extensions.DependencyInjection` dependency injection abstraction.

NOTE: It's recommended to use [Microsoft's generic host](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host) to manage application and dependency injection container lifecycle. Use the [NServiceBus.Extensions.Hosting](/nservicebus/hosting/extensions-hosting.md) package to host an NServiceBus endpoint with the generic host.


## Usage with ServiceCollection

The following snippet shows how to configure NServiceBus to use Microsoft's built-in dependency injection container:

snippet: usecontainer-servicecollection


## Usage with third party containers

NServiceBus can be configured to work with any third party dependency injection container which implements the `Microsoft.Extensions.DependencyInjection` abstraction. To use a third-party dependency injection container, pass the specific container's `IServiceProviderFactory` to the `UseContainer` configuration method. The following snippet shows this approach, using Autofac (`Autofac.Extensions.DependencyInjection`) as an example:

snippet: usecontainer-thirdparty


## Configuring the container

`UseContainer` provides a settings class which gives advanced configuration options:


### IServiceCollection access

The settings provide access to the underlying `IServiceCollection` that can be used to add additional service registrations.

snippet: settings-servicecollection


### ContainerBuilder

Third-party container-native APIs can be accessed by with `ConfigureContainer`.

snippet: settings-configurecontainer


## DependencyLifecycle mapping

[`DependencyLifecycle`](/nservicebus/dependency-injection/) maps to [ServiceLifetime](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.servicelifetime) as follows:

| `DependencyLifecycle`                                                                                             | Service Lifetime                                                                                                        |
|-----------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------|
| [InstancePerCall](/nservicebus/dependency-injection/) | [ServiceLifetime.Transient](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.servicelifetime)         |
| [InstancePerUnitOfWork](/nservicebus/dependency-injection/)                    | [ServiceLifetime.Scoped](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.servicelifetime) |
| [SingleInstance](/nservicebus/dependency-injection/)                                  | [ServiceLifetime.Singleton](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.servicelifetime)                          |

## Property injection

The `NServiceBus.Extensions.DependencyInjection` package does not support property injection out of the box. To enable property injection, refer to the configured container's documentation.


## Externally managed mode

The package allows the container to be used in [externally managed mode](/nservicebus/dependency-injection/#externally-managed-mode) for full control of the dependency injection container via the `EndpointWithExternallyManagedServiceProvider` extension point:

snippet: externally-managed-mode

WARN: `IServiceCollection` and `IServiceProvider` instances must not be shared across mutliple NServiceBus endpoints to avoid conflicting registration that might cause incorrect behavior or runtime errors.