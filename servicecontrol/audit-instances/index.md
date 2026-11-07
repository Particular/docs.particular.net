---
title: ServiceControl Audit instances
summary: Information about ServiceControl Audit instances
reviewed: 2026-10-27
component: ServiceControl
redirects:
 - servicecontrol/audit-instances/persistence
---

Each endpoint in the system can be [configured to send audit copies of every message that is processed into a central audit queue](/nservicebus/operations/auditing.md). ServiceControl Audit instances consume and store the messages sent to the audit queue and make them available for visualizing message flows in [ServicePulse](/servicepulse/).

ServiceControl Audit can optionally forward these messages into an [audit log queue](configuration.md#transport-servicecontrol-auditforwardauditmessages) for further processing if required.

```mermaid
graph LR
  Endpoints -- Audit<br>Data --> AuditQ[Audit Queue]

  SCA[ServiceControl Audit<br>instance]
  SC[ServiceControl<br>Instance]
  AuditQ --> SCA

  ServicePulse -.-> SC
  SCA --> AuditLog[Audit.Log<br>Queue]

  SC -. HTTP Queries .-> SCA
  SCA -- Notifications --> SC
```

Data about audit messages is exposed via an HTTP API from a ServiceControl Error instance, which aggregates the data stored in [all connected ServiceControl Audit instances](/servicecontrol/servicecontrol-instances/remotes.md#overview-sharding-audit-messages-with-competing-consumers).

> [!IMPORTANT]
> Connecting ServicePulse directly to a ServiceControl Audit instance is not supported.

> [!NOTE]
> The ServiceControl HTTP API is designed for use by ServicePulse only and may change at any time. Use of this HTTP API for other purposes is discouraged.

## Persistence

ServiceControl Audit instances store their data in RavenDB, SQL Server, or PostgreSQL. Each instance must select one with the required [`ServiceControl.Audit/StorageType`](/servicecontrol/audit-instances/configuration.md#storage-servicecontrol-auditstoragetype) setting; there is no default. See the [storage overview](/servicecontrol/storage/) to choose a storage type. SQL Server and PostgreSQL storage are available from ServiceControl 7; earlier versions store data in RavenDB.

SQL storage requires external storage for message bodies. See [Audit instance storage settings](/servicecontrol/audit-instances/configuration.md#storage) for configuration details.

When using RavenDB, each ServiceControl Audit instance stores message data in a RavenDB database. For instances deployed using the ServiceControl Management utility or PowerShell this database is embedded with the ServiceControl Audit instance. For ServiceControl Audit instances deployed using containers the database resides in a [separate container](/servicecontrol/storage/ravendb/containers.md).

By default, audit data is retained for 7 days. [This retention period can be customized](/servicecontrol/audit-instances/configuration.md#data-retention).

include: ravendb-exclusive-use-warning

## Notifications

Each ServiceControl Audit instance sends notification messages to a ServiceControl Error instance.

### Endpoint detection

When a ServiceControl Audit instance detects a new endpoint, it sends a notification to the ServiceControl Error instance. The Error instance keeps track of all of the endpoints in the system and can monitor them with heartbeats and custom checks.

include: servicecontrol-self-monitoring
