---
title: MongoDB Persistence Upgrade Version 4 to 5
summary: Migration instructions on how to upgrade to MongoDB Persistence version 5
reviewed: 2025-06-06
component: mongodb
related:
- persistence/mongodb
isUpgradeGuide: true
---

## Upgraded the MongoDB.Driver to version 3

The [MongoDB.Driver](https://www.nuget.org/packages/MongoDB.Driver) introduces [breaking changes](https://www.mongodb.com/docs/drivers/csharp/current/upgrade/v3/), such as enforcing the `GuidRepresentationMode.V3` to be the only supported mode which affects the storing and loading of saga data. The persistence has been updated internally to accommodate these changes and uses this default mode unless explicitly configured otherwise.

The enforcement for choosing a GUID representation mode has been introduced in the previous versions of the client. For saga data that requires backward compatibility, it is necessary to choose the GUID representation mode explicitly. This is achieved either on a global level, by overriding the `GuidSerializer`, or by adjusting the class mappings.

The following sections demonstrate a few examples to indicate some of the possible options. It is necessary to evaluate those options on a case-by-case basis to make sure previously stored sagas can still be retrieved. To learn more about serializing GUIDs in the .NET/C# Driver, see the [GUIDs page](https://www.mongodb.com/docs/drivers/csharp/current/fundamentals/serialization/guid-serialization/#std-label-csharp-guids).

### Switching the mode on a global level

If most of your GUIDs use the same representation, you can register a GuidSerializer globally. To create and register a `GuidSerializer`, run the following code early in your application, such as during the bootstrapping phase:

```csharp
BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.CSharpLegacy));
```

The above example configures the legacy GUID representation mode.

### Switching the mode on a mapping level

Assuming the following saga data as a baseline

```csharp
public class OrderSagaData : ContainSagaData
{
    public Guid OrderId { get; set; }

    public string OrderDescription { get; set; }
}
```

#### Overriding the base class map

```csharp
BsonClassMap.RegisterClassMap<ContainSagaData>(m =>
{
    m.SetIsRootClass(true);
    m.MapIdProperty(s => s.Id)
        .SetElementName("_id")
        .SetSerializer(new GuidSerializer(GuidRepresentation.CSharpLegacy));
    m.AutoMap();
    m.SetIgnoreExtraElements(true);
});
```

#### Overriding the saga map

```csharp
BsonClassMap.RegisterClassMap<OrderSagaData>(m =>
{
    m.MapProperty(s => s.OrderId)
        .SetSerializer(new GuidSerializer(GuidRepresentation.CSharpLegacy));
    m.AutoMap();
    m.SetIgnoreExtraElements(true);
});
```

#### Overriding with attributes

When using attributes, it is necessary to directly implement `IContainSagaData` since `ContainsSagaData` is persistence-agnostic and therefore doesn't contain the necessary attributes to mark the saga ID as a document ID.

```csharp
#pragma warning disable NSB0012
class SagaData : IContainSagaData
#pragma warning restore NSB0012
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.CSharpLegacy)]
    [BsonElement("_id")]
    public Guid Id { get; set; }
    public string Originator { get; set; }
    public string OriginalMessageId { get; set; }

    [BsonGuidRepresentation(GuidRepresentation.CSharpLegacy)]
    public Guid OrderId { get; set; }
    public string OrderDescription { get; set; }
}
```

#### Representing GUIDs as strings

Alternatively, GUIDs can be represented as strings. This option was previously not available and may only be used for new sagas should you wish to represent the saga IDs as strings.

```csharp
BsonClassMap.RegisterClassMap<ContainSagaData>(m =>
{
    m.SetIsRootClass(true);
    m.MapIdProperty(s => s.Id)
        .SetElementName("_id")
        .SetSerializer(new GuidSerializer(BsonType.String));
    m.AutoMap();
    m.SetIgnoreExtraElements(true);
});
```

### A GuidSerializer using the Unspecified representation is already registered

In certain cases, the persistence may raise the following exception at startup:

```text
A GuidSerializer using the Unspecified representation is already registered which indicates the default serializer has already been used. Register the GuidSerializer with the preferred representation before using the mongodb client as early as possible.
```

This exception indicates that the GuidSerializer with the Unspecified representation has already been used. This can be caused by incorrect order of class mappings. For example, instead of declaring the class map as follows

```csharp
BsonClassMap.RegisterClassMap<ContainSagaData>(m =>
{
    // This maps all GUID properties to unspecified causing the exception
    m.AutoMap();

    m.SetIsRootClass(true);
    m.MapIdProperty(s => s.Id)
        .SetElementName("_id")
        .SetSerializer(new GuidSerializer(BsonType.String));

    m.SetIgnoreExtraElements(true);
});
```

change the order to

```csharp
BsonClassMap.RegisterClassMap<ContainSagaData>(m =>
{
    m.SetIsRootClass(true);
    m.MapIdProperty(s => s.Id)
        .SetElementName("_id")
        .SetSerializer(new GuidSerializer(BsonType.String));

    // This maps all not yet mapped properties
    m.AutoMap();

    m.SetIgnoreExtraElements(true);
});
```
