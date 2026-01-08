---
title: Upgrade Version 9 to 10
summary: Instructions on how to upgrade NServiceBus from version 9 to version 10.
reviewed: 2025-09-03
component: Core
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 9
 - 10
---

include: upgrade-major

## .NET target framework

The minimum version of .NET that can be used with NServiceBus 10 is .NET 10.

## DataBus feature moved to separate NServiceBus.ClaimCheck package

The DataBus feature has been removed from the main NServiceBus package and has been released as a separate package, called [NServiceBus.ClaimCheck](https://www.nuget.org/packages/NServiceBus.ClaimCheck/).

The namespace for the DataBus feature has changed from `NServiceBus.DataBus` to `NServiceBus.ClaimCheck`. The API has also been updated to use the term ClaimCheck instead of DataBus.

The table below shows the mapping from the DataBus configuration types to their ClaimCheck equivalents.

| DataBus feature | NServiceBus.ClaimCheck |
| --- | --- |
| `EndpointConfiguration.UseDataBus` | `EndpointConfiguration.UseClaimCheck` |
| `NServiceBus.FileShareDataBus` | `NServiceBus.FileShareClaimCheck` |
| `NServiceBus.SystemJsonDataBusSerializer` | `NServiceBus.SystemJsonClaimCheckSerializer` |
| `NServiceBus.DataBusProperty<T>` | `NServiceBus.ClaimCheckProperty<T>` |

### Migrating message contracts

The NServiceBus.ClaimCheck library is line-level compatible with original DataBus feature, meaning, in-flight messages that are sent using DataBus will be properly handled by endpoints that have been upgraded to use NServiceBus.ClaimCheck; this is also true in reverse.

Some care should be taken when migrating message contracts from `DataBusProperty<T>` to `ClaimCheckProperty<T>`. While DataBus and NServiceBus.ClaimCheck are line-level compatible, they are not runtime compatible. An endpoint that is currently using the DataBus feature will not write properties that are `ClaimCheckProperty<T>` to the DataBus. The reverse is true of NServiceBus.ClaimCheck endpoints and `DataBusProperty<T>`.  To facilitate the migration, each endpoint will need a copy of the message contract that uses the supported property type.

Changing from using `DataBusProperty<T>` to specifying conventions for the claim check properties will be the easiest way to migrate whilst maintaining runtime compatibility between the new and old versions. If this is not possible, the message contracts can be versioned separately too.

If message contracts are in a versioned library that has been migrated to `ClamCheckProperty<T>`, then DataBus endpoints can remain on an older version of the contracts library until they can be upgraded to NServiceBus.ClaimCheck.

If message contracts are not in a versioned library, a local copy of the messages can be made to facilitate the transition. In this case it is imperative that all class names, namespaces, and property names are exactly the same to make sure the message can be properly deserialized when it is received.

## Sagas

### Not found handlers

In Version 10 the `IHandleSagaNotFound` interface has been deprecated in favour of `ISagaNotFoundHandler`. The [saga not found handlers](/nservicebus/sagas/saga-not-found.md) are no longer automatically registered via assembly scanning and must be mapped in the `ConfigureHowToFindSaga` method of the sagas that require the not found handler to be executed:

```csharp
protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
{
    mapper.ConfigureNotFoundHandler<MyNotFoundHandler>();
}
```

### Custom finders

In Version 10 [custom saga finders](/nservicebus/sagas/saga-finding.md) are no longer automatically registered via assembly scanning and must be mapped in the `ConfigureHowToFindSaga` method:

```csharp
protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
{
    mapper.ConfigureFinderMapping<MyMessage, MySagaFinder>();
}
```

Not having a finder configured for a given message will result in:

- **When the message is allowed to start the saga** - Compile time analyzer error [NSB0006](/nservicebus/sagas/analyzers.md#message-that-starts-the-saga-does-not-have-a-message-mapping)
- **When the message is not allowed to start the saga** - `Exception` when processing the message: `Message type CompletePaymentTransaction is handled by saga OrderSaga, but the saga does not contain a property mapping or custom saga finder to map the message to saga data. Consider adding a mapping in the saga's ConfigureHowToFindSaga method`

## Deprecated `IWantToRunBeforeConfigurationIsFinalized`

The extension point [`IWantToRunBeforeConfigurationIsFinalized`](/nservicebus/lifecycle/iwanttorunbeforeconfigurationisfinalized.md) is deprecated with a warning in NServiceBus version 10 and will be removed in NServiceBus version 11.

Final adjustments to settings before configuration is finalized should be applied via an explicit last configuration step on the endpoint configuration, instead of via implementations of this interface discovered by scanning.

## Deprecated `ExecuteTheseHandlersFirst`

Most of the time, any given message is only handled by one message handler or saga, but it is possible for multiple handlers to apply for a message type, especially when considering message types that have an inheritance hierarchy. In these cases, the handlers would be called one after the other, but not necessarily in a deterministic order, unless `ExecuteTheseHandlersFirst` specified the order:

```csharp
// Before NServiceBus 10
endpointConfiguration.ExecuteTheseHandlersFirst(typeof(FirstHandler), typeof(SecondHandler));
```

Sometimes, handler order needed to be set to guarantee that some infrastructure task occurred, such as logging. In current versions of NServiceBus, these infrastructure tasks should be replaced with [pipeline behaviors](/nservicebus/pipeline/manipulate-with-behaviors.md). In addition to the docs, more information on pipeline behaviors can be found in the blog post [Infrastructure soup](https://particular.net/blog/infrastructure-soup).

Other examples that previously required `ExecuteTheseHandlersFirst` involved multiple handlers reacting to different types in a message inheritance hierarchy, for example with each handler doing part of the work on a data object that is loaded once using the [identity map pattern](https://en.wikipedia.org/wiki/Identity_map_pattern) and then persisted once incorporating the changes from both handlers.

In these cases, replace the call to `ExecuteTheseHandlersFirst` with `AddHandler` or `AddSaga`. The handlers will be invoked in registration order. Any handlers later added during [assembly scanning](/nservicebus/hosting/assembly-scanning.md) will continue to be invoked in a non-deterministic order after the explicitly registered handlers.

```csharp
endpointConfiguration.AddSaga<SagaGoesFirst>();
endpointConfiguration.AddHandler<ThisHandlerNext>();
endpointConfiguration.AddHandler<ThenThisHandler>();
// Assembly scanned handlers go last
```

## Service registration changes

NServiceBus version 10 changes how infrastructure components are registered and resolved. Infrastructure types such as handlers, sagas, behaviors, installers and custom checks that users should never directly resolve are no longer registered in the service collection, while still supporting dependency injection for their own dependencies.

This change aligns with fundamental principles in software architecture:

- The [Dependency Inversion Principle (DIP)](https://en.wikipedia.org/wiki/Dependency_inversion_principle) states that high-level modules (business logic) should not depend on low-level modules (framework infrastructure). Both should depend on abstractions.
- The [Hollywood Principle](https://en.wiktionary.org/wiki/Hollywood_principle) describes the relationship between frameworks and application code. NServiceBus calls handlers and sagas when messages arrive. Application code should never call handlers directly.

When infrastructure components are registered in the service collection, they become part of the application's public API. This creates a problem: developers can accidentally depend on framework internals that should be implementation details, and it becomes tempting to use the Service Locator anti-pattern (resolving from `IServiceProvider` directly) instead of proper constructor injection.

ASP.NET Core follows these same principles. Controllers are not registered in the service collection by default, yet they fully support dependency injection. This is the right architectural boundary: the framework manages its own infrastructure while users manage their application services.

### Changes required

If both of the following conditions are true of all handlers, sagas, behaviors, etc. then **no action is needed**, and they will continue to work exactly as before with full dependency injection support:

* Not resolved directly using `GetService<T>()` or similar
* Not used as a constructor injection argument in another class

If one of these conditions is not true, the code needs to be refactored. This pattern was never intended and indicates logic that should be extracted into proper services.

Instead of:

```csharp
// Treating a handler like a service
var handler = serviceProvider.GetService<MyHandler>();
handler.DoSomething(); // This confuses application and framework layers
```

…adjust the code to be structured similar to:

```csharp
// Extract the logic to a proper application service
public interface IMyBusinessLogic
{
    void DoSomething();
}

public class MyBusinessLogic : IMyBusinessLogic
{
    public void DoSomething() { /* ... */ }
}

// Register it as an application service
services.AddSingleton<IMyBusinessLogic, MyBusinessLogic>();

// Use it in the handler (framework layer)
public class MyHandler : IHandleMessages<MyMessage>
{
    private readonly IMyBusinessLogic _businessLogic;

    public MyHandler(IMyBusinessLogic businessLogic)
    {
        _businessLogic = businessLogic;
    }

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        _businessLogic.DoSomething();
        return Task.CompletedTask;
    }
}

// Resolve and use the service
var businessLogic = serviceProvider.GetService<IMyBusinessLogic>();
businessLogic.DoSomething();
```

### Further benefits of the change

By keeping infrastructure components out of the service collection, clean separation is maintained:

- Framework layer: Handlers, sagas, behaviors managed by NServiceBus
- Application layer: Services, abstractions, and business logic managed by user code

This separation makes it impossible to accidentally create the wrong kind of coupling in the codebase.

This design aligns with how modern .NET frameworks handle infrastructure:

- ASP.NET Core doesn't register controllers by default
- Blazor doesn't register components
- Minimal APIs don't register endpoint handlers

NServiceBus now follows the same established patterns.

For those using [`ValidateOnBuild`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.serviceprovideroptions.validateonbuild) or [`ValidateScopes`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.serviceprovideroptions.validatescopes) on their service collection, having fewer registrations provides concrete benefits:

- Faster validation: Fewer services to validate means quicker startup times
- Fewer false positives: Infrastructure types often have complex lifetime requirements that can cause validation to fail even though NServiceBus manages them correctly
- More reliable builds: Validation focuses on application services where issues are more likely to occur
- Clearer diagnostics: When validation does fail, it's about application services, not framework internals

## Extensibility

This section describes changes to advanced extensibility APIs.

### ContextBag can no longer store null values

As part of adding nullability annotations, the `ContextBag` class no longer allows storing `null` as a value. This also applies to all types derived from `ContextBag`, including all behavior context classes and `TransportTransaction`.

### Features

In a future version of NServiceBus, [`Feature` classes](/nservicebus/pipeline/features.md) will not be automatically discovered by runtime assembly scanning. Instead, each feature should be explicitly enabled for an endpoint.

The preferred method of distributing a feature is to create an extension method on `EndpointConfiguration`, and enable the feature within that extension method:

```csharp
public static class MyFeatureConfigurationExtensions
{
    public static void EnableMyFeature(this EndpointConfiguration config)
    {
        config.EnableFeature<MyFeature>();
    }
}
```

The `EnableByDefault()` method is only used to signal that a Feature identified through assembly scanning should turn itself on by default, so this method is deprecated with a warning in NServiceBus version 10 to give advance notice of this change, and will be removed in version 11.

In addition, APIs that work with features using a `Type` have been deprecated, and instead the generic-typed variants should be used. For example:

```csharp
// Instead of:
endpointConfiguration.EnableFeature(typeof(MyFeature));
endpointConfiguration.DisableFeature(typeof(MyFeature));
// Use instead:
endpointConfiguration.EnableFeature<MyFeature>();
endpointConfiguration.DisableFeature<MyFeature>();
```

Lastly, any `Feature` classes must now have a paramaterless constructor.

### Installers

Like features, [installer classes](/nservicebus/operations/installers.md) which implement `INeedToInstallSomething` will not be automatically discovered by runtime assembly scanning in a future version of NServiceBus.

Instead, installers should be explicitly registered either from an `EndpointConfiguration` or inside a feature's `Setup` method:

```csharp
endpointConfiguration.AddInstaller<CreateMyInfrastructure>();

public class MyFeature : Feature
{
	protected override void Setup(FeatureConfigurationContext context)
	{
		context.AddInstaller<CreateMyInfrastructure>();
	}
}
```

### StartupDiagnosticEntry has required properties

As part of adding nullability annotations, the `Data` and `Name` properties of the `StartupDiagnosticEntry` class have been marked as `required`.

### ICompletableSynchronizedStorageSession and IOutboxTransaction implement IAsyncDisposable

`ICompletableSynchronizedStorageSession` and `IOutboxTransaction` implement `IAsyncDisposable` to better support asynchronous operations during the disposal of both types. For more information about IAsyncDisposable visit consult the [Implement a DisposeAsync guidelines](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-disposeasync).

### PersistenceDefinition

#### Definition factory

Any persistence definition must explicitly implement `IPersistenceDefinitionFactory<TDefinition>` to enable NServiceBus to create the persistence implementation without using reflection.

```csharp
public class CustomPersistence : PersistenceDefinition
{
    internal CustomPersistence() { }
}
````

must be changed to

```csharp
public class CustomPersistence : PersistenceDefinition, IPersistenceDefinitionFactory<CustomPersistence>
{
    internal CustomPersistence() { }

    static CustomPersistence IPersistenceDefinitionFactory<CustomPersistence>.Create() => new();
}
```

#### Supports

The `Supports<TStorageType>(…)` no longer accepts an action that gets access to the `SettingsHolder` in order to more directly identify the feature to activate. Calls to  `Supports<TStorageType>(…)` like this:

```csharp
public class CustomPersistence : PersistenceDefinition
{
    internal CustomPersistence()
    {
        Supports<StorageType.Sagas>(s => s.EnableFeatureByDefault<SagaStorage>());
        Supports<StorageType.Subscriptions>(s => s.EnableFeatureByDefault<SubscrptionStorage>());
        Supports<StorageType.Outbox>(s => s.EnableFeatureByDefault<OutboxStorage>());
    }
}
```

must be changed to `Supports<TStorageType, TFeatureType>()` like this:

```csharp
public class CustomPersistence : PersistenceDefinition
{
    internal CustomPersistence()
    {
        Supports<StorageType.Sagas, SagaStorage>();
        Supports<StorageType.Subscriptions, SubscrptionStorage>();
        Supports<StorageType.Outbox, OutboxStorage>();
    }
}
```

