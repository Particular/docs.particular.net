---
title: Migrating from NServiceBus.Persistence.MongoDB
component: mongodb
versions: '[1,)'
tags:
 - Persistence
related:
 - persistence/mongodb-tekmaven
 - persistence/mongodb/migrating-from-sbmako
reviewed: 2019-05-29
---

This package was designed to be fully compatible with the community `NServiceBus.Persistence.MongoDB` package with some minor configuration.

include: migration-warning

## Configuration

Use the following compatibility API to configure the persistence to work with existing saga data:

snippet: MongoDBTekmavenCompatibility

The `VersionFieldName` value must match the element name used by the previous saga data property decorated with the `[DocumentVersion]` attribute.

include: must-apply-conventions-for-version

## Saga data class changes

[Saga data classes](/nservicebus/sagas/#long-running-means-stateful) no longer need to provide an `int` version property decorated with a [`DocumentVersion`](/persistence/mongodb-tekmaven/#saga-definition-guideline). The version property and attribute may be safely removed from Saga data class implementations:

```diff

class MySagaData : IContainSagaData
{
	public Guid Id { get; set; }
	public string OriginatingMessageId { get; set; }
	public string Originator { get; set; }
-   [DocumentVersion]
-   public int Version { get; set; }
}

```

### How Document Versioning Works

include: document-version
