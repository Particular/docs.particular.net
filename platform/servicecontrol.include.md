Two different windows services that act as the brain in the Particular Service Platform.

ServiceControl collects information about messages flowing through the system by injesting faulted messages forwarded to the error queue and audits of successful messages forwarded to the audit queue. In addition ServiceControl gathers additional information regarding sagas, endpoints heartbeats and custom checks sent to the ServiceControl queue via plugins. 

ServiceMonitor collects metric data provided by NServiceBus endpoints to monitor endpoint health and performance via messages sent to the monitoring queue.

All of these services expose their data to [ServicePulse](/servicepulse) and [ServiceInsight](/serviceinsight) via an HTTP API and SignalR notifications.

NOTE: The ServiceControl and ServiceMonitor HTTP APIs are subject to changes and enhancements that may not be fully backward compatible. Use of these HTTP APIs is discouraged by 3rd parties.

It is important to understand that the data is still collected even if ServiceControl or ServiceMonitor are down. When the service starts working again, it will process all the information that was saved in the meantime.

## ServiceControl

To enable [ServiceControl](/servicecontrol) to collect information on the system, configure the solution endpoints appropriately:

 * [enable auditing](/nservicebus/operations/auditing.md) to collect data on individual messages;
 * configure [recoverability](/nservicebus/recoverability) to store information on messages failures;
 * [install plugins on the endpoints](/servicecontrol/plugins/) to monitor their health and sagas and use custom checks.

NOTE: All endpoints should be configured to forward to the same audit, error, and ServiceControl plugin queues unless the system has been sharded between multiple ServiceControl instances.

NOTE: ServiceControl _consumes_ messages that arrive in either the configured audit or error queues, i.e. it removes those messages from the queues. If a copy of those messages is required for further processing, configure [audit forwarding](/servicecontrol/creating-config-file.md#transport-servicecontrolforwardauditmessages) and/or [error queue forwarding](/servicecontrol/creating-config-file.md#transport-servicecontrolforwarderrormessages).

By default ServiceControl stores information for 30 days, but it can be [customized](/servicecontrol/creating-config-file.md).

Refer to the [Optimizing for use in different environments](/servicecontrol/servicecontrol-in-practice.md) article for more information about practical considerations.

## ServiceMonitor

To enable [ServiceMonitor](/servicecontrol) to collect metrics data [add the NServiceBus.Metrics](/nservicebus/operations/metrics.md) package to the solution endpoints and [configure them to send metrics data to ServiceMonitor](/servicecontrol/configure-nservicebusmetrics-for-servicemonitor.md).

NOTE: All endpoints should be configured to send metrics to the same ServiceMonitor instance, unless the system has been sharded between multiple ServiceMonitor instances.
