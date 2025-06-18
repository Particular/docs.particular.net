---
title: Dependency Injection changes
summary: Dependency Injection changes from NServiceBus 7 to 8.
reviewed: 2022-03-24
component: Core
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
 - 8
---

NServiceBus no longer provides adapters for external dependency injection containers. Instead, NServiceBus version 8 provides the ability to use any container that conforms to the `Microsoft.Extensions.DependencyInjection` abstraction.

The following adapter packages will no longer be provided:

* [Autofac](/nservicebus/dependency-injection/autofac.md)
* [Castle](/nservicebus/dependency-injection/castlewindsor.md)
* [StructureMap](/nservicebus/dependency-injection/structuremap.md)
* [Ninject](/nservicebus/dependency-injection/ninject.md)
* [Unity](/nservicebus/dependency-injection/unity.md)

Instead of the container adapter packages, use the [NServiceBus.Extensions.Hosting](/nservicebus/hosting/extensions-hosting.md) package. To use a third party dependency injection container refer to the [externally managed container mode](/nservicebus/dependency-injection/#modes-of-operation-externally-managed-mode). See the [migrating to the Generic Host](#microsoft-generic-host) or [migrating to externally managed mode](#externally-managed-container-mode) sections for further information.

## Behavioral changes

The behavior has been aligned with the expectations of the `Microsoft.Extensions.DependencyInjection` package. The following changes have been introduced:

- The same component can be registered multiple times
- The last registration of the same component wins

## Property injection

Property injection is not covered by `Microsoft.Extensions.DependencyInjection`. Therefore, the NServiceBus default dependency injection container no longer supports property injection. Property injection might still be supported by the third party containers.

## UseContainer is deprecated

The `UseContainer` API to integrate third party containers with NServiceBus has been removed as it does not align with the `Microsoft.Extensions.DependencyInjection` model. To use a custom dependency injection container with NServiceBus, use the [externally managed container mode](/nservicebus/dependency-injection/#modes-of-operation-externally-managed-mode).

## RegisterComponents changes

The `EndpointConfiguration.RegisterComponents` API now provides access to the underlying `IServiceCollection`. The registration methods formerly provided by `IConfigureComponents` are available as extension methods to simplify migration. However, it is recommended to use the official `IServiceCollection` registration API instead.

NServiceBus `DependencyLifecycle` maps directly to ServiceDescriptor `ServiceLifetime`:

| DependencyLifecycle   | ServiceLifetime |
| --------------------- | --------------- |
| InstancePerCall       | Transient       |
| SingleInstance        | Singleton       |
| InstancePerUnitOfWork | Scoped          |

For example, this statement:

```csharp
endpointConfiguration.RegisterComponents(s =>
    s.ConfigureComponent<MyService>(DependencyLifecyle.InstancePerCall));
```

may be replaced with:

```csharp
endpointConfiguration.RegisterComponents(s => s.AddTransient<MyService>());
```

See the following table for recommended replacements. See the [IServiceCollection documentation](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection?view=dotnet-plat-ext-3.1) for a full list of available registration methods.

| `ConfigureComponents` usage                                               | Replacement                  |
| ------------------------------------------------------------------------- | ---------------------------- |
| `ConfigureComponent<MyService>(DependencyLifecyle.InstancePerCall)`       | `AddTransient<MyService>()`  |
| `ConfigureComponent<MyService>(DependencyLifecyle.SingleInstance)`        | `AddSingleton<MyService>()`  |
| `ConfigureComponent<MyService>(DependencyLifecyle.InstancePerUnitOfWork)` | `AddScoped<MyService>()`     |

The former `ConfigureComponents` would automatically register all interfaces of a given type. The `IServiceCollection.Add` method, however, would not do this and all the inherited interfaces must be registered explicitly. The registrations may be forwarded to the inheriting type, for example:

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

To host NServiceBus as part of the Generic Host, install the `NServiceBus.Extensions.Hosting` NuGet package and refer to the [documentation](/nservicebus/hosting/extensions-hosting.md) for further details. Refer to the container's documentation for configuration instructions. By default, the Generic Host uses the [Microsoft.Extensions.DependencyInjection] container. See the following examples on how to integrate common DI containers with the generic host. Other supported DI containers are listed in the [official documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?#default-service-container-replacement).

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

The externally managed container mode allows integrating third-party dependency injection containers, that conform to the `Microsoft.Extensions.DependencyInjection` model, with NServiceBus via the `EndpointWithExternallyManagedContainer.Create` API. See the following sections for examples of using common DI containers.

### Migrating to externally managed mode

#### Microsoft.Extensions.DependencyInjection

The following snippet configures NServiceBus to use the Microsoft's default container implementation.

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

With newer versions of Autofac the [ability to update an existing container](https://github.com/autofac/Autofac/issues/811) has been removed. The registration phase of the dependencies is now separate from the resolve phase.

Once a component has been resolved the container is immutable. For cases when an existing lifetime scope was passed to NServiceBus:

```csharp
endpointConfiguration.UseContainer<AutofacBuilder>(
    customizations: customizations =>
    {
        customizations.ExistingLifetimeScope(container);
    });
```

it is recommended to either:

* Move the registrations to the start of the application _or_
* Avoid building the container early by passing the container builder to the various bootstrapping parts of the application _or_
* Use a [dedicated child lifetime scope](https://autofac.readthedocs.io/en/latest/integration/netcore.html#using-a-child-scope-as-a-root) which allows accessing singletons of the root container while having dependencies managed by the service collection being "rooted" within the child scope.

#### StructureMap

> [!NOTE]
> StructureMap has been sunsetted. The maintainers recommend to use [Lamar](https://jasperfx.github.io/lamar/) instead.

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

The following snippet configures NServiceBus to use Unity as its dependency injection container. This requires the `Unity.Microsoft.DependencyInjection` NuGet package.

```csharp
var serviceCollection = new ServiceCollection();
var startableEndpoint = EndpointWithExternallyManagedContainer.Create(endpointConfiguration, serviceCollection);

var container = new UnityContainer();
var serviceProvider = container.BuildServiceProvider(serviceCollection);
var endpointInstance = await startableEndpoint.Start(serviceProvider);
```

#### Spring

The following snippet configures NServiceBus to use Spring as its dependency injection container. This requires the `Spring.Extensions.DependencyInjection` NuGet package.

```csharp
var serviceCollection = new ServiceCollection();
var startableEndpoint = EndpointWithExternallyManagedContainer.Create(endpointConfiguration, serviceCollection);

var applicationContext = new GenericApplicationContext();
var serviceProviderFactory = new SpringServiceProviderFactory(applicationContext);
var serviceProvider = serviceProviderFactory.CreateServiceProvider(serviceProviderFactory.CreateBuilder(serviceCollection));
var endpointInstance = await startableEndpoint.Start(serviceProvider);
```
