---
title: Migrating from NServiceBus.Persistence.MongoDB
component: mongodb
versions: '[1,)'
tags:
 - Persistence
related:
 - persistence/mongodb/migrating-from-sbmako
reviewed: 2019-05-29
---

The `NServiceBus.Storage.MongoDB` package was designed to be fully compatible with the community `NServiceBus.Persistence.MongoDB` package with some minor configuration.

include: migration-warning

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

### How Document Versioning Works

include: document-version


## Compatibility mode

Compatibility mode allows the MongoDB persistence to work with saga data created with the `NServiceBus.Persistence.MongoDB` package without modifications to the database.

### Configuration

Use the following API to configure the package to work with existing saga data:

snippet: MongoDBTekmavenCompatibility

The `VersionElementName` value must match the `BsonDocument` element name used by the previous saga data property decorated with the `[DocumentVersion]` attribute.

include: must-apply-conventions-for-version


## Migrating data

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
