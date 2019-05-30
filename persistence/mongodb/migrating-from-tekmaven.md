---
title: Migrating from NServiceBus.Persistence.MongoDB
component: mongodb
versions: '[1,)'
tags:
 - Persistence
related:
 - persistence/mongodb-tekmaven
 - persistence/mongodb/from-sbmako
reviewed: 2019-05-29
---

This package was designed to be fully compatible with the community `NServiceBus.Persistence.MongoDB` package.

include: migration-warning

## Saga data class changes

Saga data class types no longer need to provide an `int` version property decorated with a [`DocumentVersion`](persistence/mongodb-tekmaven/#saga-definition-guideline). The version property and attribute may be safely removed from your Saga data class implementations. 

```c#

class MySagaData : IContainSagaData
{
	public Guid Id { get; set; }
	public string OriginatingMessageId { get; set; }
	public string Originator { get; set; }
-   [DocumentVersion]
-   public int Version { get; set; }
}

```

include: document-version

## Configuration

Use the following compatibility API to configure the persistence to work with your existing saga data:

snippet: MongoDBTekmavenCompatibility

include: must-apply-conventions-for-version