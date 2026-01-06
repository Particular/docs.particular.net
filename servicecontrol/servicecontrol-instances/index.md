---
title: ServiceControl Error instances
summary: A guide to ServiceControl Error Instances. Learn about how ServiceControl persists message data and the health monitoring options available.
reviewed: 2024-07-19
component: ServiceControl
related:
- servicecontrol/import-failed-messages
redirects:
- servicecontrol/persistence
---
A ServiceControl Error instance:

* Monitors [the central `error` queue](/nservicebus/recoverability/configure-error-handling.md#error-queue-monitoring) and stores the failed messages making them available for manual retries using [ServicePulse](/servicepulse/intro-failed-messages.md).
* Aggregates and forwards data from [Audit instances](/servicecontrol/audit-instances/) for visualization in [ServicePulse](/servicepulse/).
* Collects and serves [heartbeat](/monitoring/heartbeats/) and [custom check](/monitoring/custom-checks/) data for presentation by [ServicePulse](/servicepulse/health-check-notifications.md)
* Publishes [integration events](/servicecontrol/contracts.md) that can be handled by user built [endpoints](/nservicebus/messaging/publish-subscribe/publish-handle-event.md) that can perform a custom action when those events occur.
* If [configured](/servicecontrol/servicecontrol-instances/configuration.md#transport-servicecontrolforwarderrormessages), forwards failed messages to an [error log queue](/servicecontrol/errorlog-auditlog-behavior.md) for custom processing.

## Persistence

Each ServiceControl Error instance stores message data in a RavenDB database. For instances deployed using the [ServiceControl Management Utility](/servicecontrol/servicecontrol-instances/deployment/scmu.md) or [PowerShell](/servicecontrol/servicecontrol-instances/deployment/powershell.md) this database is embedded with the ServiceControl Error instance. For ServiceControl Error instances deployed using [containers](/servicecontrol/servicecontrol-instances/deployment/containers.md) the database resides in a [separate container](/servicecontrol/ravendb/containers.md).

Failed message data is retained until 7 days after successful retry is detected or the failed message is [manually archived](/servicepulse/intro-archived-messages.md). [This retention period can be customized](/servicecontrol/servicecontrol-instances/configuration.md#data-retention).

include: ravendb-exclusive-use-warning

## Notifications

include: servicecontrol-self-monitoring
