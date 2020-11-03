---
title: Upgrade ServiceControl from Version 4 to Version 5
summary: Instructions on how to upgrade ServiceControl from version 4 to 5
isUpgradeGuide: true
reviewed: 2020-10-30
---

## Overview

Upgrading ServiceControl from version 4 to version 5 is a major upgrade and requires careful planning due to switch from RavenDB 3.5 to RavenDB 5 for data storage. 

## Prerequisites

Before upgrading to ServiceControl version 5 the instance being upgraded must be upgraded to at least [version 4.13.0](https://github.com/Particular/ServiceControl/releases/tag/4.13.0).

For more information how to upgrade from Version 3.x to 4.13.0 consult the [upgrade documentation](/servicecontrol/upgrades/3to4).

### Upgrading ServiceControl

ServiceControl and ServiceControl.Audit instances cannot be upgraded in place. The data storage format used by ServiceControl and ServiceControl Audit version 5 is incompatible with previous versions.

There are two migration paths available. The simplest option is to run a [side-by-side installation](/servicecontrol/upgrades/4to5/side-by-side.md). The existing version 4 ServiceControl installation is disconnected from the input queues and can still be queried using ServiceInsight or ServicePulse.


The existing version 4 ServiceControl installation is disconnected from the input queues and can still be queried using ServiceInsight or ServicePulse.

#### Side-by-side installation

In a side-by-side installation, the existing version 4 installation is disconnected from the error and audit queues and 

TODO: This is a very rough draft

- Modify existing instance to use !disable for input queue
- Put the instance in the maintenance mode
- Create a new instance for V5
- Execute data migration
  - ProcessedMessages -> Audit
  - FailedMessages -> Main
- Remove old instance

### Time for upgrade

TODO

### Obsolete configuration options

#### ExposeRavenDB

RavenDB HTTP API is always exposed and bound to `localhost`.

#### ExpirationProcessBatchSize

Document expiration takes advantage of built-in RavenDB mechanisms that do not allow configuring of batch size.

#### RavenDBLogLevel

RavenDB logging can be configured using [RavenDB configuration mechanism](https://ravendb.net/docs/article-page/5.0/csharp/server/configuration/configuration-options).

Note: The `settings.json` file needs to be placed in the `RavenDBServer` subfolder in the ServiceControl installation directory.

#### Raven/IndexStoragePath

RavenDB does not expose this setting in Version 5. 

#### Raven/Esent/LogsPath

RavenDB does not expose this setting in Version 5. 