---
title: NServiceBus features requiring usage of Transport Adapter
summary: What features of NServiceBus require usage of Transport Adapter when connecting to ServiceControl
reviewed: 2017-06-28
---

Some features of NServiceBus, particularly related to physical routing of messages, cannot be supported by ServiceControl. The reason for not supporting them is the fact that these features require extensive code-based configuration and ServiceControl is a stand-alone service. The Transport Adapter is designed to bridge the gap. Transport Adapter is provided as a library package (rather than stand-alone service) so users can customize the transport the same way as they do for the regular business endpoints.


## SQL Server transport

The [multi-instance](/transports/sql/deployment-options.md#modes-overview-multi-instance) where endpoints connect to different instances of SQL Server is not supported because ServiceControl can't send a retried failed message to the endpoint's own database.


## RabbitMQ transport

Neither [direct topology](/transports/rabbitmq/routing-topology.md#direct-routing-topology) nor [custom topologies](/transports/rabbitmq/routing-topology.md#custom-routing-topology) are supported as ServiceControl is configured to use the default topology which expects an exchange exists with the name same as the destination endpoint's name.


## Azure Service Bus

 * Using [aliases](/transports/azure-service-bus/securing-connection-strings.md) instead of connection string will make it impossible to retry messages from ServiceControl.
 * Leveraging [multiple namespaces in a topology](/transports/azure-service-bus/multiple-namespaces-support.md) will require a setup of multiple ServiceControl instances.
 * Customizing [brokered message creation](/transports/azure-service-bus/brokered-message-creation.md) may lead to incompatible wire formats and deserialization errors.


## Azure Storage Queues

 * Using [aliases](/transports/azure-storage-queues/configuration.md#connection-strings-using-aliases-for-connection-strings-to-storage-accounts) instead of connection string will make it impossible to retry messages from ServiceControl.
 * Leveraging [multiple storage accounts](/transports/azure-storage-queues/multi-storageaccount-support.md) will require a setup of multiple ServiceControl instances.
 * Choosing a [non default serializer for the message wrapper](/transports/azure-storage-queues/configuration.md#configuration-parameters-serializemessagewrapperwith) or using a [custom envelope wrapper](/transports/azure-storage-queues/configuration.md#custom-envelope-unwrapper) may lead to incompatible wire formats.
