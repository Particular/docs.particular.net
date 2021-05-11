---
title: Migrating from NServiceBus.Persistence.MongoDB
component: mongodb
versions: '[1,)'
related:
 - persistence/mongodb/migrating-from-sbmako
reviewed: 2021-05-11
---

The `NServiceBus.Storage.MongoDB` package was designed to be fully compatible with the community [`NServiceBus.Persistence.MongoDB`](https://github.com/tekmaven/NServiceBus.Persistence.MongoDb) package with some minor configuration.

include: migration-warning


## NServiceBus upgrade

`NServiceBus.Storage.MongoDB` is available for NServiceBus Version 7 and later. When migrating from `NServiceBus.Persistence.MongoDB` it is recommended to remove the persistence package and upgrade the endpoint to NServiceBus Version 7 before installing the `NServiceBus.Storage.MongoDB` package.


## Customizing the connection

`NServiceBus.Storage.MongoDB` does not provide a configuration setting to pass a connection string directly. Instead, a `MongoClient` can be passed to the new configuration API.

```diff
-   persistence.SetConnectionString("mongodb://localhost/my-database");
persistence.MongoClient(new MongoDB.Driver.MongoClient("mongodb://localhost"));
persistence.DatabaseName("my-database");
```

WARNING: A database name passed in the connection string to the `MongoClient` is **only used for authentication**. Use `persistence.DatabaseName(<database>)` to configure the database to be used.

For more details about the MongoDB persistence configuration options, see the [MongoDB persistence documentation](/persistence/mongodb).

## Saga data class changes

[Saga data classes](/nservicebus/sagas/#long-running-means-stateful) no longer need to provide an `int` version property decorated with a `DocumentVersion`. The version property and attribute may be safely removed from saga data class implementations:

```diff

class MySagaData : IContainSagaData
{
	public Guid Id { get; set; }
	public string OriginatingMessageId { get; set; }
	public string Originator { get; set; }
-       [DocumentVersion]
-       public int Version { get; set; }
}

```

### Saga data compatibility mode

Use the following API to configure the package to work with existing saga data:

snippet: MongoDBTekmavenCompatibility

The `VersionElementName` value must match the `BsonDocument` element name used by the previous saga data property decorated with the `[DocumentVersion]` attribute.

include: must-apply-conventions-for-version


### Migrating saga data

As an alternative to compatibility mode, saga data created by the `NServiceBus.Persistence.MongoDB` package can be migrated to the data format used by the `NServiceBus.Storage.MongoDB` package. This approach requires the endpoint to be stopped during migration. Use the `mongo` shell to connect to the database and execute the following script:

```javascript
db.getCollectionNames().forEach(collectionName => {
    db[collectionName].updateMany({
        Originator: { $exists: true },
        OriginalMessageId: { $exists: true }
    },
    {
        $rename: { "Version": "_version" }
    })
});
```

Replace `"Version"` with the name of the version property on the saga data which was previously decorated with the `[DocumentVersion]` attribute.

WARNING: Be sure to create a backup of the database prior to migrating the saga data.


## Subscriptions

Subscriptions are recreated by restarting the subscribing endpoints. Alternatively, existing subscriptions can be migrated to the new data format.


### Migrating subscriptions

In the [Ryan Hoffman](https://github.com/tekmaven) implementation there is a single document per event type containing a collection of subscribers. In NServiceBus.Storage.MongoDB, subscriptions are individual documents. Each subscription needs to be converted into an `eventsubscription` document.

```javascript
db.subscriptions.find().forEach(type => {
   type.Subscribers.forEach(subscription => {
       var parts = subscription.split('@');
       db.eventsubscription.insert({
           MessageTypeName: type._id.TypeName,
           TransportAddress: parts[0],
           Endpoint: parts[1]
       });
   });
});
```
