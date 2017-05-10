---
title: Learning Persistence
component: LearningPersistence
reviewed: 2017-05-01
related:
---

The Learning Persistence simulates [saga](/nservicebus/sagas/) persistence infrastructure by storing data in the local file system. All files and directories are relative to the current project directory.

include: learning-warning

Added in Version 6.3.

include: learning-usages


## Usage

snippet: LearningPersistence


### Storage Directory

By default all data is stored in a `.sagas` directory that exists at the project root.

To configure the storage location:

snippet: LearningPersistenceSagaStorageDirectory