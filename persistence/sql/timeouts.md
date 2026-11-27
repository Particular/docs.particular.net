---
title: Timeouts Persister
summary: SQL Persistence does not store timeouts. Migrate stored timeouts to transport-native delayed delivery.
component: SqlPersistence
reviewed: 2026-11-27
versions: '[4,)'
---

Delayed messages use the transport's [native delayed delivery](/nservicebus/messaging/delayed-delivery.md). SQL persistence does not store timeouts. See the [SQL persistence upgrade guide](/persistence/upgrades/sql-6to7.md#timeout-storage) for migration guidance.

## Migrating timeouts

Existing timeouts can be migrated to native delayed delivery with the [migration tool](/nservicebus/tools/migrate-to-native-delivery.md).
