NServiceBus supports two modes of operation for containers, *internally managed* and *externally managed*.

## Internally managed mode

In *internally managed* mode, NServiceBus manages the entire lifecycle of the container, including registration, component resolution, and disposal.

### Built-in default container

NServiceBus has a built-in default container that supports the `Microsoft.Extensions.DependencyInjection.Abstractions` model. Custom services may be registered using the `IServiceCollection` API.

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

WARNING: `Singleton` components with dependencies that are scoped `Transient` or `Scoped` will still resolve. In effect, these dependencies, while not scoped as `Singleton`, will behave as if they are `Singleton` because the instances will exist inside the parent component.

snippet: SingleInstance

or using a delegate:

snippet: DelegateSingleInstance

### Using a third party containers

third party or custom dependency injection containers can be used via the [externally managed mode](#externally-managed-mode).


## Externally managed mode

In *externally managed* mode, NServiceBus registers its components in the container but does not own the container's lifecycle. NServiceBus uses the `Microsoft.Extensions.DependencyInjection.Abstractions` API to integrate with third party containers.

WARN: Every NServiceBus endpoint requires its own dependency injection container. Sharing containers across multiple endpoints results in conflicting registrations and might cause incorrect behavior or runtime errors.

During the registration phase, an instance of `IServiceCollection` is passed to the `EndpointWithExternallyManagedContainer.Create` method. The following snippets show how to use Microsoft's default implementation from the `Microsoft.Extensions.DependencyInjection` NuGet package:

snippet: ExternalPrepare

Later, during the resolution phase, the `Start` method requires an instance of `IServiceProvider`.

snippet: ExternalStart

NOTE: Refer to the container's documentation on how to use the container with the `Microsoft.Extensions.DependencyInjection.Abstractions` API.

### Injecting the message session

`IMessageSession` is not registered automatically in the container and must be registered explicitly to be injected. Access to the session is provided via `IStartableEndpointWithExternallyManagedContainer.MessageSession`

Note: The session is only valid for use after the endpoint have been started, so it is provided as `Lazy<IMessageSession>`.

[This sample](/samples/dependency-injection/extensions-dependency-injection/) demonstrates how to register the message session.

## Microsoft Generic Host

When hosting NServiceBus with the [Microsoft Generic Host](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host) using the `NServiceBus.Extensions.Hosting` package, refer to the [configure custom containers documentation](/nservicebus/hosting/extensions-hosting.md#dependency-injection-integration-configure-custom-containers) for further details.
