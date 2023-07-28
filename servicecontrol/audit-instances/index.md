---
title: ServiceControl Audit instances
summary: Information about ServiceControl Audit instances
reviewed: 2021-08-06
component: ServiceControl
---

In ServiceControl versions 4 and above, a ServiceControl Audit instance manages the audit queue. Data about audit messages is exposed via an HTTP API on a ServiceControl Error instance. This API is used by [ServiceInsight](/serviceinsight/) for visualizing message flows.

NOTE: The ServiceControl HTTP API is designed for use by ServiceInsight only and may change at any time. Use of this HTTP API for other purposes is discouraged.

```mermaid
graph LR
  Endpoints -- Audit<br>Data --> AuditQ[Audit Queue]

  SCA[ServiceControl Audit<br>instance]
  SC[ServiceControl<br>Instance]
  AuditQ --> SCA

  ServiceInsight -.-> SC
  SCA --> AuditLog[Audit.Log<br>Queue]

  SC -. HTTP Queries .-> SCA
  SCA -- Notifications --> SC
```

Each endpoint in the system should be [configured to send audit copies of every message that is processed into a central audit queue](/nservicebus/operations/auditing.md). A ServiceControl Audit instance reads the messages in the audit queue and makes them available for visualization in ServiceInsight. ServiceControl Audit can optionally forward these messages into an Audit Log queue for further processing if required. In some cases, it might be useful to exclude certain message types from being forwarded to the audit queue. This can be accomplished with a [custom behavior in the pipeline](/samples/pipeline/audit-filtering).

Each ServiceControl Audit instance stores data in an embedded database. Audit data is retained for 30 days. [This retention period can be customized](/servicecontrol/audit-instances/creating-config-file.md#data-retention).

## Connected to a ServiceControl instance

When using ServiceControl Management to create a new ServiceControl instance, a connected ServiceControl Audit instance is automatically created. Using PowerShell, create the ServiceControl instance first, then the ServiceControl Audit instance.

When [auditing](/nservicebus/operations/auditing.md) NServiceBus messages there must be at least one ServiceControl audit instance. ServiceInsight connects directly to the ServiceControl Error instance, which will aggregate the data stored in [all connected ServiceControl Audit instances](/servicecontrol/servicecontrol-instances/remotes.md#overview-sharding-audit-messages-with-competing-consumers).

Connecting ServiceInsight directly to a ServiceControl Audit instance is not supported.

## Notifications

Each ServiceControl Audit instance sends notification messages to a ServiceControl Error instance.

### Endpoint detection

When a ServiceControl Audit instance detects a new endpoint, it sends a notification to the ServiceControl Error instance. The Error instance keeps track of all of the endpoints in the system and can monitor them with heartbeats and custom checks.

### Successful retry detection

When a ServiceControl Audit instance detects that an audited message is the result of a retry, it sends a notification to the ServiceControl Error instance.

### Health monitoring

include: self-monitoring
