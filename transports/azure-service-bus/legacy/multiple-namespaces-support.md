---
title: Multiple namespace support
summary: Configuring Azure Service Bus transport to support different partitioning strategies and destination namespaces
reviewed: 2019-10-06
component: ASB
versions: '[7,)'
redirects:
 - nservicebus/azure-service-bus/multiple-namespaces-support
 - transports/azure-service-bus/multiple-namespaces-support
---

include: legacy-asb-warning


Azure Service Bus transport supports configuring multiple Azure Service Bus namespaces, in order to:

 * Enable various _namespace partitioning strategies_ to cover scenarios such as High Availability and multiple Data Center support, or to overcome [Azure services limits](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-quotas).
 * Enable _cross namespace routing_ to endpoints outside of the partition set.

The namespace partitioning strategy can be configured using the `NamespacePartitioning()` configuration API section, where the cross namespace routing can be configured using the `NamespaceRouting()` API section. NServiceBus provides a few namespace partitioning strategies implementations. It's also possible to implement a [custom partitioning strategy](/transports/azure-service-bus/legacy/addressing-logic.md#namespace-partitioning-implementing-a-custom-namespace-partitioning-strategy) if needed.

NOTE: Using multiple namespace is currently NOT compatible with ServiceControl. A [ServiceControl transport adapter](/servicecontrol/transport-adapter/) or multiple installations of ServiceControl is required in order to leverage both.

## Single namespace partitioning

By default the Azure Service Bus transport uses the `SingleNamespacePartitioning` strategy, when it is configured using the `ConnectionString` extension method:

snippet: single_namespace_partitioning_strategy_with_default_connection_string

This is the functional equivalent of providing a namespace using the `AddNamespace` partitioning API with the default namespace alias and namespace's connection string.

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


### Combining High Availability and failover options 

To achieve high availability and failover, a custom strategy can be used. For example, a combination of round robin and failover strategies would ensure that messages are not throttled by the broker and sent when one of the namespaces is experiencing an outage.

partial: caching

## Cross namespace routing

NServiceBus allows to specify destination addresses using an `"endpoint@physicallocation"` in various places such as the [Send](/nservicebus/messaging/send-a-message.md) and [Routing](/nservicebus/messaging/routing.md) API or the `MessageEndpointMappings`. In this notation the `physicallocation` section represents the location where the endpoint's infrastructure is hosted, such as a machine name or a Service Bus namespace.

Using this notation it is possible to route messages to any endpoint hosted in namespaces that do not belong to the current endpoint's partition set.

snippet: namespace_routing_send_options_full_connectionstring

partial: alias

partial: registered-endpoint


### Default namespace alias

When using the `ConnectionString` method to configure a namespace, it will get an alias as well. This alias is represented by the `DefaultNamespaceAlias` configuration setting, which has a value of `default`.

When doing cross namespace request reply communication between endpoints configured this way, in combination with the `UseNamespaceAliasesInsteadOfConnectionStrings()` configuration method to [secure connection strings](securing-connection-strings.md), then the reply address header will include a value of `"sourceendpoint@default"`. However, the connection string that is mapped to this alias is different for each endpoint in the communication and it will break the request-reply pattern.

In order to overcome this problem, it is possible to change the value of the `DefaultNamespaceAlias` configuration setting using the API:

snippet: default_namespace_alias

or use `NamespacePartitioning().AddNamespace()` with a different alias instead of the `ConnectionString()` method in the source endpoint.

Also, ensure that the same alias and connection string are registered with the replying endpoint using the `NamespaceRouting().AddNamespace()` method.