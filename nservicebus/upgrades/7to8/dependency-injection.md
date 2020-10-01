---
title: Dependency Injection changes
summary: Dependency Injection changes from NServiceBus 7 to 8.
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

Instead of the container adapter packages, use the [NServiceBus.Extensions.Hosting](/nservicebus/hosting/extensions-hosting.md) package or the [externally managed container mode](/nservicebus/dependency-injection/#externally-managed-mode) to use a third party dependency injection container. See the [migrating to the Generic Host](#microsoft-generic-host) or [migrating to externally managed mode](#externally-managed-container-mode) sections for further information.

## Property injection

Property injection is not covered by `Microsoft.Extensions.DependencyInjection.Abstractions`. Therefore, the NServiceBus default dependency injection container no longer supports property injection. Property injection might still be supported by third party containers.

## UseContainer is deprecated

The `UseContainer` API to integrate third party containers with NServiceBus has been removed as it does not align with the `Microsoft.Extensions.DependencyInjection.Abstractions` model. To use a custom dependency injection container with NServiceBus, use the [externally managed container mode](/nservicebus/dependency-injection/#externally-managed-mode).

## RegisterComponents changes

The `EndpointConfiguration.RegisterComponents` API now provides access to the underlying `IServiceCollection`. The registration methods formerly provided by `IConfigureComponents` are available as extension methods to simplify migration. However, it is recommended to use the official `IServiceCollection` registration API instead. 

NServiceBus `DependencyLifecycle` maps directly to ServiceDescriptor `ServiceLifetime`:

| DependencyLifecycle   | ServiceLifetime |
| --------------------- | --------------- |
| InstancePerCall       | Transient       |
| SingleInstance        | Singleton       |
| InstancePerUnitOfWork | Scoped          |

For example, this statement:

```
endpointConfiguration.RegisterComponents(s => 
    s.ConfigureComponent<MyService>(DependencyLifecyle.InstancePerCall));
```

may be replaced with:

```
endpointConfiguration.RegisterComponents(s => s.AddTransient<MyService>());
```

The former `ConfigureComponents` automatically registered all interfaces of a given type. The `IServiceCollection.Add` methods do not do this. Any inherited interfaces must be registered explicitly. For example, their registrations may be forwarded to the inheriting type:

```
endpointConfiguration.RegisterComponents(s => {
    s.AddSingleton<MyService>(); // MyService implements both IServiceA and IServiceB interfaces
    s.AddSingleton<IServiceA>(serviceProvider => serviceProvider.GetService<MyService>());
    s.AddSingleton<IServiceB>(serviceProvider => serviceProvider.GetService<MyService>());
});
```

## Microsoft Generic Host

NServiceBus integrates with the [Microsoft Generic Host](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host) and automatically uses the host's managed dependency injection container. Most third party dependency injection containers support the Generic Host.

### Migrating to the Generic Host

To host NServiceBus as part of the Generic Host, install the `NServiceBus.Extensions.Hosting` NuGet package and refer to the [documentation](/nservicebus/hosting/extensions-hosting.md) for further details. Refer to the container's documentation for configuration instructions. By default, the Generic Host uses the [Microsoft.Extensions.DependencyInjection] container. See the following examples on how to integrate common DI containers with the generic host. Other supported DI containers are listed in the [official documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1#default-service-container-replacement).

#### Autofac

The snippet shows how to configure Autofac using the `Autofac.Extensions.DependencyInjection` NuGet package:

```csharp
Host.CreateDefaultBuilder(args)
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .UseNServiceBus(...)
    ...
    .Build();
```

#### Lamar

The snippet shows how to configure Lamar using the `Lamar.Microsoft.DependencyInjection` NuGet package:

```csharp
Host.CreateDefaultBuilder(args)
    .UseLamar()
    .UseNServiceBus(...)
    ...
    .Build();
```

#### Unity

The snippet shows how to configure Unity using the `Unity.Microsoft.DependencyInjection` NuGet package:

```csharp
Host.CreateDefaultBuilder(args)
    .UseUnityServiceProvider()
    .UseNServiceBus(...)
    ...
    .Build();
```

## Externally managed container mode

The externally managed container mode allows integrating third-party dependency injection containers, that conform to the `Microsoft.Extensions.DependencyInjection.Abstractions` model, with NServiceBus via the `EndpointWithExternallyManagedContainer.Create` API. See the following sections for examples of using common DI containers.

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

Note: StructureMap has been sunsetted. The maintainers recommend to use [Lamar](https://jasperfx.github.io/lamar/) instead.

The following snippet configures NServiceBus to use StructureMap as its dependency injection container. This requires the `StructureMap.Microsoft.DependencyInjection` NuGet package.

```csharp
var serviceCollection = new ServiceCollection();
var startableEndpoint = EndpointWithExternallyManagedContainer.Create(endpointConfiguration, serviceCollection);

var container = new Container();
container.Populate(serviceCollection);
var endpointInstance = await startableEndpoint.Start(container.GetInstance<IServiceProvider>());
```


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
