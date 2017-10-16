ServiceControl is a suite of back-end tools that collect useful information about a running NServiceBus system. It collects data on every single message flowing through the system (Audit Queue), errors (Error Queue), as well as additional information regarding sagas, endpoints heartbeats and custom checks (Control Queue). The information is then exposed to [ServicePulse](/servicepulse) and [ServiceInsight](/serviceinsight) via an HTTP API and SignalR notifications.

NOTE: The ServiceControl HTTP API may change at any time. It is designed for use by ServicePulse and ServiceInsight only. Use of this HTTP API for other purposes is discouraged.

Each NServiceBus endpoint can be configured to send data about its operation to a set of centralized queues that are unique to the system being monitored. These queues are monitored by a [ServiceControl instance](/servicecontrol/servicecontrol-instances/) which collects and processes this data. ServiceControl instances are created and managed using the ServiceControl Management Utility.

NOTE: All endpoints should be configured to forward to the same audit, error, and ServiceControl instance queues unless the system has been sharded between multiple ServiceControl instances.

It is important to understand that the data is still collected even if the ServiceControl instance is down. When it starts working again, it will process all the information that was saved in the meantime.

To enable [ServiceControl](/servicecontrol) to gather this information, configure the solution appropriately:

 * [enable auditing](/nservicebus/operations/auditing.md) to collect data on individual messages;
 * configure [recoverability](/nservicebus/recoverability) to store information on messages failures;
 * [install plugins on the endpoints](/servicecontrol/plugins/) to monitor their health and sagas and use custom checks.

NOTE: ServiceControl _consumes_ messages that arrive in either the configured audit or error queues, i.e. it removes those messages from the queues. If a copy of those messages is required for further processing, configure [audit forwarding](/servicecontrol/creating-config-file.md#transport-servicecontrolforwardauditmessages) and/or [error queue forwarding](/servicecontrol/creating-config-file.md#transport-servicecontrolforwarderrormessages).

By default ServiceControl stores information for 30 days, but it can be [customized](/servicecontrol/creating-config-file.md).

Refer to the [Optimizing for use in different environments](/servicecontrol/servicecontrol-in-practice.md) article for more information about practical considerations.
