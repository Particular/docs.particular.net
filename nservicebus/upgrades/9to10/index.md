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

### Custom finders

In Version 10 [custom saga finders](/nservicebus/sagas/saga-finding.md) are no longer automatically registered via assembly scanning and must be mapped in the `ConfigureHowToFindSaga` method:

```
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

