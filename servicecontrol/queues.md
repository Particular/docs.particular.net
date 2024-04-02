---
title: ServiceControl Queues
summary: A breakdown of all of the queues required by each ServiceControl instance
reviewed: 2024-04-02
component: ServiceControl
---

ServiceControl relies on a number of queues to function. The simplest way to configure these queues is to use ServiceControl Management or Powershell to install each ServiceControl instance.

These queues can be manually created before deploying a ServiceControl instance:

- [Azure Service Bus](/transports/azure-service-bus/operational-scripting.md#asb-transport-queue-create)
- [Azure Storage Queues](/transports/azure-storage-queues/operations-scripting.md#create-queues)
- [SQL Server](/transports/sql/operations-scripting.md#create-queues)
- [MSMQ](/transports/msmq/operations-scripting.md#create-queues)
- [RabbitMQ](/transports/rabbitmq/operations-scripting.md#endpoint-create)
- [Amazon SQS](/transports/sqs/operations-scripting.md)

NOTE: ServiceControl instances do not subscribe to any events, and so do not require any subscriptions to be configured.

## Error instance

### Failed messages queue

If an NServiceBus endpoint is unable to process a message, after the configured number of retries, it will forward a copy of the message to this queue. The ServiceControl error instance will read these messages and add them to its database.

- Default value: _error_

| Component                          | Access<br/>requirements | Configuration |
| ---------------------------------- | :---------------------: | ------------- |
| NServiceBus endpoints              |          Write          |
| ServiceControl Error instance      |          Read           |
| ServiceControl Audit instance      |            -            |
| ServiceControl Monitoring instance |            -            |

### Input queue

Each ServiceControl Error instance has an input queue which accepts control messages from NServiceBus endpoints, and from ServiceControl instances.

If the heartbeats or custom checks plugins are in use, they should be configured to send messages to this queue.

- Template: &lt;instance name&gt;
- Default value: _Particular.ServiceControl_

| Component                          | Access<br/>requirements | Configuration |
| ---------------------------------- | :---------------------: | ------------- |
| NServiceBus endpoints              |          Write          |
| ServiceControl Error instance      |       Read/Write        |
| ServiceControl Audit instance      |          Write          |
| ServiceControl Monitoring instance |            -            |

NOTE: The ServiceControl Audit instance sends a control message to the ServiceControl Error instance when it encounters an endpoint for the first time, and when retried message has been audited, indicating that the retry was a success.

### Error queue

If the ServiceControl Error instance cannot process a message, a copy is forwarded to this error queue.

- Template: &lt;instance name&gt;.Error
- Default: _Particular.ServiceControl.Error_

| Component                          | Access<br/>requirements | Configuration |
| ---------------------------------- | :---------------------: | ------------- |
| NServiceBus endpoints              |            -            |
| ServiceControl Error instance      |       Read/Write        |
| ServiceControl Audit instance      |            -            |
| ServiceControl Monitoring instance |            -            |

NOTE: The ServiceControl Error instance includes a custom check to see if there are messages in this queue.

### Staging queue

This queue is used by ServiceControl Error instances during the retry process.

- Template: &lt;instance name&gt;.staging
- Default: _Particular.ServiceControl.staging_

| Component                          | Access<br/>requirements | Configuration |
| ---------------------------------- | :---------------------: | ------------- |
| NServiceBus endpoints              |            -            |
| ServiceControl Error instance      |       Read/Write        |
| ServiceControl Audit instance      |            -            |
| ServiceControl Monitoring instance |            -            |

### Failed messages forwarding queue

If configured to do so, the ServiceControl Error instance will forward a copy of every message it processes from the Failed messages queue.

- Template: &lt;failed messages queue&gt;.log
- Default value: _error.log_

| Component                          | Access<br/>requirements | Configuration |
| ---------------------------------- | :---------------------: | ------------- |
| NServiceBus endpoints              |            -            |
| ServiceControl Error instance      |          Write          |
| ServiceControl Audit instance      |            -            |
| ServiceControl Monitoring instance |            -            |

### Endpoint instance input queues

During retry operations, the ServiceControl Error instance will forward messages back to the input queue of the endpoint that failed to process it.

| Component                          | Access<br/>requirements | Configuration |
| ---------------------------------- | :---------------------: | ------------- |
| NServiceBus endpoints              |          Read           |
| ServiceControl Error instance      |          Write          |
| ServiceControl Audit instance      |            -            |
| ServiceControl Monitoring instance |            -            |

## Audit instance

### Audit queue

If configured to do so, NServiceBus endpoints send a copy of every message processed to the audit queue and a ServiceControl Audit instance reads them.

If the Saga Audit plugin is used, it should be configured to send messages to the audit queue as well.

- Default value: _audit_

| Component                          | Access<br/>requirements | Configuration                                                                                                                      |
| ---------------------------------- | :---------------------: | ---------------------------------------------------------------------------------------------------------------------------------- |
| NServiceBus endpoints              |          Write          | [Audit configuration](/nservicebus/operations/auditing.md) and [Saga Audit plugin](/nservicebus/sagas/saga-audit.md#configuration) |
| ServiceControl Error instance      |            -            |
| ServiceControl Audit instance      |          Read           | [ServiceBus/AuditQueue](/servicecontrol/audit-instances/creating-config-file.md#transport-servicebusauditqueue)                    |
| ServiceControl Monitoring instance |            -            |

### Input queue

Each ServiceControl Audit instance includes an input queue. This is currently not used.

- Template: &lt;instance name&gt;
- Default value: _Particular.ServiceControl.Audit_

| Component                          | Access<br/>requirements | Configuration |
| ---------------------------------- | :---------------------: | ------------- |
| NServiceBus endpoints              |            -            |
| ServiceControl Error instance      |            -            |
| ServiceControl Audit instance      |          Read           |
| ServiceControl Monitoring instance |            -            |

### Error queue

If a ServiceControl Audit instance is unable to process a message, it will forward it to the configured error queue.

- Template: &lt;instance name&gt;.Error
- Default value: _Particular.ServiceControl.Audit.Error_

| Component                          | Access<br/>requirements | Configuration |
| ---------------------------------- | :---------------------: | ------------- |
| NServiceBus endpoints              |            -            |
| ServiceControl Error instance      |            -            |
| ServiceControl Audit instance      |          Write          |
| ServiceControl Monitoring instance |            -            |

### Audit forwarding queue

If [Audit Forwarding is enabled](/servicecontrol/audit-instances/creating-config-file.md#transport-servicecontrol-auditforwardauditmessages), after the ServiceControl Audit instance processes a message from the audit queue, it will forward a copy to this queue.

- Template: &lt;audit queue name&gt;.log
- Default value: _audit.log_

| Component                          | Access<br/>requirements | Configuration                                                                                                         |
| ---------------------------------- | :---------------------: | --------------------------------------------------------------------------------------------------------------------- |
| NServiceBus endpoints              |            -            |
| ServiceControl Error instance      |            -            |
| ServiceControl Audit instance      |          Write          | [ServiceBus/AuditLogQueue](/servicecontrol/audit-instances/creating-config-file.md#transport-servicebusauditlogqueue) |
| ServiceControl Monitoring instance |            -            |

## Monitoring instance

### Input queue

Endpoints send monitoring information to the monitoring queue and the monitoring instance reads them.

- Template: &lt;instance name&gt;
- Default value: _Particular.Monitoring_

| Component                          | Access<br/>requirements | Configuration                                                                                                                             |
| ---------------------------------- | :---------------------: | ----------------------------------------------------------------------------------------------------------------------------------------- |
| NServiceBus endpoints              |          Write          | [Monitoring plugin](/monitoring/metrics/install-plugin.md#service-control-metrics-address)                                                |
| ServiceControl Error instance      |            -            |
| ServiceControl Audit instance      |            -            |
| ServiceControl Monitoring instance |          Read           | [Monitoring/EndpointName](/servicecontrol/monitoring-instances/installation/creating-config-file.md#host-settings-monitoringendpointname) |

### Error queue

When the ServiceControl Monitoring instance cannot process a message it is forwarded to this queue.

- Default value: _error_
- Configure via:

| Component                          | Access<br/>requirements | Configuration                                                                                                                     |
| ---------------------------------- | :---------------------: | --------------------------------------------------------------------------------------------------------------------------------- |
| NServiceBus endpoints              |            -            |
| ServiceControl Error instance      |            -            |
| ServiceControl Audit instance      |            -            |
| ServiceControl Monitoring instance |          Write          | [Monitoring/ErrorQueue](/servicecontrol/monitoring-instances/installation/creating-config-file.md#transport-monitoringerrorqueue) |
