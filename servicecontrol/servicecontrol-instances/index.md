---
title: ServiceControl Error instances
summary: A guide to ServiceControl Error Instances. Learn how ServiceControl stores message data and about the available health monitoring options.
reviewed: 2026-10-27
component: ServiceControl
related:
- servicecontrol/import-failed-messages
redirects:
- servicecontrol/persistence
---
A ServiceControl Error instance:

* Monitors [the central `error` queue](/nservicebus/recoverability/configure-error-handling.md#error-queue-monitoring) and stores the failed messages, making them available for manual retries using [ServicePulse](/servicepulse/intro-failed-messages.md).
* Aggregates and forwards data from [Audit instances](/servicecontrol/audit-instances/) for visualization in [ServicePulse](/servicepulse/).
* Collects and serves [heartbeat](/monitoring/heartbeats/) and [custom check](/monitoring/custom-checks/) data for presentation by [ServicePulse](/servicepulse/health-check-notifications.md).
* Publishes [integration events](/servicecontrol/contracts.md) that can be handled by user-built [endpoints](/nservicebus/messaging/publish-subscribe/publish-handle-event.md) that can perform a custom action when those events occur.
* Forwards failed messages to an [error log queue](/servicecontrol/errorlog-auditlog-behavior.md) for custom processing if [configured](/servicecontrol/servicecontrol-instances/configuration.md#transport-servicecontrolforwarderrormessages) to do so.

## Storage

ServiceControl Error instances store their data in RavenDB, SQL Server, or PostgreSQL. Each instance must select one with the required [`ServiceControl/StorageType`](/servicecontrol/servicecontrol-instances/configuration.md#storage-servicecontrolstoragetype) setting; there is no default. See the [storage overview](/servicecontrol/storage/) to choose a storage type. SQL Server and PostgreSQL storage are available from ServiceControl 7; earlier versions store data in RavenDB.

SQL storage requires external storage for message bodies. See [Error instance storage settings](/servicecontrol/servicecontrol-instances/configuration.md#storage) for configuration details.

When using RavenDB, instances deployed using the [ServiceControl Management Utility](/servicecontrol/servicecontrol-instances/deployment/scmu.md) or [PowerShell](/servicecontrol/servicecontrol-instances/deployment/powershell.md) use an embedded database. Instances deployed using [containers](/servicecontrol/servicecontrol-instances/deployment/containers.md) use a [separate RavenDB container](/servicecontrol/storage/ravendb/containers.md).

Failed message data is retained until seven days after successful retry is detected or the failed message is [manually archived](/servicepulse/intro-archived-messages.md). [This retention period can be customized](/servicecontrol/servicecontrol-instances/configuration.md#data-retention).

include: ravendb-exclusive-use-warning

## Notifications

include: servicecontrol-self-monitoring
