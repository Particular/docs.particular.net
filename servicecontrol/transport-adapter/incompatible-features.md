---
title: NServiceBus Features Requiring a Transport Adapter
summary: Which features of NServiceBus require a transport adapter when connecting to ServiceControl
reviewed: 2020-05-18
---

Some features of NServiceBus, particularly related to physical routing of messages, cannot be supported by ServiceControl. The reason for not supporting them is the fact that these features require extensive code-based configuration and ServiceControl is a standalone service. Transport adapters are designed to bridge the gap. [ServiceControl.TransportAdapter](https://www.nuget.org/packages/ServiceControl.TransportAdapter/) is provided as a library package (rather than standalone service) so users can customize the transport the same way as for regular business endpoints.


## SQL Server transport

[Multi-instance](/transports/sql/deployment-options.md?version=SqlTransportLegacySystemClient_3#multi-instance.md), where endpoints connect to different instances of SQL Server, is not supported. Multi-catalog and multi-schema modes are supported in ServiceControl version 3.0 and above.


## RabbitMQ transport

[Custom topologies](/transports/rabbitmq/routing-topology.md#custom-routing-topology) are not supported.


## Azure Service Bus transport (Legacy)

 * Using [aliases](/transports/azure-service-bus/legacy/securing-connection-strings.md) instead of a connection string is not supported as ServiceControl is not able to retry messages.
 * Leveraging [multiple namespaces in a topology](/transports/azure-service-bus/legacy/multiple-namespaces-support.md) requires setup of multiple ServiceControl instances.
 * Customizing [brokered message creation](/transports/azure-service-bus/legacy/brokered-message-creation.md) may lead to incompatible wire formats and deserialization errors.
 * Customizing entity paths by using [namespace hierarchy](/transports/azure-service-bus/legacy/namespace-hierarchy.md) is not supported.


## Azure Storage Queues transport

 * Using [aliases](/transports/azure-storage-queues/configuration.md#connection-strings-using-aliases-for-connection-strings-to-storage-accounts) instead of connection string is not supported as ServiceControl is not able to retry messages.
 * Leveraging [multiple storage accounts](/transports/azure-storage-queues/multi-storageaccount-support.md) requires setup of multiple ServiceControl instances.
 * Choosing a [non-default serializer for the message wrapper](/transports/azure-storage-queues/configuration.md#configuration-parameters-serializemessagewrapperwith) or using a [custom envelope wrapper](/transports/azure-storage-queues/configuration.md#custom-envelope-unwrapper) may lead to incompatible wire formats.
 