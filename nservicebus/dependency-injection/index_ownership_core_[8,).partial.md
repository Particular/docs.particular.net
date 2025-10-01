NServiceBus supports two modes of operation for containers, *internally managed* and *externally managed*.
NServiceBus uses [Microsoft.Extensions.DependencyInjection (MS.DI)](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection) as the default recommended dependency injection container.

> [!TIP]
> For most scenarios, use the built-in MS.DI container. Only use a third-party container if you require features not available in MS.DI.

NServiceBus automatically registers and invokes message handlers, sagas, and other user-provided extension points using a dependency injection container.

## Modes of operation

NServiceBus supports two modes for dependency injection:

- **Internally managed:** NServiceBus manages the container lifecycle as part of the endpoint lifecycle.
- **Externally managed:** The application or host provides and controls the container lifecycle.

Understanding the different modes is only required in custom hosting scenarios or when using third-party container integrations. Applications using the [generic host](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host) or the [web application builder](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/webapplication) should leverage [NServiceBus.Extensions.Hosting](/nservicebus/hosting/extensions-hosting.md) which takes care of the necessary plumbing automatically.

### Internally managed mode

NServiceBus creates and manages the `IServiceCollection` and `IServiceProvider` including proper disposal. Register your services using the standard MS.DI API exposed over the `RegisterComponents` API on the endpoint configuration:

```csharp
endpointConfiguration.RegisterComponents(services =>
{
    services.AddTransient<MyService>();
    services.AddScoped<MyScopedService>();
    services.AddSingleton<MySingletonService>();
});
```

See [MS.DI lifetimes](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection#service-lifetimes) and [service registrations](#service-registrations) for more details.

The internally managed mode is intended to be used in scenarios where the NServiceBus endpoint is the entry point of the application or where multiple endpoints are hosted within the same application and each endpoint requires an isolated container instance.

### Externally managed mode

> [!WARNING]
> Every NServiceBus endpoint requires its own dependency injection container. Sharing containers across multiple endpoints results in conflicting registrations and might cause incorrect behavior or runtime errors.

If full control over the container, including its lifetime and disposal, is needed, use externally managed mode:

During the registration phase, an instance of `IServiceCollection` is passed to the `EndpointWithExternallyManagedContainer.Create` method. The following snippets show how to use Microsoft's default implementation from the `Microsoft.Extensions.DependencyInjection` NuGet package:

snippet: ExternalPrepare

Later, during the resolution phase, the `Start` method requires an instance of `IServiceProvider`.

snippet: ExternalStart

> [!NOTE]
> Refer to the container's documentation on how to use the container with the `Microsoft.Extensions.DependencyInjection` API.

The externally managed mode is intended to be used in scenarios where the NServiceBus endpoint integrates into an existing application or host already using either MS.DI or using a third party container that supports integration with MS.DI.

## Property injection

Property injection is **not supported** by default with MS.DI. Use constructor injection for all dependencies. If you require property injection, refer to your container's documentation.

## Using third-party containers

If features are needed that are not available in MS.DI, it is possible to integrate a third-party container that supports the MS.DI abstractions. See [Externally managed mode](#modes-of-operation-externally-managed-mode) on how to use the externally managed mode to seperate the registration phase from the resolving phase on the container and consult the third-party container documentation on how to integrate the container into MS.DI.

The NServiceBus Version 7 to 8 upgrade guide also contains snippets hinting at the custom configuration potentially required to integrate with [Autofac, Lamar, Unity, StructureMap, Castle Windows, or Spring](/nservicebus/upgrades/7to8/dependency-injection.md#externally-managed-container-mode-migrating-to-externally-managed-mode).

For leveraging third-party containers with `NServiceBus.Extensions.Hosting`, refer to the [configure custom containers documentation](/nservicebus/hosting/extensions-hosting.md#dependency-injection-integration-configure-custom-containers) for further details.

## Service registrations

Custom services may be registered using the `IServiceCollection` API.

#### Instance per call

A new instance will be returned for each call.

Represented by the enum value `ServiceLifetime.Transient`.

snippet: InstancePerCall

or using a delegate:

snippet: DelegateInstancePerCall

#### Instance per unit of work

The instance will be a singleton for the duration of the [unit of work](/nservicebus/pipeline/unit-of-work.md). In practice this means the processing of a single transport message.

Represented by the enum value `ServiceLifetime.Scoped`.

snippet: InstancePerUnitOfWork

or using a delegate:

snippet: DelegateInstancePerUnitOfWork

#### Single instance

The same instance will be returned each time.

Represented by the enum value `ServiceLifetime.Singleton`.

> [!WARNING]
> `Singleton` components with dependencies that are scoped `Transient` or `Scoped` will still resolve. In effect, these dependencies, while not scoped as `Singleton`, will behave as if they are `Singleton` because the instances will exist inside the parent component.

snippet: SingleInstance

or using a delegate:

snippet: DelegateSingleInstance

## Injecting the message session

`IMessageSession` is not registered automatically in the container and must be registered explicitly to be injected. Access to the session is provided via `IStartableEndpointWithExternallyManagedContainer.MessageSession`

> [!NOTE]
> The session is only valid for use after the endpoint have been started, so it is provided as `Lazy<IMessageSession>`.

## Resolving dependencies

It is recommended to follow the [dependency injection guidelines](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-guidelines) for .NET. Be aware of the following special cases with NServiceBus:

- [Injecting dependencies into behaviors](/nservicebus/pipeline/manipulate-with-behaviors.md)
