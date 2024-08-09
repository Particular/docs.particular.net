---
title: Replacing an Error instance using Containers
summary: Instructions on how to replace a ServiceControl Error instance with zero downtime
reviewed: 2024-07-10
component: ServiceControl
related:
  - servicecontrol/migrations/replacing-error-instances/scmu
  - servicecontrol/migrations/replacing-error-instances/powershell
  - servicecontrol/migrations/replacing-audit-instances
---

This article describes how to replace an Error instance with zero downtime when using containers.

> [!NOTE]
> This does not include the complete process, only the steps specific to container deployments.
>
> For an overview of the process and details for other deployment scenarios, see [Replacing an Error Instance](/servicecontrol/migrations/replacing-error-instances/).

## Disable error message ingestion

> [!NOTE]
> If migrating from ServiceControl on Windows to containerized deployment, care is needed to ensure that instances can communicate with each other.
>
> * All instances will need access to the message queue infrastructure at all times.
> * The active ServiceControl Error instance must be able to communicate over HTTP with all active Audit instances.
>
> This guide assumes this is true, and that the user knows how to create routable URLs to allow the communication, even if one instance is hosted on a virtual machine and another instance is running on containerized infrastructure.
>
> Additionally, If the old Error instance is not deployed on containers, refer to the instructions for [ServiceControl Management](scmu.md#disable-error-message-ingestion) or [PowerShell](powershell.md#disable-error-message-ingestion).

Modify the old Error instance container by specifying the [`INGESTERRORMESSAGES` environment variable](/servicecontrol/servicecontrol-instances/configuration.md#recoverability-servicecontrolingesterrormessages) with a value of `false`.

## Replace the Error instance


[Deploy a new Error instance container](/servicecontrol/servicecontrol-instances/deployment/containers.md) with its own [database container](/servicecontrol/ravendb/containers.md). The [`REMOTEINSTANCES` environment variable](/servicecontrol/servicecontrol-instances/configuration.md#host-settings-servicecontrolremoteinstances) should match the configuration of the old Error instance so that it can communicate to the same Audit instance(s).

Now, the old and new Error instance's are both available, but the old Error instance is not ingesting messages.

When confident of a successful upgrade, the old Error instance can be removed. If the old Error instance is not deployed in a container, refer to the instructions for removing the old instance in the [ServiceControl Management](scmu.md#replace-the-error-instance-create-a-new-error-instance) or [PowerShell](powershell.md#replace-the-error-instance-create-a-new-error-instance) guides.