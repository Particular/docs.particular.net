---
title: MongoDB Persistence Upgrade Version 4 to 5
summary: Migration instructions on how to upgrade to MongoDB Persistence version 5
reviewed: 2020-11-06
component: mongodb
related:
- persistence/mongodb
isUpgradeGuide: true
---

## Upgraded the MongoDB.Driver to version 3

The [MongoDB.Driver](https://www.nuget.org/packages/MongoDB.Driver) introduces [breaking changes](https://www.mongodb.com/docs/drivers/csharp/current/upgrade/v3/), such as enforcing the `GuidRepresentationMode.V3` to be the only supported mode which affects the storing and loading of saga data. The persistence has been updated internally to accommodate these changes and uses this default mode unless explicitely configured.

The enforcement for choosing a GUID representation mode has been introduced in the previous versions of the client. For saga data that requires to remain backward compatible it is necessary to choose the GUID representation mode explicitely either on a global level by overriding the GuidSerializer or by adjusting the class mappings.

Here are a few examples to indicate some of the possible options. It is necessary to evaluate those options on a case by case basis to make sure previously stored sagas can still be retrieved. To learn more about serializing GUIDs in the .NET/C# Driver, see the [GUIDs page](https://www.mongodb.com/docs/drivers/csharp/current/fundamentals/serialization/guid-serialization/#std-label-csharp-guids).

### Switching the mode on a global level

If most of your GUIDs use the same representation, you can register a GuidSerializer globally. To create and register a `GuidSerializer`, run the following code early in your application, such as during the bootstrapping phase:

```csharp
BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.CSharpLegacy));
```

The above example configures the legacy GUID representation mode.

### Taking full control over the saga data via class mapping

```csharp
var classMap = new BsonClassMap(typeof(SagaData));
classMap.MapIdProperty(nameof(SagaData.Id))
    .SetElementName("_id")
    .SetSerializer(new GuidSerializer(GuidRepresentation.CSharpLegacy));
...
classMap.SetIgnoreExtraElements(true);
```

### Taking full control over the saga data via class mapping and attributes

```csharp
class SagaData : IContainSagaData
{
    [BsonGuidRepresentation(GuidRepresentation.CSharpLegacy)]
    [BsonElement("_id")]
    public Guid Id { get; set; }
    public string Originator { get; set; }
    public string OriginalMessageId { get; set; }
}

var classMap = new BsonClassMap(typeof(SagaData));
classMap.AutoMap();
classMap.SetIgnoreExtraElements(true);

BsonClassMap.RegisterClassMap(classMap);
```

### Representing GUIDs as strings

Alternatively it is supported to represent GUIDs as strings. This option was previously not available and may only be used for new sagas should you wish to represent the saga IDs as strings.

```
var classMap = new BsonClassMap(typeof(SagaData));
classMap.MapIdProperty(nameof(SagaData.Id))
    .SetElementName("_id")
    // This instructs MongoDB to treat the Guid as a string in the database and not longer
    // as a binary value with a subtype, which is the default behavior
    .SetSerializer(new GuidSerializer(BsonType.String));
...
classMap.SetIgnoreExtraElements(true);

BsonClassMap.RegisterClassMap(classMap);
```
