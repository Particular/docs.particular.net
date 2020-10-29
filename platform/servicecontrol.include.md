ServiceControl is a background process that collects useful information about an NServiceBus system. It uses an audit queue to collect every message flowing through the system and an error queue to collect failed messages. It can also collect saga state changes, endpoint [heartbeats](/monitoring/heartbeats/), and perform [custom checks](/monitoring/custom-checks/) using a control queue. This information is exposed to [ServicePulse](/servicepulse) and [ServiceInsight](/serviceinsight) via an HTTP API and SignalR notifications.

ServiceControl can also be configured to collect detailed performance metrics for display in ServicePulse.

NOTE: The ServiceControl HTTP API may change at any time. It is designed for use by ServicePulse and ServiceInsight only. The use of this HTTP API for other purposes is not recommended.

NServiceBus endpoints can be configured to send data about their operations to a set of centralized queues that are unique to the system. A [ServiceControl instance](/servicecontrol/servicecontrol-instances/) monitors these queues, and collects and processes this data. ServiceControl instances are created and managed using the ServiceControl Management Utility.

Note that data is still collected even if the ServiceControl instance is down. When it starts working again, it will process the information that was saved while it was offline.

To enable [ServiceControl](/servicecontrol) to gather this information, configure the endpoints appropriately:

 * [Enable auditing](/nservicebus/operations/auditing.md) to collect all messages.
 * [Configure recoverability](/nservicebus/recoverability) to collect failed messages.
 * [Install plugins](/servicecontrol/plugins/) to monitor endpoint health, collect saga state changes, and use custom checks.

NOTE: ServiceControl _consumes_ messages from the audit and error queues. That is, it removes all messages from those queues. If a copy of those messages is required for further processing, configure [audit forwarding](/servicecontrol/audit-instances/creating-config-file.md#transport-servicecontrol-auditforwardauditmessages) and/or [error queue forwarding](/servicecontrol/creating-config-file.md#transport-servicecontrolforwarderrormessages).

See [_Optimizing ServiceControl for use in different environments_](/servicecontrol/servicecontrol-in-practice.md) for more information about practical considerations.
