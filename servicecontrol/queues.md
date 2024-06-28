---
title: ServiceControl Queues
summary: A breakdown of all of the queues required by each ServiceControl instance
reviewed: 2024-04-02
component: ServiceControl
---

ServiceControl relies on a number of queues to function. The simplest way to configure these queues is to use ServiceControl Management or Powershell to install each ServiceControl instance.

These queues can be manually created before deploying a ServiceControl instance. The technique used will differ depending on the transport in use:

- [Azure Service Bus](/transports/azure-service-bus/operational-scripting.md#operational-scripting-asb-transport-queue-create)
- [Azure Storage Queues](/transports/azure-storage-queues/operations-scripting.md#create-queues)
- [SQL Server](/transports/sql/operations-scripting.md#create-queues)
- [MSMQ](/transports/msmq/operations-scripting.md#create-queues)
- [RabbitMQ](/transports/rabbitmq/operations-scripting.md#endpoint-create)
- [Amazon SQS](/transports/sqs/operations-scripting.md)

> [!NOTE]
> ServiceControl instances do not subscribe to any events, and so do not require any subscriptions to be configured.

## Error instance

These queues are required by ServiceControl Error instances.

### Failed messages queue

If an NServiceBus endpoint is unable to process a message, after the configured number of retries, it will forward a copy of the message to this queue. The ServiceControl error instance will read these messages and add them to its database.

- Default name: **_error_**

| Component                          | Access<br/>requirements | Configuration                                                                                                                        |
| ---------------------------------- | :---------------------: | ------------------------------------------------------------------------------------------------------------------------------------ |
| NServiceBus endpoints              |          Write          | [Failed message configuration](/nservicebus/recoverability/configure-error-handling.md#configure-the-error-queue-address-using-code) |
| ServiceControl Error instance      |          Read           | [ServiceBus/ErrorQueue](/servicecontrol/servicecontrol-instances/configuration.md#transport-servicebuserrorqueue)                                      |
| ServiceControl Audit instance      |            -            |
| ServiceControl Monitoring instance |            -            |

### Input queue

Each ServiceControl Error instance has an input queue which accepts control messages from NServiceBus endpoints, and from ServiceControl instances.

If the heartbeats or custom checks plugins are in use, they should be configured to send messages to this queue.

- Template: `<instance name>`
- Default name: **_Particular.ServiceControl_**

| Component                          | Access<br/>requirements | Configuration                                                                                                                                                       |
| ---------------------------------- | :---------------------: | ------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| NServiceBus endpoints              |          Write          | [Heartbeats plugin](/monitoring/heartbeats/install-plugin.md) and [Custom checks plugin](/monitoring/custom-checks/install-plugin.md)                               |
| ServiceControl Error instance      |       Read/Write        | _Instance name_                                                                                                                                                     |
| ServiceControl Audit instance      |          Write          | [ServiceControl.Audit/ServiceControlQueueAddress](/servicecontrol/audit-instances/configuration.md#transport-servicecontrol-auditservicecontrolqueueaddress) |
| ServiceControl Monitoring instance |            -            |

> [!NOTE]
> The ServiceControl Audit instance sends a control message to the ServiceControl Error instance when it encounters an endpoint for the first time, and when a retried message has been audited to indicate that the retry was a success.

### Error queue

If the ServiceControl Error instance cannot process a message, a copy is forwarded to this error queue.

- Template: `<instance name>.Error`
- Default name: **_Particular.ServiceControl.Error_**

| Component                          | Access<br/>requirements | Configuration                |
| ---------------------------------- | :---------------------: | ---------------------------- |
| NServiceBus endpoints              |            -            |
| ServiceControl Error instance      |       Read/Write        | _Based off of instance name_ |
| ServiceControl Audit instance      |            -            |
| ServiceControl Monitoring instance |            -            |

> [!NOTE]
> The ServiceControl Error instance includes a custom check to see if there are messages in this queue.

### Staging queue

This queue is used by ServiceControl Error instances during the retry process.

- Template: `<instance name>.staging`
- Default name: **_Particular.ServiceControl.staging_**

| Component                          | Access<br/>requirements | Configuration                |
| ---------------------------------- | :---------------------: | ---------------------------- |
| NServiceBus endpoints              |            -            |
| ServiceControl Error instance      |       Read/Write        | _Based off of instance name_ |
| ServiceControl Audit instance      |            -            |
| ServiceControl Monitoring instance |            -            |

### Failed messages forwarding queue

If [configured to do so](/servicecontrol/servicecontrol-instances/configuration.md#transport-servicecontrolforwarderrormessages), the ServiceControl Error instance will forward a copy of every message it processes from the Failed messages queue.

- Template: `<failed messages queue>.log`
- Default name: **_error.log_**

| Component                          | Access<br/>requirements | Configuration                                                                                         |
| ---------------------------------- | :---------------------: | ----------------------------------------------------------------------------------------------------- |
| NServiceBus endpoints              |            -            |
| ServiceControl Error instance      |          Write          | [ServiceBus/ErrorLogQueue](/servicecontrol/servicecontrol-instances/configuration.md#transport-servicebuserrorlogqueue) |
| ServiceControl Audit instance      |            -            |
| ServiceControl Monitoring instance |            -            |

### Endpoint instance input queues

During retry operations, the ServiceControl Error instance will forward messages back to the input queue of the endpoint that failed to process it.

| Component                          | Access<br/>requirements | Configuration                                                                                                                                         |
| ---------------------------------- | :---------------------: | ----------------------------------------------------------------------------------------------------------------------------------------------------- |
| NServiceBus endpoints              |          Read           | [Endpoint name](/nservicebus/endpoints/specify-endpoint-name.md)                                                                                      |
| ServiceControl Error instance      |          Write          | Configured via the [`NServiceBus.FailedQ` header](/nservicebus/messaging/headers.md#error-forwarding-headers-nservicebus-failedq) on failed messages |
| ServiceControl Audit instance      |            -            |
| ServiceControl Monitoring instance |            -            |

## Audit instance

These queues are required by ServiceControl Audit instances.

### Audit queue

If configured to do so, NServiceBus endpoints send a copy of every message processed to the audit queue and a ServiceControl Audit instance reads them.

If the Saga Audit plugin is used, it should be configured to send messages to the audit queue as well.

- Default name: **_audit_**

| Component                          | Access<br/>requirements | Configuration                                                                                                                      |
| ---------------------------------- | :---------------------: | ---------------------------------------------------------------------------------------------------------------------------------- |
| NServiceBus endpoints              |          Write          | [Audit configuration](/nservicebus/operations/auditing.md) and [Saga Audit plugin](/nservicebus/sagas/saga-audit.md#configuration) |
| ServiceControl Error instance      |            -            |
| ServiceControl Audit instance      |          Read           | [ServiceBus/AuditQueue](/servicecontrol/audit-instances/configuration.md#transport-servicebusauditqueue)                    |
| ServiceControl Monitoring instance |            -            |

### Input queue

Each ServiceControl Audit instance includes an input queue. This is currently not used but is required for the ServiceControl Audit instance to run.

- Template: `<instance name>`
- Default value: **_Particular.ServiceControl.Audit_**

| Component                          | Access<br/>requirements | Configuration                                                                                                                                         |
| ---------------------------------- | :---------------------: | ----------------------------------------------------------------------------------------------------------------------------------------------------- |
| NServiceBus endpoints              |            -            |
| ServiceControl Error instance      |            -            |
| ServiceControl Audit instance      |          Read           | [ServiceControl.Audit/InternalQueueName](/servicecontrol/audit-instances/configuration.md#recoverability-servicecontrol-auditinternalqueuename) |
| ServiceControl Monitoring instance |            -            |

### Error queue

If a ServiceControl Audit instance is unable to process a message, it will forward it to the configured error queue.

- Template: `<instance name>.Error`
- Default name: **_Particular.ServiceControl.Audit.Error_**

| Component                          | Access<br/>requirements | Configuration                |
| ---------------------------------- | :---------------------: | ---------------------------- |
| NServiceBus endpoints              |            -            |
| ServiceControl Error instance      |            -            |
| ServiceControl Audit instance      |          Write          | _Based off of instance name_ |
| ServiceControl Monitoring instance |            -            |

### Audit forwarding queue

If [Audit Forwarding is enabled](/servicecontrol/audit-instances/configuration.md#transport-servicecontrol-auditforwardauditmessages), after the ServiceControl Audit instance processes a message from the audit queue, it will forward a copy to this queue.

- Template: `<audit queue name>.log`
- Default name: **_audit.log_**

| Component                          | Access<br/>requirements | Configuration                                                                                                         |
| ---------------------------------- | :---------------------: | --------------------------------------------------------------------------------------------------------------------- |
| NServiceBus endpoints              |            -            |
| ServiceControl Error instance      |            -            |
| ServiceControl Audit instance      |          Write          | [ServiceBus/AuditLogQueue](/servicecontrol/audit-instances/configuration.md#transport-servicebusauditlogqueue) |
| ServiceControl Monitoring instance |            -            |

## Monitoring instance

These queues are required by ServiceControl Monitoring instances.

### Input queue

Endpoints send monitoring information to the monitoring queue and the monitoring instance reads them.

- Template: `<instance name>`
- Default value: **_Particular.Monitoring_**

| Component                          | Access<br/>requirements | Configuration                                                                                                                             |
| ---------------------------------- | :---------------------: | ----------------------------------------------------------------------------------------------------------------------------------------- |
| NServiceBus endpoints              |          Write          | [Monitoring plugin](/monitoring/metrics/install-plugin.md#configuration-service-control-metrics-address)                                  |
| ServiceControl Error instance      |            -            |
| ServiceControl Audit instance      |            -            |
| ServiceControl Monitoring instance |          Read           | [Monitoring/EndpointName](/servicecontrol/monitoring-instances/configuration.md#host-settings-monitoringendpointname) |

### Error queue

When the ServiceControl Monitoring instance cannot process a message it is forwarded to this queue.

- Default name: **_error_**
- Configure via:

| Component                          | Access<br/>requirements | Configuration                                                                                                                     |
| ---------------------------------- | :---------------------: | --------------------------------------------------------------------------------------------------------------------------------- |
| NServiceBus endpoints              |            -            |
| ServiceControl Error instance      |            -            |
| ServiceControl Audit instance      |            -            |
| ServiceControl Monitoring instance |          Write          | [Monitoring/ErrorQueue](/servicecontrol/monitoring-instances/configuration.md#transport-monitoringerrorqueue) |
