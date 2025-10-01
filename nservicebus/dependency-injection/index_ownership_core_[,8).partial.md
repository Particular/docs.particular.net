NServiceBus automatically registers and invokes message handlers, sagas, and other user-provided extension points using a dependency injection container.

## Modes of operation

NServiceBus supports two modes for dependency injection:

- **Internally managed:** NServiceBus manages the container lifecycle as part of the endpoint lifecycle.
- **Externally managed:** The application or host provides and controls the container lifecycle.

## Internally managed mode

In *internally managed* mode, NServiceBus manages the entire lifecycle of the container, including registration, component resolution, and disposal.

include: internallymanagedcontainer

## Externally managed mode

In *externally managed* mode, NServiceBus registers its components in the container but does not own the container's lifecycle. The container is provided by the user in two phases, one for registration (`IConfigureComponents`) and one for resolution (`IBuilder`).

> [!WARNING]
> Every NServiceBus endpoint requires its own dependency injection container. Sharing containers across multiple endpoints results in conflicting registrations and might cause incorrect behavior or runtime errors.

During the registration phase, an instance of `IConfigureComponents` is passed to the `EndpointWithExternallyManagedContainer.Create` method. For example, for Autofac's `ContainerBuilder`, this is the phase during which its type registration methods would be called.

snippet: ExternalPrepare

Later, during the resolution phase, the `Start` method requires an instance of `IBuilder`. At this stage, the container has already been initialized with all its registrations. For example, for Autofac's `ContainerBuilder`, this is the phase during which its `Build` method would be called.

snippet: ExternalStart

> [!NOTE]
> The `Adapt` methods are implemented by the user and are container-specific. [NServiceBus.Extensions.DependencyInjection](/nservicebus/dependency-injection/extensions-dependencyinjection.md) supports externally managed mode using `Microsoft.Extensions.DependencyInjection` abstractions (`IServiceCollection` and `IServiceProvider`) that are supported by most dependency injection containers.

### Injecting the message session

`IMessageSession` is not registered automatically in the container and must be registered explicitly to be injected. Access to the session is provided via `IStartableEndpointWithExternallyManagedContainer.MessageSession`

> [!NOTE]
> The session is only valid for use after the endpoint have been started, so it is provided as `Lazy<IMessageSession>`.
