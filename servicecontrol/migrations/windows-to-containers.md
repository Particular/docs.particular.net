---
title: Migrate ServiceControl to container deployment
summary: Instructions on how to migrate ServiceControl instances from Windows to container-based hosting.
reviewed: 2024-07-15
isUpgradeGuide: true
component: ServiceControl
related:
---

ServiceControl 5.3.0 adds the ability to host ServiceControl instances in Linux containers. This article describes how to migrate from ServiceControl hosted on Windows hosting to new ServiceControl instances hosted in containerized infrastructure.

Migration for ServiceControl instances is different for each instance type.

## Migrating Error instances

Migration of an Error instance is only necessary if error message data must be preserved. This is the recommended approach, as error messages may contain valuable business data.

See the article [Replacing an Error instance](/servicecontrol/migrations/replacing-error-instances/) for migration instructions. This procedure includes disabling error ingestion on the old instance so that new error messages are held temporarily in the error queue, replaying or archiving any in-flight error messages, and then creating a new Error instance to begin processing the messages held in the error queue.

When the objective is to migrate from Windows hosting to container-based hosting, follow the instructions in the article above labeled "using ServiceControl Management" or "using PowerShell" when referring to the existing Error instance, and "using Containers" when referring to the new Error instance.

## Migrating Audit instances

Migration of an Audit instance is only necessary if audit message data must be preserved. Audit data is regularly deleted after it reaches the end of its retention period, so when considering a migration, it's necessary to consider whether the value of the audit data, as used by ServicePulse, is worth the added complexity of the migration procedure.

See the article [Replacing an Audit instance](/servicecontrol/migrations/replacing-audit-instances/) for migration instructions. The procedure includes creating a new audit instance in parallel with the old one, and disabling audit ingestion on the old instance. In this way, audit data is served from both instances until all the audit data in the Audit instance reaches its retention period and is deleted, after which the old audit instance can be removed.

When the objective is to migrate from Windows hosting to container-based hosting, follow the instructions in the article above labeled "using ServiceControl Management" or "using PowerShell" when referring to the existing Audit instance, and "using Containers" when referring to the new Audit instance.

## Migrating Monitoring instances

Monitoring instances do not permanently store any data. As a result, a [new Monitoring instance can be deployed using containers](/servicecontrol/monitoring-instances/deployment/containers.md) at any time, while the old instance is stopped and removed.