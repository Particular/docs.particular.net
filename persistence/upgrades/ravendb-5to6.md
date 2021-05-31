---
title: RavenDB Persistence Upgrade from 5 to 6
summary: Instructions on how to upgrade NServiceBus.RavenDB 5 to 6
component: Raven
related:
 - nservicebus/upgrades/6to7
reviewed: 2021-05-31
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---

This update supports upgrading to RavenDB 4.2 while using NServiceBus 7.x. The upgrade to RavenDB 4 is a **major** upgrade. It's not backwards compatible, primarily because the [RavenDB API has a lot of breaking changes](https://ravendb.net/docs/article-page/4.2/csharp/migration/client-api/introduction). Additionally, the data storage format has changed, so databases need to have their [data migrated to a new RavenDB 4 database](https://ravendb.net/docs/article-page/4.2/csharp/migration/server/data-migration).

## Required RavenDB client

RavenDB Persistence requires [RavenDB.Client 4.2.0](https://www.nuget.org/packages/RavenDB.Client/4.2.0) or later. RavenDB 4.0 and 4.1 [are not LTS releases](https://ravendb.net/support) and are not supported.


## Supported cluster configurations

include: cluster-configuration-warning

## Storage format for sagas

RavenDB 4 only allows a string to be used as a document id. However, saga data (inheriting `ContainSagaData` or implementing `IContainSagaData`) has an `Id` property of type `Guid`, which is not compatible. To account for this, sagas stored by NServiceBus.RavenDB version 6 and above will be wrapped by a `SagaDataContainer`:

```cs
class SagaDataContainer
{
    public string Id { get; set; }
    public string IdentityDocId { get; set; }
    public IContainSagaData Data { get; set; }
}
```

Sagas stored in the old "unwrapped" format will be automatically converted to the new format the first time they are updated.


## Custom saga finders no longer supported

Because of the change in saga data storage format and the need to convert previously stored sagas on demand, [saga finders](/nservicebus/sagas/saga-finding.md) are no longer supported in RavenDB Persistence.


## Connect only with a `DocumentStore`

Due to the changes in how RavenDB accepts connection information in RavenDB 4.0, NServiceBus now requires connection information to be provided in the form of a `DocumentStore` in code.

These methods of supplying the connection information are now deprecated and will throw a `NotImplementedException` if used:

* `ConnectionParameters`
* Connection strings
* Default document store at `http://localhost:8080`

See [Connection Options](/persistence/ravendb/connection.md?version=raven_6) for supported connection options in NServiceBus.RavenDB version 6.


## Subscription versioning options removed

The following methods are deprecated and will throw a `NotImplementedException` if used:

* `persistence.DisableSubscriptionVersioning()`
* `persistence.UseLegacyVersionedSubscriptions()`

Subscription versioning now does not include the message assembly version _by default_. Systems using the `DisableSubscriptionVersioning()` method (the new default) can safely remove this call as it is no longer needed.

See the [subscription versioning for NServiceBus.RavenDB version 5](/persistence/ravendb/subscription-versioning.md?version=raven_4) for more details.


## Legacy document conventions cannot be used

In this version, RavenDB Persistence will only use the document id conventions specified by the DocumentStore. **It is possible that a system will be using legacy or custom conventions, which must be supplied or the documents will not be found.**

If using `persistence.DoNotUseLegacyConventionsWhichIsOnlySafeForNewEndpoints()` in the endpoint, then default conventions are already in use and no conversion is needed. This method call can be removed, as it is deprecated and will throw a `NotImplementedException`.

### Converting an existing database

These are abbreviated instructions. See the [background information below](ravendb-5to6.md#legacy-document-conventions-cannot-be-used-background) for more info.

First, find the list of **Collections** in the metadata document:

1. [Migrate the database to RavenDB 4.2](https://ravendb.net/docs/article-page/4.2/csharp/migration/server/data-migration).
2. Locate the converted documents with a prefix of `NServiceBus/DocumentCollectionNames/`. If multiple such documents exist, locate the document containing an `EndpointName` property matching the desired endpoint.
3. The collection names are contained in an array under the `Collections` key.

For each collection name:

1. If the collection name is a simple pluralization of the class name, it can be ignored as it matches RavenDB's default naming convention.
    * Examples: `TimeoutDatas`, `OrderSagaDatas`
2. For all remaining collection names, create a rule that will map the class names to the collection names.

As an example, consider a collection names document like the following. The comments show the real class name.

```json
{
    "EndpointName": "FakeEndpointName",
    "Collections": [
        "TimeoutDatas",
        "OrderSagaDatas",  // class name == OrderSagaData
        "ShippingSaga",    // class name == ShippingSagaData
        "ShippingPolicy"   // class name == ShippingPolicy
    ],
    "@metadata": {}
}
```

In this case, `TimeoutDatas` and `OrderSagaDatas` match the pluralization rule and don't need the rule to fix them. However, `ShippingSaga` and `ShippingPolicy` do not fit the standard pluralization of their class names `ShippingSagaData` and `ShippingPolicy`, respectively, and need a mapping to continue to function.

The following snippet will create a document id convention that will allow the older documents to be loaded by RavenDB Persistence:

snippet: 5to6-LegacyDocumentIdConventions

Since conversion to RavenDB 4.x requires downtime for data migration anyway, it's always preferable to perform several test runs to ensure that older documents can be loaded correctly.


### Background

RavenDB's default document naming strategy, used for timeouts and sagas is to pluralize the class name:

* `OrderSagaData` => _OrderSagaDatas/{GUID}_
* `ShippingStatus` => _ShippingStatuses/{GUID}_
* `UserStory` => _UserStories/{GUID}_

NServiceBus.RavenDB version 2 and below changed the convention, in only some cases, to use the bare class name, further clipping off `Data` for saga data types:

```cs
// Behavior from NServiceBus.RavenDB 2.0
static string LegacyFindTypeTagName(Type type)
{
    var tagName = type.Name;

    if (IsASagaEntity(type))
    {
        tagName = tagName.Replace("Data", string.Empty);
    }

    return tagName;
}
```

This introduced a bug, which was fixed in NServiceBus.RavenDB version 2.2.0 by introducing a system document with id `NServiceBus/DocumentCollectionNames/{SHA1HashOfEndpointName}` containing the collection names in use in the current database by inspecting the built-in `Raven/DocumentsByEntityName` index.

In RavenDB 4 and above, there is no default `Raven/DocumentsByEntityName` index, and creating a new one would create an unnecessary indexing burden on the system, so the conventions must be specified manually if necessary, as shown above.


## Legacy Outbox document id format no longer used

In versions as far back as 2.2.3, the document id format for Outbox records was changed to include the endpoint name so that multiple endpoints wouldn't conflict with each other when cleaning up their outbox data. New outbox records were always stored with the updated naming convention.

Since outbox data is only persisted for a period of days, and the conversion has been contained in multiple versions over the course of years, the conversion is no longer necessary and has been removed in 6.0.


## No longer converting legacy timeouts

In NServiceBus versions 3 and 4, RavenDB persistence was part of the main NServiceBus library, and `TimeoutData` had a different class and namespace name. All versions of the NServiceBus.RavenDB package includes a converter that changes the class name on deserialization, causing automatic conversions of timeouts as they expire. Timeouts that have been generated with NServiceBus versions 3 and 4 that are still present at the time of the upgrade with a lapse date in the future must be converted to the new format manually. 

### Patching legacy timeouts

The following patch can be used to update all timeout documents to the new format.

```javascript
from TimeoutDatas  
update {
    if(!this.Destination.Queue) {
        return;
    }
    if(!this.Destination.Machine) {
        this.Destination = this.Destination.Queue
    }
    else {
        this.Destination = this.Destination.Queue + "@" + this.Destination.Machine
    }
}
```

Only timeout documents with the old format are modified. Timeout documents already in the new format are not modified.

For example

```json
{
    "Destination": {
        "Queue": "Queue",
        "Machine": "Machine"
    },
    "SagaId": "00000000-0000-0000-0000-000000000000",
    ...
```

will be patched to

```json
{
    "Destination": "Queue@Machine",
    "SagaId": "00000000-0000-0000-0000-000000000000",
    ...
```

The patch can be either applied in RavenDB management studio under Documents\Patch as shown in [Documents and Collections](https://ravendb.net/docs/article-page/4.2/csharp/studio/database/documents/documents-and-collections)
 or by using the [client patch API.](https://ravendb.net/docs/article-page/4.2/csharp/client-api/operations/patching/set-based). Endpoints can continue to run while applying the patch.

When the endpoint tries to process a timeout that is stored with the old timeout format, the following exception is thrown.

```
System.InvalidOperationException: Could not convert document TimeoutDatas/Number to entity of type NServiceBus.TimeoutPersisters.RavenDB.TimeoutData ---> Newtonsoft.Json.JsonSerializationException: Cannot deserialize the current JSON object (e.g. {"name":"value"}) into type 'System.String' because the type requires a JSON primitive value (e.g. string, number, boolean, null) to deserialize correctly.
...
```

When this exception is thrown, the timeout document is not removed from the database and will be picked up on the next timeout persister query interval. Applying the patch to the database will allow the timeout persister to read the timeout document and correctly process the message. Restarting the endpoint will force the timeout persister to deliver all outstanding timeout messages once the patch has been applied.

Additionally, when the exception is thrown, a copy of the message is sent to the error queue. These are grouped together in ServicePulse under the exception type `System.InvalidOperationException: Newtonsoft.Json.Serialization.JsonSerializerInternalReader.CreateObject(JsonReader reader, Type objectType, JsonContract contract, JsonProperty member, JsonContainerContract containerContract, JsonProperty containerMember, Object existingValue)`. These messages have not been deleted from the endpoint timeout persistence and should not be retried from ServicePulse. Once the patch has been applied to the endpoint timeout persistence, this group of failed messages may be [deleted](/servicepulse/intro-archived-messages.md).

## Updated .NET Framework versions

Because the [RavenDB.Client 4.2.0 NuGet package](https://www.nuget.org/packages/RavenDB.Client/4.2.0) supports only `netstandard20` and `netcoreapp2.1`, RavenDB persistence now supports the following frameworks:

* .NET Core 2.0 and above
* .NET Framework 4.7.2 and above
* .NET Framework 4.6.1, although [there are several issues with consuming .NET Standard libraries from .NET 4.6.1](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) and Microsoft recommends upgrading to .NET Framework 4.7.2 or higher.
