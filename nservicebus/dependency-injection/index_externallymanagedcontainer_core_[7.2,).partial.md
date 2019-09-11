## Externally managed mode

In *externally managed* mode, NServiceBus registers its components in the container but does not own the container's lifecycle. The container is passed by the user via two different abstractions, one for the registration phase (`IConfigureComponents`) and one for the resolution phase (`IBuilder`).

First, the `EndpointWithExternallyManagedContainer.Create` method requires an instance of `IConfigureComponents`. At this stage the container is in the registration phase (e.g. Autofac's `ContainerBuilder`).

snippet: ExternalPrepare

Second, the `Start` method requires an instance of `IBuilder`. At this stage the container has already been initialized will all its registrations.

snippet: ExternalStart

NOTE: The `Adapt` methods need to be provided by the user and are container-specific. See the [ASP.NET Core sample](/samples/dependency-injection/aspnetcore/) to see how these methods are implemented based on the ASP.NET Core Dependency Injection abstractions.
