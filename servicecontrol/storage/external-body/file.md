---
title: File System Message Body Storage
summary: Configure file system storage for message bodies in SQL-backed ServiceControl Error and Audit instances.
reviewed: 2026-11-02
component: ServiceControl
versions: '[7,)'
---

File system storage saves message bodies to a local directory. Set `ServiceControl/MessageBody/StorageType` on an Error instance, or `ServiceControl.Audit/MessageBody/StorageType` on an Audit instance, to `FileSystem` when the instance uses [SQL Server](/servicecontrol/storage/sqlserver/) or [PostgreSQL](/servicecontrol/storage/postgresql/) storage.

## Required settings

include: sc-error-messagebody-filesystem-storagepath

include: sc-audit-messagebody-filesystem-storagepath

## Optional settings

include: sc-error-messagebody-filesystem-dataspaceremainingthreshold

include: sc-audit-messagebody-filesystem-dataspaceremainingthreshold

## Setup and runtime

Setup creates the configured directory if it does not exist. The setup identity must be able to create the directory. The runtime identity needs read, write, and delete permissions on it. The retention sweeper deletes body files before removing the corresponding database records.

> [!WARNING]
> Use a durable directory or volume. Message bodies stored on a temporary or ephemeral file system are lost when it is reset, for example when a container restarts.

Setup does not check that the directory is writable: if the directory already exists but the runtime identity cannot write to it, setup succeeds and the instance fails to start up. In a container, the instance runs as a non-root user; see the container volume requirements for [Error instances](/servicecontrol/servicecontrol-instances/deployment/containers.md#volumes) and [Audit instances](/servicecontrol/audit-instances/deployment/containers.md#volumes).

Set these variables in an Error instance's environment:

```text
SERVICECONTROL_MESSAGEBODY_STORAGETYPE=FileSystem
SERVICECONTROL_MESSAGEBODY_FILESYSTEM_STORAGEPATH=/var/servicecontrol/bodies
```

Set these variables in an Audit instance's environment, using a different directory from any other instance:

```text
SERVICECONTROL_AUDIT_MESSAGEBODY_STORAGETYPE=FileSystem
SERVICECONTROL_AUDIT_MESSAGEBODY_FILESYSTEM_STORAGEPATH=/var/servicecontrol/audit-bodies
```

See the [external message body storage overview](/servicecontrol/storage/external-body/) for shared configuration, retention, and backup guidance.
