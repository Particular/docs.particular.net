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

### Saga consistency behavior

The persister locks the file for the duration of the mesage processing. Any other message that tries to create or modify an existing saga instance will fail and the message will be retried.
The in-memory persister does not support locking. Although the persister will apply a file lock this does not result in blocking processing and waiting until the file lock is released. Instead, an exception will be raised due to the file lock and requires the message to be reprocessed via [recoverability](/nservicebus/recoverability/).
Please read the guidance on [saga concurrency](/nservicebus/sagas/concurrency.md) on potential improvements.


### Concurrent access to non-existing saga instances
```
System.IO.IOException: The file 'S:\.sagas\OrderSaga\944b7efb-7146-adf1-d6ae-968f0d7757fa.json' already exists.
```

### Concurrent access to existing saga instances
```
The process cannot access the file 'S:\.sagas\OrderSaga\a71d248d-0d94-e0bf-3673-361dbd3ec026.json' because it is being used by another process.
```

Please read the guidance on [saga concurrency](/nservicebus/sagas/concurrency.md) on potential improvements.
