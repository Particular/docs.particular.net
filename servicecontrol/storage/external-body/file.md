---
title: File System Message Body Storage
summary: Configure file system storage for message bodies in SQL-backed ServiceControl Error instances.
reviewed: 2026-06-01
component: ServiceControl
versions: '[7,)'
---

File system storage saves message bodies to a local directory. Set `ServiceControl/MessageBody/StorageType` to `FileSystem` on a [SQL Server](/servicecontrol/storage/sqlserver/) or [PostgreSQL](/servicecontrol/storage/postgresql/) Error instance.

## Required settings

| App config key | Environment variable | Default | Required |
| --- | --- | --- | --- |
| `ServiceControl/MessageBody/FileSystem/StoragePath` | `SERVICECONTROL_MESSAGEBODY_FILESYSTEM_STORAGEPATH` | None | Yes |

## Optional settings

| App config key | Environment variable | Default | Notes |
| --- | --- | --- | --- |
| `ServiceControl/MessageBody/FileSystem/DataSpaceRemainingThreshold` | `SERVICECONTROL_MESSAGEBODY_FILESYSTEM_DATASPACEREMAININGTHRESHOLD` | `15` | Percentage of free disk space that must remain available. Must be between `0` and `100`. |

## Setup and runtime

Setup creates the configured directory if it does not exist. The setup identity must be able to create the directory. The runtime identity needs read, write, and delete permissions on it. The retention sweeper deletes body files before removing the corresponding database records.

Use a durable mount or volume. If the storage path is on a temporary or ephemeral filesystem, message bodies will be lost when the filesystem is reset.

Set these variables in the instance's environment:

```text
SERVICECONTROL_MESSAGEBODY_STORAGETYPE=FileSystem
SERVICECONTROL_MESSAGEBODY_FILESYSTEM_STORAGEPATH=/var/servicecontrol/bodies
```

See the [external message body storage overview](/servicecontrol/storage/external-body/) for shared configuration, retention, and backup guidance.
