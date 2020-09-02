---
title: Dependency Injection Changes
summary: Dependency injection changes from NServiceBus 7 to 8.
reviewed: 2020-02-20
component: Core
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
 - 8
---

Support for external dependency injection containers is no longer provided by NServiceBus adapters for each container library. Instead, NServiceBus version 8 directly provides the ability to use any container that conforms to the `Microsoft.Extensions.DependencyInjection.Abstractions` container abstraction.

The following adapter packages will no longer be provided:

* [Autofac](/nservicebus/dependency-injection/autofac.md)
* [Castle](/nservicebus/dependency-injection/castlewindsor.md)
* [StructureMap](/nservicebus/dependency-injection/structuremap.md)
* [Ninject](/nservicebus/dependency-injection/ninject.md)
* [Unity](/nservicebus/dependency-injection/unity.md)

Instead of the container adapter packages, use the [externally managed container mode](/nservicebus/dependency-injection#externally-managed-mode) to use a third party dependency injection container. See the [migrating to externally managed mode](#externally-managed-container-mode-migrating-to-externally-managed-mode) section for examples using common dependency injection containers.

## Property injection

Property injection is not covered by `Microsoft.Extensions.DependencyInjection.Abstractions`, therefore the NServiceBus default dependency injection container does not support property injection anymore. Property injection might be supported by third party containers that can be enabled using the [externally managed container mode](/nservicebus/dependency-injection#externally-managed-mode).

## UseContainer is deprecated

The `UseContainer` API to integrate third party containers with NServiceBus has been removed as it does not align with the `Microsoft.Extensions.DependencyInjection.Abstractions` model. To use a custom dependency injection container with NServiceBus, use the [externally managed container mode](/nservicebus/dependency-injection#externally-managed-mode).

## Externally managed container mode

The externally managed container mode allows to integrate third party dependency injection containers with NServiceBus that conform to the `Microsoft.Extensions.DependencyInjection.Abstractions` model via the `EndpointWithExternallyManagedContainer.Create` API. See the following sections for examples using common DI containers.

### Migrating to externally managed mode

#### Microsoft.Extensions.DependencyInjection

The following snippet configures NServiceBus to use the Microsoft's default container implementation. This requires the `Microsoft.Extensions.DependencyInjection` NuGet package.

```csharp
var serviceCollection = new ServiceCollection();
var startableEndpoint = EndpointWithExternallyManagedContainer.Create(endpointConfiguration, serviceCollection);

var endpoint = await startableEndpoint.Start(serviceCollection.BuildServiceProvider());
```

#### Autofac

The following snippet configures NServiceBus to use Autofac as its dependency injection container. This requires the `Autofac.Extensions.DependencyInjection` NuGet package.

```csharp
var serviceCollection = new ServiceCollection();
var startableEndpoint = EndpointWithExternallyManagedContainer.Create(endpointConfiguration, serviceCollection);

var containerBuilder = new ContainerBuilder();
containerBuilder.Populate(serviceCollection);
var autofacContainer = containerBuilder.Build();
var endpointInstance = await startableEndpoint.Start(new AutofacServiceProvider(autofacContainer));
```

#### StructureMap

The following snippet configures NServiceBus to use StructureMap as its dependency injection container. This requires the `StructureMap.Microsoft.DependencyInjection` NuGet package.

```csharp
var serviceCollection = new ServiceCollection();
var startableEndpoint = EndpointWithExternallyManagedContainer.Create(endpointConfiguration, serviceCollection);

var container = new Container();
container.Populate(serviceCollection);
var endpointInstance = await startableEndpoint.Start(container.GetInstance<IServiceProvider>());
```

Note: StructureMap has been sunsetted. The maintainers recommend to use [Lamar](https://jasperfx.github.io/lamar/) instead.

#### Castle Windsor

The following snippet configures NServiceBus to use Castle Windsor as its dependency injection container. This requires the `Castle.Windsor.MsDependencyInjection` NuGet package.

```csharp
var serviceCollection = new ServiceCollection();
var startableEndpoint = EndpointWithExternallyManagedContainer.Create(endpointConfiguration, serviceCollection);

var container = new WindsorContainer();
var serviceProvider = WindsorRegistrationHelper.CreateServiceProvider(container, serviceCollection);
var endpointInstance = await startableEndpoint.Start(serviceProvider);
```

#### Unity

The following snippet configures NServiceBus to use Unity as its dependency injection container. This requires the `Castle.Windsor.MsDependencyInjection` NuGet package.

```csharp
var serviceCollection = new ServiceCollection();
var startableEndpoint = EndpointWithExternallyManagedContainer.Create(endpointConfiguration, serviceCollection);

var container = new WindsorContainer();
var serviceProvider = WindsorRegistrationHelper.CreateServiceProvider(container, serviceCollection);
var endpointInstance = await startableEndpoint.Start(serviceProvider);
```