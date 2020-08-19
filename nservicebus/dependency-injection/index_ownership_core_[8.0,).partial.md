NServiceBus supports two modes of operation for containers, *internally managed* and *externally managed*.

## Internally managed mode

In *internally managed* mode, NServiceBus manages the entire lifecycle of the container, including registration, component resolution, and disposal.

include: internallymanagedcontainer

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
