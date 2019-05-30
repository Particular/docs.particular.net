---
title: Migrating from NServiceBus.MongoDB
component: mongodb
versions: '[1,)'
tags:
 - Persistence
related:
 - persistence/mongodb/from-tekmaven
reviewed: 2019-05-29
---

This package was designed to be fully compatible with the community `NServiceBus.MongoDB` package with some minor configuration.

include: migration-warning

## Configuration

Use the following compatibility API to configure the persistence to work with your existing saga data:

snippet: MongoDBSBMakoCompatibility

The `VersionFieldName` value must match the element name used for the `DocumentVersion` property from the community persister.

include: must-apply-conventions-for-version

In addition the collection naming scheme must follow the same naming scheme used by `NServiceBus.MongoDB`: `sagaDataType => sagaDataType.Name` as demonstrated in the above snippet.

## Saga data class changes

[Saga data classes](nservicebus/sagas/#long-running-means-stateful) no longer need to implement [`IHaveDocumentVersion`](https://github.com/sbmako/NServiceBus.MongoDB#sagas). If the saga data class extends [`ContainMongoSagaData`](https://github.com/sbmako/NServiceBus.MongoDB#sagas) it no longer needs to do so. In cases where `IHaveDocumentVersion` has been explicitly implemented by the saga data class the `DocumentVersion` and `ETag` properties may be safely removed from your Saga data class implementations.

If the `ETag` property is not removed it will no longer be updated by the persister.

```c#

- class MySagaData : IContainSagaData, IHaveDocumentVersion
+ class MySagaData : IContainSagaData
{
	public Guid Id { get; set; }
	public string OriginatingMessageId { get; set; }
	public string Originator { get; set; }
-   public int DocumentVersion { get; set; }
-   public int ETag { get; set; }
}

```

### How Document Versioning Works

include: document-version
