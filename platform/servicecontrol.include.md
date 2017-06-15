ServiceControl is the brain in the Particular Platform. ServiceControl collects information about messages flowing through the system by ingesting faulted messages forwarded to the error queue and audits of successful messages forwarded to the audit queue. In addition ServiceControl gathers additional information regarding sagas, endpoints heartbeats, custom checks, and metric data sent to the ServiceControl queue via plugins.

There are two types of ServiceControl instances that can be installed. Regualar ServiceControl instances collect error and audit information and manages retries. ServiceControl Monitoring instances collect metric data and monitor the health of running endpoints. A typical installation of ServiceControl will contain one of each instance.

ServiceControl exposes data to [ServicePulse](/servicepulse) and [ServiceInsight](/serviceinsight) via an HTTP API and SignalR notifications.

NOTE: The ServiceControl HTTP APIs are subject to changes and enhancements that may not be fully backward compatible. Use of these HTTP APIs is discouraged by 3rd parties.

It is important to understand that the data is still collected even if all ServiceControl instances are offline. When the instances resume operation, they will process all the information that has been collected in their input queues.

## ServiceControl instances

To enable [ServiceControl](/servicecontrol) to collect information on the system, [create a ServiceControl instance](/servicecontroll/installation.md) and configure the solution endpoints appropriately:

 * [enable auditing](/nservicebus/operations/auditing.md) to collect data on individual messages;
 * configure [recoverability](/nservicebus/recoverability) to store information on messages failures;
 * [install plugins on the endpoints](/servicecontrol/plugins/) to monitor their health and sagas and use custom checks.

NOTE: All endpoints should be configured to forward to the same audit, error, and ServiceControl plugin queues unless the system has been sharded between multiple ServiceControl instances.

NOTE: ServiceControl _consumes_ messages that arrive in either the configured audit or error queues, i.e. it removes those messages from the queues. If a copy of those messages is required for further processing, configure [audit forwarding](/servicecontrol/creating-config-file.md#transport-servicecontrolforwardauditmessages) and/or [error queue forwarding](/servicecontrol/creating-config-file.md#transport-servicecontrolforwarderrormessages).

By default ServiceControl stores information for 30 days, but it can be [customized](/servicecontrol/creating-config-file.md).

Refer to the [Optimizing for use in different environments](/servicecontrol/servicecontrol-in-practice.md) article for more information about practical considerations.

## ServiceControl Monitoring instances

To enable [ServiceControl](/servicecontrol) to collect metrics data [create a ServiceControl Monitoring instance](/servicecontroll/installation.md) and [configure solution endpoints to send metrics data to it](/servicecontrol/configure-endpoints-for-monitoring.md).

NOTE: All endpoints should be configured to send metrics to the same ServiceControl Monitoring instance, unless the system has been sharded between multiple ServiceControl Monitoring instances..
