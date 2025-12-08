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

## Extensibility

This section describes changes to advanced extensibility APIs.

### ContextBag can no longer store null values

As part of adding nullability annotations, the `ContextBag` class no longer allows storing `null` as a value. This also applies to all types derived from `ContextBag`, including all behavior context classes and `TransportTransaction`.

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

## Sagas

### Custom finders

In Version 10 [custom saga finders](/nservicebus/sagas/saga-finding) are no longer automatically registered via assembly scanning and must be mapped in `ConfigureHowToFindSaga`

```
protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
{
    mapper.ConfigureFinderMapping<MyMessage, MySagaFinder>();
}
```
