## Using a non-default container

NServiceBus can use a non-default container in two modes, *external* and *internal*.


## Non-default external container


In the *external* mode NServiceBus registers its components in the external container but does not own the lifecycle of that external container. In order to use use an external container, it has to provide two adapters -- one for the registration phase (`IConfigureComponents`) and one for the resolution phase (`IBuilder`). These abstractions are kept separate to enforce the separation between phases. 

First, the `Prepare` method requires an instance of `IConfigureComponents`. At this stage the external container is in the registration phase (e.g. Autofac's `ContainerBuilder`).

snippet: ExternalPrepare

Second, the `Start` method requires an instance of `IBuilder`. At this stage the external container has already been initialized will all its registrations. 

snippet: ExternalStart

NOTE: The `Adapt` methods need to be provided by the user and are container-specific. See the [ASP.NET Core sample](/samples/dependency-injection/aspnetcore/) to see how these methods are implemented based on the ASP.NET Core Dependency Injection abstractions.

## Non-default internal container

In the *internal* mode NServiceBus is fully managing the container's creation and lifetime. 