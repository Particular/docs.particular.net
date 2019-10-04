---
title: Learning Persistence
component: LearningPersistence
reviewed: 2019-02-04
related:
 - samples/saga/simple
 - transports/learning
redirects:
 - nservicebus/learning-persistence
---

include: learning-persistence-description

include: learning-warning

Added in NServiceBus 6.3.

include: learning-usages


## Usage

snippet: LearningPersistence


### Storage Directory

By default all data is stored in a `.sagas` directory that exists at [AppDomain.CurrentDomain.BaseDirectory](https://msdn.microsoft.com/en-us/library/system.appdomain.basedirectory.aspx).

To configure the storage location:

snippet: LearningPersistenceSagaStorageDirectory

Each saga will be stored a sub-directory matching the saga type name with the saga data being serialized into a `.json` file named based on the saga Id.

## Saga concurrency

Learning persistence uses [optimistic concurrency control](https://en.wikipedia.org/wiki/Optimistic_concurrency_control) when accessing saga data. See below for examples of the exceptions thrown when conflicts occur. More information about these scenarios is available in _[saga concurrency](/nservicebus/sagas/concurrency.md)_, including guidance on how to reduce the number of conflicts.

### Creating saga data

```
System.IO.IOException: The file 'S:\.sagas\OrderSaga\944b7efb-7146-adf1-d6ae-968f0d7757fa.json' already exists.
```

### Updating or deleting saga data

```
The process cannot access the file 'S:\.sagas\OrderSaga\a71d248d-0d94-e0bf-3673-361dbd3ec026.json' because it is being used by another process.
```
