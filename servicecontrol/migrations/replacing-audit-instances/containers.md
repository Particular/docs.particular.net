---
title: Replacing an Audit instance using Containers
summary: Instructions on how to replace a ServiceControl Audit instance with zero downtime
reviewed: 2024-07-10
component: ServiceControl
related:
  - servicecontrol/migrations/replacing-audit-instances/scmu
  - servicecontrol/migrations/replacing-audit-instances/powershell
  - servicecontrol/migrations/replacing-error-instances
---

This article describes how to replace an Audit instance with zero downtime when using containers. For an overview of the process and details for other deployment scenarios, see [Replacing an Audit Instance](/servicecontrol/migrations/replacing-audit-instances/).

## Add a new audit instance

> [!NOTE]
> If migrating from ServiceControl on Windows to containerized deployment, care is needed to ensure that instances can communicate with each other.
>
> * All instances will need access to the message queue infrastructure at all times.
> * The active ServiceControl Error instance must be able to communicate over HTTP with all active Audit instances.
>
> This guide assumes this is true, and that the user knows how to create routable URLs to allow the communication, even if one instance is hosted on a virtual machine and another instance is running on containerized infrastructure.

First, a new audit instance must be created. [Deploy a new Audit instance container](/servicecontrol/audit-instances/deployment/containers.md) with its own [database container](/servicecontrol/ravendb/containers.md).

## Add the instance to RemoteInstances

> [!NOTE]
> If the Error instance is not yet deployed on containers, refer to the instructions for [ServiceControl Management](scmu.md#add-the-instance-to-remoteinstances) or [PowerShell](powershell.md#add-the-instance-to-remoteinstances).

Next, modify the Error instance container by changing the [`REMOTEINSTANCES` environment variable](/servicecontrol/servicecontrol-instances/configuration.md#host-settings-servicecontrolremoteinstances) to include the new Audit instance's API URL. The value must be a JSON-encoded array containing objects that each have an `api_uri` value.

This example of a `REMOTEINSTANCES` value (as it would be used in a `docker run` command) shows two remote instances on the Docker internal network:

```shell
-e REMOTEINSTANCES='[{"api_uri":"http://audit-1:44444/api"},{"api_uri":"http://audit-2:44444/api"}]'
```

## Disable audit queue ingestion on the old instance

> [!NOTE]
> If the old Audit instance is not deployed on containers, refer to the instructions for [ServiceControl Management](scmu.md#disable-audit-queue-ingestion-on-the-old-instance) or [PowerShell](powershell.md#disable-audit-queue-ingestion-on-the-old-instance).

Modify the old Audit instance container by specifying the [`INGESTAUDITMESSAGES` environment variable](/servicecontrol/audit-instances/configuration.md#recoverability-servicecontrolingestauditmessages) with a value of `false`.

## Decommission the old audit instance

> [!NOTE]
> If the old Audit instance is not deployed on containers, refer to the instructions for [ServiceControl Management](scmu.md#decommission-the-old-audit-instance) or [PowerShell](powershell.md#decommission-the-old-audit-instance).

When the audit retention period has expired and there are no remaining processed messages in the database, you can decommission the old audit instance:

1. Adjust the `REMOTEINSTANCES` environment variable [as described above](#add-the-instance-to-remoteinstances) except remove the old Audit instance URI from the collection.
2. Remove the old Audit instance by stopping and removing the container, as well as the related [database container](/servicecontrol/ravendb/containers.md).