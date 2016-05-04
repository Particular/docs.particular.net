---
title: Multiple namespace support
summary: Configuring Azure Service Bus transport to support different partitioning strategies. 
component: ASB
tags:
- Cloud
- Azure
- Transports 
reviewed: 2016-04-26
---

Azure Service Bus transport enables various _namespace partitioning strategies_ to cover scenarios such as High Availability and multiple Data Center support, or to overcome [Azure services limits](https://azure.microsoft.com/en-us/documentation/articles/service-bus-quotas/).  

The namespace partitioning strategy can be configured using Azure Service Bus transport configuration API.

## Single namespace partitioning

The `SingleNamespacePartitioning` is the default namespace partitioning strategy, and is the easiest one to set up.

The namespace connection string can be defined using `ConnectionString` extension method:

snippet: single_namespace_partitioning_strategy_with_default_connection_string

or by providing a map between namespace name and namespace's connection string.  

snippet: single_namespace_partitioning_strategy_with_add_namespace

With this strategy, the transport uses only a single namespace to send and receive messages, so only one namespace can be configured. When more than one namespace is specified, then NServiceBus throws a `ConfigurationErrorsException` at startup.


## Round robin namespace partitioning

The `RoundRobinNamespacePartitioning` can be used to avoid throttling by the service. With this strategy, the transport uses multiple namespaces. Messages are sent to a single namespace and received from all namespaces. For sending operations, namespaces are used in a round-robin fashion.  
 
snippet: round_robin_partitioning_strategy
  
Multiple namespaces have to be configured when using `RoundRobinNamespacePartitioning` strategy. When only one namespace is specified, then NServiceBus throws a `ConfigurationErrorsException` at startup.


## Fail over namespace partitioning

The `FailOverNamespacePartitioning` can be used to provide High Availability. It uses two different namespaces: a primary and a secondary. The transport uses the primary namespace by default, and switches to secondary one when the primary is not available.

snippet: fail_over_partitioning_strategy
  
Exactly two namespaces have to be configured when using `FailOverNamespacePartitioning` strategy. When only one namespace is specified, then NServiceBus throws a `ConfigurationErrorsException` at startup.
