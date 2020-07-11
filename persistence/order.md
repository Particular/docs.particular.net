---
title: Configuration order for persistence
summary: When configuring persistence, order is important
component: Core
reviewed: 2020-07-11
versions: '[5.0,)'
redirects:
- nservicebus/persistence-order
- nservicebus/persistence/order
---

When using different persistence options for storage types, the configuration order is important. When specifying multiple persistence options for the same storage type, the last-configured option will be used. Using the generic `UsePersistence<TPersistenceOption>` (without specifying a storage type) applies the persistence to all its supported storage types.


### Example 1

In this example, RavenDB persistence is used for all storage types as it overwrites the configuration for the outbox and subscription storage types.

snippet: PersistenceOrder_Incorrect


### Example 2

To avoid overwriting, all storage types can be explicitly configured using the `UsePersistence<TPersistenceOption, TStorageType>` API.

snippet: PersistenceOrder_Explicit


### Example 3

Instead of explicitly defining all storage types, the generic persistence option can specified before the explicit overwrites. In this example, RavenDB persistence will be used for all storage types except for the outbox and subscriptions.

snippet: PersistenceOrder_Correct
