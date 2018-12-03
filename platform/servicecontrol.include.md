ServiceControl is a background process that collects useful information about an NServiceBus system. It uses an audit queue to collect every message flowing through the system and an error queue to collect failed messages. It can also collect saga state changes, endpoint [heartbeats](/monitoring/heartbeats/), and perform [custom checks](/monitoring/custom-checks/) using a control queue. This information is exposed to [ServicePulse](/servicepulse) and [ServiceInsight](/serviceinsight) via an HTTP API and SignalR notifications.

NOTE: The ServiceControl HTTP API may change at any time. It is designed for use by ServicePulse and ServiceInsight only. Use of this HTTP API for other purposes is not recommended.

Each NServiceBus endpoint can be configured to send data about its operation to a set of centralized queues that are unique to the system being monitored. These queues are monitored by a [ServiceControl instance](/servicecontrol/servicecontrol-instances/) which collects and processes this data. ServiceControl instances are created and managed using the ServiceControl Management Utility.

Note that data is still collected even if the ServiceControl instance is down. When it starts working again, it will process the information that was saved while it was offline.

To enable [ServiceControl](/servicecontrol) to gather this information, configure the solution appropriately:

 * [enable auditing](/nservicebus/operations/auditing.md) to collect data on individual messages;
 * configure [recoverability](/nservicebus/recoverability) to store information on messages failures;
 * [install plugins on the endpoints](/servicecontrol/plugins/) to monitor their health and sagas and use custom checks.

NOTE: ServiceControl _consumes_ messages that arrive in either the configured audit or error queues, i.e. it removes those messages from the queues. If a copy of those messages is required for further processing, configure [audit forwarding](/servicecontrol/creating-config-file.md#transport-servicecontrolforwardauditmessages) and/or [error queue forwarding](/servicecontrol/creating-config-file.md#transport-servicecontrolforwarderrormessages).

By default ServiceControl stores information for 30 days, but this period can be [customized](/servicecontrol/creating-config-file.md).

See [Optimizing ServiceControl for use in different environments](/servicecontrol/servicecontrol-in-practice.md) for more information about practical considerations.
