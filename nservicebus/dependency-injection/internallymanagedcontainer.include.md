### Built-in default container

NServiceBus comes with a default internal container with a registration API for registration of user types. The following dependency lifecyles are supported.

#### Instance per call

A new instance will be returned for each call.

Represented by the enum value `DependencyLifecycle.InstancePerCall`.

snippet: InstancePerCall

or using a delegate:

snippet: DelegateInstancePerCall

#### Instance per unit of work

The instance will be a singleton for the duration of the [unit of work](/nservicebus/pipeline/unit-of-work.md). In practice this means the processing of a single transport message.

Represented by the enum value `DependencyLifecycle.InstancePerUnitOfWork`.

snippet: InstancePerUnitOfWork

or using a delegate:

snippet: DelegateInstancePerUnitOfWork

#### Single instance

The same instance will be returned each time.

Represented by the enum value `DependencyLifecycle.SingleInstance`.

WARNING: `SingleInstance` components that have dependencies that are scoped `InstancePerCall` or `InstancePerUnitOfWork` will still resolve. In effect these dependencies, while not scoped `SingleInstance`, will behave as if they are `SingleInstance` as the instances will exist inside the parent component.


snippet: SingleInstance

or using a delegate:

snippet: DelegateSingleInstance

or using the explict singleton API:

snippet: RegisterSingleton

### Using a third party container

NServiceBus but also supports the following third party containers:

* [Autofac](autofac.md)
* [CastleWindsor](castlewindsor.md)
* [Ninject](ninject.md)
* [SimpleInjector](simpleinjector.md)
* [Spring](spring.md)
* [StructureMap](structuremap.md)
* [Unity](unity.md)
* [Microsoft.Extensions.DependencyInjection](msdependencyinjection.md)

#### Plugging in other containers

If a specific library is not supported, create a plugin using the `IContainer` abstraction. Once this is created and registered, NServiceBus will use the custom dependency injection to look up its own dependencies.

Create a class that implements 'IContainer':

snippet: CustomContainer

Create a class that implements 'ContainerDefinition' and returns the 'IContainer' implementation:

snippet: CustomContainerDefinition

Then register the `ContainerDefinition` to be used:

snippet: CustomContainerUsage

### Injecting the endpoint instance

`IEndpointInstance` is not registered automatically and must be registered explicitly to be injected.

NOTE: In NServiceBus version 6 and above `IEndpointInstance` is not `IDisposable`.
