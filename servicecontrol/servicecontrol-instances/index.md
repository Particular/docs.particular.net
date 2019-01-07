---
title: ServiceControl instances
reviewed: 2017-07-10
component: ServiceControl
related:
- servicecontrol/import-failed-messages
---

ServiceControl instances collect and analyze data about the endpoints that make up a system and the messages flowing between them. This data is exposed to [ServiceInsight](/serviceinsight/) and [ServicePulse](/servicepulse/) via an HTTP API and SignalR, and via [external integration events](/servicecontrol/contracts.md).

NOTE: The ServiceControl HTTP API is designed for use by ServicePulse and ServiceInsight only and may change at any time. Use of this HTTP API for other purposes is discouraged.

```mermaid
graph LR
  subgraph Endpoints
    Audit
    Error
    Plugins[Heartbeats<br>Custom Checks<br>Saga Audit]
  end

  Audit -- Audit<br>Data --> AuditQ[Audit Queue]
  Error -- Error<br>Data --> ErrorQ[Error Queue]
  Plugins -- Plugin<br>Data --> SCQ
	
  SCQ[ServiceControl<br>Input Queue] --> SC[ServiceControl<br>Instance]
  AuditQ --> SC
  ErrorQ --> SC
  ServicePulse -.-> SC
  ServiceInsight -.-> SC
  SC --> AuditLog[Audit.Log<br>Queue]
  SC --> ErrorLog[Error.Log<br>Queue]
  SC -. Integration<br>Events .-> Watchers[Alert<br>Subscribers]
```

Each endpoint in the system should be [configured to send audit copies of every message that is processed into a central audit queue](/nservicebus/operations/auditing.md). A ServiceControl instance reads the messages in the audit queue and makes them available for visualization in ServiceInsight. ServiceControl can [optionally forward these messages into an Audit Log queue](/servicecontrol/errorlog-auditlog-behavior.md) for further processing if required.

Each endpoint in the system should be [configured to send failed messages to a central error queue](/nservicebus/recoverability/) after those messages have gone through immediate and delayed retries. A ServiceControl instance reads the messages in the error queue and makes them available to be retried manually in ServicePulse and ServiceInsight. ServiceControl can [optionally forward these messages into an Error Log queue](/servicecontrol/errorlog-auditlog-behavior.md) for further processing if required. 

Each endpoint may have additional plugins installed which collect and send data to a ServiceControl instance. The [Heartbeats plugin](/monitoring/heartbeats/) can be used to detect which endpoint instances are running and which are offline. The [Custom Checks plugin](/monitoring/custom-checks/) enables endpoints to send user-defined health reports to ServiceControl on a regular schedule. The [Saga Audit plugin](/servicecontrol/plugins/saga-audit.md) instruments audit messages with details of saga state changes for [visualization in ServiceInsight](/serviceinsight/#the-saga-view).

Each ServiceControl instance raises external integration events when important situations are detected. These are standard NServiceBus events that can be subscribed to by any NServiceBus endpoint. See [Use ServiceControl events](/servicecontrol/contracts.md) for a complete list.

Each ServiceControl instance stores data in an embedded database. Audit data is retained for 30 days. Failed message data is retained until the message is retried or manually archived. [These retention periods can be customized](/servicecontrol/creating-config-file.md#data-retention).

Each environment should have a single audit queue and a single error queue that all endpoints are configured to use. Each environment should have a single ServiceControl instance that is connected to it's audit and error queues. Consider the advice given in the [Planning](/servicecontrol/servicecontrol-in-practice.md) section of the documentation before creating a new ServiceControl instance.

### Self-monitoring via custom checks

ServiceControl includes some basic self-monitoring implemented as [custom checks](/monitoring/custom-checks/). These checks will be reported in ServicePulse alongside any custom checks being reported from endpoints.

#### MSMQ transactional dead letter queue

MSMQ servers have a single transactional dead letter queue. Messages that cannot be delivered to queues located on remote servers will eventually be moved to the transactional dead letter queue when the MSMQ service is unable to deliver the message. ServiceControl will monitor the transactional dead letter queue on the server it is installed on as the presence of messages in this queue may indicate a problem with delivering message retries.

#### Azure Service Bus staging dead letter queue

Azure Service Bus queues each come with an associated dead letter queue. When ServiceControl sends a message for retry it utilizes a staging queue to do so. ServiceControl will monitor the dead letter queue of the ServiceControl staging queue as the presence of messages in this queue indicates a problem with delivering message retries.

#### Failed imports

When ServiceControl is unable to properly import an audit or error message, the error is logged and the message is stored separately in ServiceControl. ServiceControl will monitor these failed import stores and notify when any are found. Read more about re-importing failed messages [here](/servicecontrol/import-failed-messages.md).
