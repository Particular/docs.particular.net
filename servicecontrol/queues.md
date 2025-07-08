---
title: ServiceControl Queues
summary: A breakdown of all of the queues required by each ServiceControl instance
reviewed: 2025-07-06
component: ServiceControl
---

ServiceControl relies on a defined set of queues for each instance type to ingest, process, and optionally forward messages. These queues are created during instance setup and vary depending on transport.

## Queue Setup

Queues are only created during "Setup". Setup is run when an instance is created or updated. The simplest way to configure ServiceControl queues during installation is by using:

- ServiceControl Management Utility (SCMU) or
- PowerShell scripts or
- Container flags like `--setup` or `--setup-and-run`

These queues can also be manually created before deploying a ServiceControl instance. The technique used will differ depending on the transport in use:

- [Azure Service Bus](/transports/azure-service-bus/operational-scripting.md#available-commands-asb-transport-queue-create)
- [Azure Storage Queues](/transports/azure-storage-queues/operations-scripting.md#create-queues)
- [SQL Server](/transports/sql/operations-scripting.md#create-queues)
- [MSMQ](/transports/msmq/operations-scripting.md#create-queues)
- [RabbitMQ](/transports/rabbitmq/operations-scripting.md#endpoint-create)
- [Amazon SQS](/transports/sqs/operations-scripting.md)

> [!NOTE]
> ServiceControl instances do not subscribe to any events, and so do not require any subscriptions to be configured.

The following is an overview of all queues based on the default instance names.

| Queue                                                              | Error Instance | Audit Instance| Monitoring Instance | Purpose |
|--------------------------------------------------------------------|:-----:|:-----:|:----------:|:----------:|
| [error](#error-instance-failed-messages-queue)                     |  CR   |       |            |Reads failed messages |
| [audit](#audit-instance-audit-queue)                               |       |  CR   |            |Reads audited messages |
| [error.log](#error-instance-failed-messages-forwarding-queue)      |  CR   |       |            |Forwards copy of failed messages |
| [audit.log](#audit-instance-audit-forwarding-queue)      |  CR   |       |            |Forwards copy of audited messages |
| [particular.monitoring](#monitoring-instance-input-queue)          |       |       |     CR     |Receives heartbeats, checks |
| [particular.servicecontrol](#error-instance-input-queue)           |  CR   |   W   |            |Input queue for error/heartbeat |
| [particular.servicecontrol.audit](#audit-instance-input-queue)           |  CR   |   W   |            |Input queue for audit instance |
| [particular.servicecontrol.error](#error-instance-error-queue)    |  CW   |       |            |Internal error queue |
| [particular.servicecontrol.audit.error](#audit-instance-error-queue)    |  CW   |       |            |Internal error queue for audit |
| [particular.servicecontrol.staging](#error-instance-staging-queue) |  CRW  |       |            |Temporary queue for retries |
| [servicecontrol.throughputdata](#error-instance-throughput-data)   |  CR   |       |     W      |Tracks metrics / throughput |

R = Read (Dequeue)
W = Write (Enqueue)
C = Create

## Error instance

The ServiceControl Error instance uses dedicated queues to receive failed messages and control signals necessary for tracking and recovery operations.

### Failed messages queue

When an NServiceBus endpoint is unable to process a message, after exhausting the configured number of retries, it will forward a copy of the message to an error queue which by default is named as `error`. The ServiceControl error instance reads messages from this queue and persists them in its embedded RavenDB database, marking them as candidates for retry or inspection.

- Default name: **_error_**

| Component                          | Access<br/>requirements | Configuration                                                                                                                        |
|------------------------------------|:-----------------------:|--------------------------------------------------------------------------------------------------------------------------------------|
| NServiceBus endpoints              |          Write          | [Failed message configuration](/nservicebus/recoverability/configure-error-handling.md#configure-the-error-queue-address-using-code) |
| ServiceControl Error instance      |          Read           | [ServiceBus/ErrorQueue](/servicecontrol/servicecontrol-instances/configuration.md#transport-servicebuserrorqueue)                    |
| ServiceControl Audit instance      |            -            |                                                                                                                                      |
| ServiceControl Monitoring instance |            -            |                                                                                                                                      |

### Input queue

Each ServiceControl Error instance has an input queue (also called the control queue) which accepts control messages from NServiceBus endpoints, and from ServiceControl instances. These control messages include:

- Heartbeats
- Custom checks
- Coordination messages (from the Audit instance)

When plugins like heartbeats or custom checks are used, they should be configured to send messages to this input queue.

- Queue name Template: `<instance name>`
- Default name: **_Particular.ServiceControl_**

| Component                          | Access<br/>requirements | Configuration                                                                                                                                                |
|------------------------------------|:-----------------------:|--------------------------------------------------------------------------------------------------------------------------------------------------------------|
| NServiceBus endpoints              |          Write          | [Heartbeats plugin](/monitoring/heartbeats/install-plugin.md) and [Custom checks plugin](/monitoring/custom-checks/install-plugin.md)                        |
| ServiceControl Error instance      |    Create/Read/Write    | [ServiceControl/InstanceName](/servicecontrol/servicecontrol-instances/configuration.md#host-settings-servicecontrolinstancename)                            |
| ServiceControl Audit instance      |          Write          | [ServiceControl.Audit/ServiceControlQueueAddress](/servicecontrol/audit-instances/configuration.md#transport-servicecontrol-auditservicecontrolqueueaddress) |
| ServiceControl Monitoring instance |            -            |                                                                                                                                                              |

> [!NOTE]
> The ServiceControl Audit instance sends a control message to the ServiceControl Error instance when it encounters an endpoint for the first time, and when a retried message has been audited to indicate that the retry was a success.

### Error queue

The ServiceControl Error instance uses an internal queue to handle processing failures that occur during ingestion from the main error queue. If ServiceControl encounters a problem while reading or deserializing a message from the external error queue, it will forward a copy to this internal error queue to prevent message loss and allow for diagnosis or recovery.

- Template: `<instance name>.Error`
- Default name: **_Particular.ServiceControl.Error_**

| Component                          | Access<br/>requirements | Configuration                |
|------------------------------------|:-----------------------:|------------------------------|
| NServiceBus endpoints              |            -            |                              |
| ServiceControl Error instance      |    Create/Read/Write    | _Based off of instance name_ |
| ServiceControl Audit instance      |            -            |                              |
| ServiceControl Monitoring instance |            -            |                              |

> [!NOTE]
> The ServiceControl Error instance includes a built-in custom check that monitors this internal queue. If messages are detected, the check can raise alerts in ServicePulse to notify of ingestion issues.

### Staging queue

The Staging queue is used exclusively by the ServiceControl Error instance to temporarily hold messages during the retry process. When a user initiates a retry in ServicePulse, failed messages are moved into this queue before being re-dispatched to their original endpoint.

This queue exists only during retries. Once messages are dispatched back to their original destination, they are removed from the staging queue.
It is created automatically during setup and used internally by ServiceControl — endpoints do not interact with it directly.

- Template: `<instance name>.staging`
- Default name: **_Particular.ServiceControl.staging_**

| Component                          | Access<br/>requirements | Configuration                |
|------------------------------------|:-----------------------:|------------------------------|
| NServiceBus endpoints              |            -            |                              |
| ServiceControl Error instance      |    Create/Read/Write    | _Based off of instance name_ |
| ServiceControl Audit instance      |            -            |                              |
| ServiceControl Monitoring instance |            -            |                              |

### Failed messages forwarding queue

If [forwarding is enabled](/servicecontrol/servicecontrol-instances/configuration.md#transport-servicecontrolforwarderrormessages), the ServiceControl Error instance will forward a copy of each successfully ingested failed message to a [designated log queue](/servicecontrol/errorlog-auditlog-behavior.md).  These queues are not directly managed by ServiceControl and are meant as points of external integration.

- Template: `<failed messages queue>.log`
- Default name: **_error.log_**

| Component                          | Access<br/>requirements | Configuration                                                                                                           |
|------------------------------------|:-----------------------:|-------------------------------------------------------------------------------------------------------------------------|
| NServiceBus endpoints              |            -            |                                                                                                                         |
| ServiceControl Error instance      |      Create/Write       | [ServiceBus/ErrorLogQueue](/servicecontrol/servicecontrol-instances/configuration.md#transport-servicebuserrorlogqueue) |
| ServiceControl Audit instance      |            -            |                                                                                                                         |
| ServiceControl Monitoring instance |            -            |                                                                                                                         |

### Endpoint instance input queues

During a retry operation, the ServiceControl Error instance re-dispatches failed messages back to the original endpoint's input queue. This queue is the same one where the endpoint normally receives operational messages.

ServiceControl uses the `NServiceBus.FailedQ` header (present on the original failed message) to determine which queue to target. The endpoint then reprocesses the message as if it had just arrived for the first time. Once a message is retried and dispatched to the original endpoint, the endpoint attempts to process it again.

- If the message is successfully processed, it is typically forwarded to the `audit` queue. The Audit instance then ingests the message, confirming that the retry was successful — triggering status updates in ServicePulse that mark the message as resolved.

- If the message fails again during processing, it is returned to the `error` queue. The Error instance ingests it as a newly failed message, and it reappears in ServicePulse for further analysis or retry attempts

| Component                          | Access<br/>requirements | Configuration                                                                                                                                        |
|------------------------------------|:-----------------------:|------------------------------------------------------------------------------------------------------------------------------------------------------|
| NServiceBus endpoints              |       Create/Read       | [Endpoint name](/nservicebus/endpoints/specify-endpoint-name.md)                                                                                     |
| ServiceControl Error instance      |          Write          | Configured via the [`NServiceBus.FailedQ` header](/nservicebus/messaging/headers.md#error-forwarding-headers-nservicebus-failedq) on failed messages |
| ServiceControl Audit instance      |            -            |                                                                                                                                                      |
| ServiceControl Monitoring instance |            -            |                                                                                                                                                      |

### Throughput data

The Throughput Data queue is used to collect and share performance metrics between ServiceControl instances. It enables visibility into message processing rates, endpoint activity, and system health.

- Default name: **_ServiceControl.ThroughputData_**

| Component                          | Access<br/>requirements | Configuration                                                                                                                                                                                                                                                        |
|------------------------------------|:-----------------------:|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| ServiceControl Error instance      |       Create/Read       | Configured via the [`LicensingComponent/ServiceControlThroughputDataQueue` setting](/servicecontrol/servicecontrol-instances/configuration.md#usage-reporting-when-using-servicecontrol-licensingcomponentservicecontrolthroughputdataqueue) |
| ServiceControl Monitoring instance |          Write          | Queue configured via [`Monitoring/ServiceControlThroughputDataQueue`]/servicecontrol/monitoring-instances/configuration.md#usage-reporting-monitoringservicecontrolthroughputdataqueue)                                                     |

## Audit instance

The ServiceControl Audit instance requires specific queues to ingest successfully processed messages from endpoints and support internal operations.


### Audit queue

When auditing is enabled, NServiceBus endpoints send a copy of every successfully processed message to the `audit` queue. The Audit instance reads from this queue and persists the messages to its embedded RavenDB database for visualization and analysis (e.g., ServicePulse).

If the Saga Audit plugin is used, it should be configured to send messages to the audit queue as well.

- Default name: **_audit_**

| Component                          | Access<br/>requirements | Configuration                                                                                                                      |
|------------------------------------|:-----------------------:|------------------------------------------------------------------------------------------------------------------------------------|
| NServiceBus endpoints              |      Create/Write       | [Audit configuration](/nservicebus/operations/auditing.md) and [Saga Audit plugin](/nservicebus/sagas/saga-audit.md#configuration) |
| ServiceControl Error instance      |            -            |                                                                                                                                    |
| ServiceControl Audit instance      |       Create/Read       | [ServiceBus/AuditQueue](/servicecontrol/audit-instances/configuration.md#transport-servicebusauditqueue)                           |
| ServiceControl Monitoring instance |            -            |                                                                                                                                    |

### Input queue

Each ServiceControl Audit instance defines an input queue, even though it is currently not actively used for message ingestion. However, its presence is required for the instance to operate correctly.

- Template: `<instance name>`
- Default value: **_Particular.ServiceControl.Audit_**

| Component                          | Access<br/>requirements | Configuration                                                                                                                        |
|------------------------------------|:-----------------------:|--------------------------------------------------------------------------------------------------------------------------------------|
| NServiceBus endpoints              |            -            |                                                                                                                                      |
| ServiceControl Error instance      |            -            |                                                                                                                                      |
| ServiceControl Audit instance      |       Create/Read       | [ServiceControl.Audit/InstanceName](/servicecontrol/audit-instances/configuration.md#host-settings-servicecontrol-auditinstancename) |
| ServiceControl Monitoring instance |            -            |                                                                                                                                      |

### Error queue

If a ServiceControl Audit instance encounters a message it cannot process — due to deserialization issues, invalid headers, or other ingestion failures — it forwards that message to a dedicated error queue for isolation and troubleshooting. This queue is created and written to by the Audit instance itself. It is not consumed by other ServiceControl components and is intended for diagnostic purposes.

- Template: `<instance name>.Error`
- Default name: **_Particular.ServiceControl.Audit.Error_**

| Component                          | Access<br/>requirements | Configuration                |
|------------------------------------|:-----------------------:|------------------------------|
| NServiceBus endpoints              |            -            |                              |
| ServiceControl Error instance      |            -            |                              |
| ServiceControl Audit instance      |      Create/Write       | _Based off of instance name_ |
| ServiceControl Monitoring instance |            -            |                              |

### Audit forwarding queue

If [Audit Forwarding is enabled](/servicecontrol/audit-instances/configuration.md#transport-servicecontrol-auditforwardauditmessages), the ServiceControl Audit instance sends a copy of each successfully ingested audit message to a designated log queue. This allows external systems to consume or archive audit data independently of ServiceControl.

- Template: `<audit queue name>.log`
- Default name: **_audit.log_**

| Component                          | Access<br/>requirements | Configuration                                                                                                  |
|------------------------------------|:-----------------------:|----------------------------------------------------------------------------------------------------------------|
| NServiceBus endpoints              |            -            |                                                                                                                |
| ServiceControl Error instance      |            -            |                                                                                                                |
| ServiceControl Audit instance      |      Create/Write       | [ServiceBus/AuditLogQueue](/servicecontrol/audit-instances/configuration.md#transport-servicebusauditlogqueue) |
| ServiceControl Monitoring instance |            -            |                                                                                                                |

## Monitoring instance

The Monitoring instance collects performance and health data from endpoints using the NServiceBus Metrics plugin. It uses a set of queues to ingest, forward, and share this data with other ServiceControl components.

### Input queue

Endpoints send monitoring data to the Monitoring instance via its input queue. Messages are sent to this queue even if the Monitoring instance is offline. Once available, it will process any backlog.

- Template: `<instance name>`
- Default value: **_Particular.Monitoring_**

| Component                          | Access<br/>requirements | Configuration                                                                                                         |
|------------------------------------|:-----------------------:|-----------------------------------------------------------------------------------------------------------------------|
| NServiceBus endpoints              |          Write          | [Monitoring plugin](/monitoring/metrics/install-plugin.md#configuration-service-control-metrics-address)              |
| ServiceControl Error instance      |            -            |                                                                                                                       |
| ServiceControl Audit instance      |            -            |                                                                                                                       |
| ServiceControl Monitoring instance |       Create/Read       | [Monitoring/InstanceName](/servicecontrol/monitoring-instances/configuration.md#host-settings-monitoringinstancename) |

### Error queue

If the Monitoring instance fails to process a message — due to deserialization errors or unsupported formats — it forwards the message to a configured error queue.

- Default name: **_error_**
- Configure via:  Configuration Setting `Monitoring/ErrorQueue`

| Component                          | Access<br/>requirements | Configuration                                                                                                 |
|------------------------------------|:-----------------------:|---------------------------------------------------------------------------------------------------------------|
| NServiceBus endpoints              |            -            |                                                                                                               |
| ServiceControl Error instance      |            -            |                                                                                                               |
| ServiceControl Audit instance      |            -            |                                                                                                               |
| ServiceControl Monitoring instance |      Create/Write       | [Monitoring/ErrorQueue](/servicecontrol/monitoring-instances/configuration.md#transport-monitoringerrorqueue) |

### Throughput data

This queue enables the Monitoring instance to share performance metrics with the Error instance — such as message rates and endpoint activity.

- Queue name: **_ServiceControl.ThroughputData_**

| Component                          | Access<br/>requirements | Configuration                                                                                                                                                                                                                                                        |
|------------------------------------|:-----------------------:|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| ServiceControl Error instance      |       Create/Read       | Configured via the [`LicensingComponent/ServiceControlThroughputDataQueue` setting](/servicecontrol/servicecontrol-instances/configuration.md#usage-reporting-when-using-servicecontrol-licensingcomponentservicecontrolthroughputdataqueue) |
| ServiceControl Monitoring instance |          Write          | Queue configured via [`Monitoring/ServiceControlThroughputDataQueue`](/servicecontrol/monitoring-instances/configuration.md#usage-reporting-monitoringservicecontrolthroughputdataqueue)                                                     |

Both instances must be configured to use the same queue name to ensure metrics are exchanged correctly.
