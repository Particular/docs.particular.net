---
title: Learning Persistence
component: LearningPersistence
reviewed: 2020-12-01
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


## Persistence at a glance

For a description of each feature, see the [persistence at a glance legend](/persistence/#persistence-at-a-glance).

|Feature                    |   |
|:---                       |---
|Storage Types              |Sagas only. Subscriptions/Timeouts handled natively by Learning Transport.
|Transactions               |None
|Concurrency control        |Optimistic concurrency via file locks
|Scripted deployment        |Not supported
|Installers                 |None. The required folder structure is always created at runtime.


## Usage

snippet: LearningPersistence


### Storage Directory

By default all data is stored in a `.sagas` directory that exists at [AppDomain.CurrentDomain.BaseDirectory](https://msdn.microsoft.com/en-us/library/system.appdomain.basedirectory.aspx).

To configure the storage location:

snippet: LearningPersistenceSagaStorageDirectory

Each saga will be stored a sub-directory matching the saga type name with the saga data being serialized into a `.json` file named based on the saga ID.

## Saga concurrency

When simultaneously handling messages, conflicts may occur. See below for examples of the exceptions which are thrown. _[Saga concurrency](/nservicebus/sagas/concurrency.md)_ explains how these conflicts are handled, and contains guidance for high-load scenarios.

### Starting a saga

Example exception:

```
System.IO.IOException: The file 'S:\.sagas\OrderSaga\944b7efb-7146-adf1-d6ae-968f0d7757fa.json' already exists.
```

### Updating or deleting saga data

Learning persistence uses file locks when updating or deleting saga data. The effect is similar to [optimistic concurrency control](https://en.wikipedia.org/wiki/Optimistic_concurrency_control) with the difference that failure will occur when reading the saga data, before the handler is invoked.

Example exception:

```
The process cannot access the file 'S:\.sagas\OrderSaga\a71d248d-0d94-e0bf-3673-361dbd3ec026.json' because it is being used by another process.
```
