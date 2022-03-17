---
title: Timeouts Persister
component: SqlPersistence
reviewed: 2020-02-27
versions: '[4,)'
---

## Connection

The timeouts persister can be configured to use a dedicated connection builder. For example, it may be used for creating timeouts tables in a separate database.

snippet: SqlPersistenceTimeoutConnection

## Migrating timeouts

Timeouts can be migrated to the native-delay delivery implementation with the [migration tool](/nservicebus/tools/migrate-to-native-delivery.md).
