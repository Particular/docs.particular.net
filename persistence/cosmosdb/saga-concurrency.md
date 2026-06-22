---
title: CosmosDB Persistence Saga Concurrency
summary: Explains how concurrency works with sagas in the CosmosDB persister
component: CosmosDB
reviewed: 2026-06-22
related:
 - nservicebus/sagas/concurrency
---

## Default behavior

When handling messages simultaneously, conflicts may occur. See below for examples of the exceptions that are thrown. _[Saga concurrency](/nservicebus/sagas/concurrency.md)_ explains how these conflicts are handled, and contains guidance for high-load scenarios.

include: saga-concurrency

### Starting a saga

Example exception:

```
The 'OrderSagaData' saga with id '7ac4d199-6560-4d1a-b83a-b3dad94b0802' could not be created possibly due to a concurrency conflict.
```

### Updating or deleting saga data

partial: sagaconcurrency

Example exception:

```
The 'OrderSagaData' saga with id '7ac4d199-6560-4d1a-b83a-b3dad94b0802' was updated by another process or no longer exists.
```

partial: pessimistic-locking
