---
title: NServiceBus features requiring usage of Transport Adapter
summary: What features of NServiceBus require usage of Transport Adapter when connecting to ServiceControl
---

Some features of NServiceBus, particularly related to physical routing of messages, cannot be supported by ServiceControl. The reason for not supporting them is the fact that these features require extensive code-based configuration and ServiceControl is a stand-alone service. The Transport Adapter is designed to bridge the gap. Transport Adapter is provided as a library package (rather than stand-alone service) so users can customize the transport the same way as they do for the regular business endpoints.


## SQL Server transport

The [multi-instance](/nservicebus/sqlserver/deployment-options.md#modes-overview-multi-instance) where endpoints connect to different instances of SQL Server is not supported because ServiceControl can't send a retried failed message to the endpoint's own database.


## RabbitMQ transport

Neither [direct topology](/nservicebus/rabbitmq/routing-topology.md#direct-routing-topology) nor [custom topologies](/nservicebus/rabbitmq/routing-topology.md#custom-routing-topology) are supported as ServiceControl is configured to use the default topology which expects an exchange exists with the name same as the destination endpoint's name.
