---
title: Multiple namespace support
summary: Configuring Azure Service Bus transport to support different partitioning strategies and destination namespaces
component: ASB
reviewed: 2016-07-05
tags:
- Cloud
- Azure
- Transports
---


## Multiple namespaces support

Azure Service Bus transport supports configuring multiple Azure Service Bus namespaces, in order to:

 * Enable various _namespace partitioning strategies_ to cover scenarios such as High Availability and multiple Data Center support, or to overcome [Azure services limits](https://azure.microsoft.com/en-us/documentation/articles/service-bus-quotas/).
 * Enable _cross namespace routing_ to endpoints outside of the partition set.

The namespace partitioning strategy can be configured using the `NamespacePartitioning()` configuration API section, where the cross namespace routing can be configured using the `NamespaceRouting()` API section.


## Single namespace partitioning

By default the Azure Service Bus transport uses the `SingleNamespacePartitioning` strategy, when it is configured using the `ConnectionString` extension method:

snippet: single_namespace_partitioning_strategy_with_default_connection_string

This is the functional equivalent of providing a namespace using the `AddNamespace` partitioning API with the default namespace name and namespace's connection string.

snippet: single_namespace_partitioning_strategy_with_add_namespace

With this strategy, the transport uses only a single namespace to send and receive messages, hence only one namespace can be configured for the purpose of partitioning. When more than one namespace, or none, is specified for partitioning, then a [ConfigurationErrorsException](https://msdn.microsoft.com/en-us/library/system.configuration.configurationerrorsexception.aspx) will be thrown at startup.


## Round robin namespace partitioning

The `RoundRobinNamespacePartitioning` can be used to avoid throttling by the service. With this strategy, the transport uses multiple namespaces for the communication between endpoints. Messages are sent to a single namespace and received from all namespaces. For sending operations, namespaces are chosen in a round-robin fashion.

snippet: round_robin_partitioning_strategy

Multiple namespaces have to be configured when using `RoundRobinNamespacePartitioning` strategy. When less than two namespaces are specified, then a [ConfigurationErrorsException](https://msdn.microsoft.com/en-us/library/system.configuration.configurationerrorsexception.aspx) will be thrown at startup.


## Fail over namespace partitioning

The `FailOverNamespacePartitioning` can be used to provide High Availability. It uses two different namespaces: a primary and a secondary. The transport uses the primary namespace by default, and switches to secondary one when the primary is not available.

snippet: fail_over_partitioning_strategy

Exactly two namespaces have to be configured when using `FailOverNamespacePartitioning` strategy. When only one or more than two namespaces are specified, then a [ConfigurationErrorsException](https://msdn.microsoft.com/en-us/library/system.configuration.configurationerrorsexception.aspx) will be thrown at startup.


## Cross namespace routing

NServiceBus allows to specify destination addresses using an `"endpoint@physicallocation"` in various places such as the [Send](/nservicebus/messaging/send-a-message.md) and [Routing](/nservicebus/messaging/routing.md) API or the [MessageEndpointMappings](/nservicebus/messaging/message-owner.md). In this notation the `physicallocation` section represents the location where the endpoint's infrastructure is hosted, such as a machine name or a servicebus namespace.

Using this notation it is possible to route messages to any endpoint hosted in namespaces that do not belong to the current endpoint's partition set.

snippet: namespace_routing_send_options_full_connectionstring

Versions 7 and above of the Azure Service Bus transport it is also possible to name namespaces, and use those namespace names instead of the connectionstring value; in all of these places.

snippet: namespace_routing_send_options_named

This requires the namespace name and connectionstring to be registered using the `NamespaceRouting()` API.

snippet: namespace_routing_registration


### Default namespace name

When using the `ConnectionString` method to configure a namespace, it will get a name as well. This name is represented by the `DefaultNamespaceName` configuration setting, which has a value of `default`.

When doing cross namespace request reply communication between endpoints configured this way, in combination with the `UseNamespaceNamesInsteadOfConnectionStrings()` configuration method to [secure connection strings](securing-connection-strings.md), then the reply address header will include a value of `"sourceendpoint@default"`. However the connectionstring that is mapped to this name is different for each endpoint in the communication and it will break the request reply pattern.

In order to overcome this problem, it is possible to change the value of the `DefaultNamespaceName` configuration setting using the API:

snippet: default_namespace_name

or use `NamespacePartitioning().AddNamespace()` with a different name instead of the `ConnectionString()` method in the source endpoint.

And ensure that the same name and connectionstring are registered in the replying endpoint using the `NamespaceRouting().AddNamespace()` method.