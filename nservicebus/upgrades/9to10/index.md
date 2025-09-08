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

NServiceBus 10 targets .NET 10 only. Go to [supported frameworks and platforms](/nservicebus/upgrades/supported-platforms.md) to learn more about the support timeline.

## Nullability

Progress has been made to enable nullability accross the API surface.

### Behavioral changes

#### ContextBag

The ContextBag no longer allows storing `null` as a value. In order to store `null` it is required to decorate the value with wrapper class. So instead of

```csharp
context.Set<string>("YourKey", null);
```

use

```csharp
context.Set<string>("YourKey", new YourKeyValue());

public record YourKeyValue(string? someValueThatMightBeNull = null);
```

### API changes

This is section is relevant for projects that have nullability enabled.

#### Inputs

The APIs below have been modified to accomodate to be able to receive nullable parameters.

| API |
|---|
|`sendOptions.StartNewConversation`|
|`CriticalError`|
|`endpointConfiguration.EnableInstallers`|
|`logger.Debug`|
|`logger.DebugFormat`|
|`logger.Error`|
|`logger.ErrorFormat`|
|`logger.Fatal`|
|`logger.FatalFormat`|
|`logger.Info`|
|`logger.InfoFormat`|
|`logger.Warn`|
|`logger.WarnFormat`|

#### Outputs

The APIs below have been modified to return nullable values.

| API |
|---|
|`routingOptions.GetDestination(this NServiceBus.ReplyOptions options)`|
|`routingOptions.GetDestination(this NServiceBus.SendOptions options)`|
|`routingOptions.GetReplyToRoute(this NServiceBus.ReplyOptions options)`|
|`routingOptions.GetReplyToRoute(this NServiceBus.SendOptions options)`|
|`routingOptions.GetRouteToSpecificInstance(this NServiceBus.SendOptions options)`|

## StartupDiagnosticEntry has required properties

The properties `Data` and `Name` of `StartupDiagnosticEntry` have been marked as required.

## DataBus feature moved to separate NServiceBus.ClaimCheck package

The DataBus feature has been removed from `NServiceBus.Core` and has released as a separate package, called [NServiceBus.ClaimCheck](https://www.nuget.org/packages/NServiceBus.ClaimCheck/).

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

## NServiceBus.Persistence changes

### ICompletableSynchronizedStorageSession and IOutboxTransaction implement IAsyncDisposable

`ICompletableSynchronizedStorageSession` and `IOutboxTransaction` implement `IAsyncDisposable` to better support asynchronous operations during the disposal of both types. For more information about IAsyncDisposable visit consult the [Implement a DisposeAsync guidelines](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-disposeasync).

## NServiceBus.Pipeline changes

### Nullable Inputs

The APIs below have been modified to accomodate to be able to receive nullable parameters.

| API |
|---|
|`context.TryGetIncomingPhysicalMessage`|

