ServiceControl is the monitoring brain in the Particular Service Platform. It collects data on every single message flowing through the system (Audit Queue), errors (Error Queue), as well as additional information regarding sagas, endpoints heartbeats and custom checks (Control Queue). The information is then exposed to [ServicePulse](/servicepulse) and [ServiceInsight](/serviceinsight) via an HTTP API and SignalR notifications.

NOTE: ServiceControl HTTP API is subject to changes and enhancements that may not be fully backward compatible. Use of this HTTP API is discouraged by 3rd parties.

It is important to understand that the data is still collected even if ServiceControl is down. When it starts working again, it will process all the information that was saved in the meantime.

To enable [ServiceControl](/servicecontrol) to gather this information, configure the solution appropriately:

 * [enable auditing](/nservicebus/operations/auditing.md) to collect data on individual messages;
 * configure [recoverability](/nservicebus/recoverability) to store information on messages failures;
 * [install plugins on the endpoints](/servicecontrol/plugins/) to monitor their health and sagas and use custom checks.

By default ServiceControl stores information for 30 days, but it can be [customized](/servicecontrol/creating-config-file.md).

Refer to the [Optimizing for use in different environments](/servicecontrol/servicecontrol-in-practice.md) article for more information about practical considerations.
