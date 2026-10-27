---
title: File System Message Body Storage
summary: Configure file system storage for message bodies in SQL-backed ServiceControl Error instances.
reviewed: 2026-10-27
component: ServiceControl
versions: '[7,)'
---

File system storage saves message bodies to a local directory. Set `ServiceControl/MessageBody/StorageType` to `FileSystem` on a [SQL Server](/servicecontrol/storage/sqlserver/) or [PostgreSQL](/servicecontrol/storage/postgresql/) Error instance.

## Required settings

include: sc-error-messagebody-filesystem-storagepath

## Optional settings

include: sc-error-messagebody-filesystem-dataspaceremainingthreshold

## Setup and runtime

Setup creates the configured directory if it does not exist. The setup identity must be able to create the directory. The runtime identity needs read, write, and delete permissions on it. The retention sweeper deletes body files before removing the corresponding database records.

> [!WARNING]
> Use a durable directory or volume. Message bodies stored on a temporary or ephemeral file system are lost when it is reset, for example when a container restarts.

Set these variables in the instance's environment:

```text
SERVICECONTROL_MESSAGEBODY_STORAGETYPE=FileSystem
SERVICECONTROL_MESSAGEBODY_FILESYSTEM_STORAGEPATH=/var/servicecontrol/bodies
```

See the [external message body storage overview](/servicecontrol/storage/external-body/) for shared configuration, retention, and backup guidance.
