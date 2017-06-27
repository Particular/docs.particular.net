---
title: Learning Persistence
component: LearningPersistence
reviewed: 2017-05-01
related:
 - samples/saga/simple
 - nservicebus/learning-transport
redirects:
 - nservicebus/learning-persistence
---

include: learning-persistence-description

include: learning-warning

Added in Version 6.3.

include: learning-usages


## Usage

snippet: LearningPersistence


### Storage Directory

By default all data is stored in a `.sagas` directory that exists at [AppDomain.CurrentDomain.BaseDirectory](https://msdn.microsoft.com/en-us/library/system.appdomain.basedirectory.aspx).

To configure the storage location:

snippet: LearningPersistenceSagaStorageDirectory

Each saga will be stored a sub-directory matching the saga type name with the saga data being serialized into a `.json` file named based on the saga Id.