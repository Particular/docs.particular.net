NServiceBus supports two modes of operation for containers, *internally managed* and *externally managed*.

## Internally managed mode

In *internally managed* mode, NServiceBus manages the entire lifecycle of the container including registration, component resolution, and disposal.

include: internallymanagedcontainer

## Externally managed mode

In *externally managed* mode, NServiceBus registers its components in the container but does not own the container's lifecycle. The container is provided by the user in two phases, one for registration (`IConfigureComponents`) and one for resolution (`IBuilder`).

During the registration phase an instance of `IConfigureComponents` is passed to the `EndpointWithExternallyManagedContainer.Create` method. For example, for Autofac's `ContainerBuilder`, this is the phase during which its type registration methods would be called.

snippet: ExternalPrepare

Later, during the resolution phase, the `Start` method requires an instance of `IBuilder`. At this stage the container has already been initialized will all its registrations. For example, for Autofac's `ContainerBuilder`, this is the phase during which its `Build` method would be called.

snippet: ExternalStart

NOTE: The `Adapt` methods are implemented by the user and are container-specific. See the [ASP.NET Core sample](/samples/dependency-injection/aspnetcore/) to see how these methods are implemented based on the ASP.NET Core dependency injection abstractions.
