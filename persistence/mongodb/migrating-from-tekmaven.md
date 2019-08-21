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

## Compatibility mode

The compatibility mode enables the MongoDB persistence to work with existing saga data without modifications to the database.

### Configuration

Use the following compatibility API to configure the package to work with existing saga data:

snippet: MongoDBTekmavenCompatibility

The `VersionElementName` value must match the `BsonDocument` element name used by the previous saga data property decorated with the `[DocumentVersion]` attribute.

include: must-apply-conventions-for-version

### Saga data class changes

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

#### How Document Versioning Works

include: document-version


## Migrating data

Alternatively to the compatibility mode, the existing saga data can be migrated to the data format used by the `NServiceBus.Storage.MongoDB` package. This approach requires the endpoint to be stopped during the migration. Use the `mongo` Shell to connect to your database and execute the following script:

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

Replace `"Version"` with the name of the version property on your saga data.

WARNING: Make sure to create a backup of your database prior to migrating the saga data.
