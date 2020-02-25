---
title: NServiceBus.Extensions.DependencyInjection
summary: Provides integration with the Microsoft.Extensions.DependencyInjection abstraction.
reviewed: 2020-02-17
component: Extensions.DependencyInjection
tags:
 - Dependency Injection
related:
 - samples/dependency-injection/extensions-dependency-injection
redirects:	
 - nservicebus/dependency-injection/property-injection
 - nservicebus/property-injection-in-handlers
---

The `NServiceBus.Extensions.DependencyInjection` package provides integration with the `Microsoft.Extensions.DependencyInjection` dependency injection abstraction.


## Usage with ServiceCollection

The following snippet shows how to configure NServiceBus to use Microsoft's built-in dependency injection container:

snippet: usecontainer-servicecollection


## Usage with third party containers

NServiceBus can also be configured to work with any third party dependency injection container which implements the `Microsoft.Extensions.DependencyInjection` abstraction. To use a third-party dependency injection container, pass the specific container's `IServiceProviderFactory` to the `UseContainer` configuration method. The following snippet shows this approach, using Autofac (`Autofac.Extensions.DependencyInjection`) as an example:

snippet: usecontainer-thirdparty


## Configuring the container

`UseContainer` provides a settings class which gives advanced configuration options:


### IServiceCollection access

The settings provide access to the underlying `IServiceCollection` that can be used to add additional service registrations.

snippet: settings-servicecollection


### ContainerBuilder

Third-party container-native APIs can be accessed by with `ConfigureContainer`.

snippet: settings-configurecontainer


## Property Injection

The `NServiceBus.Extensions.DependencyInjection` package does not support property injection out of the box. To enable property injection, refer to the configured container's documentation.


## Externally managed mode

The package allows the container to be used in [externally managed mode](/nservicebus/dependency-injection/#externally-managed-mode) for full control of the dependency injection container via the `EndpointWithExternallyManagedServiceProvider` extension point:

snippet: externally-managed-mode
