---
title: Multiple namespace support
summary: Configuring Azure ServiceBus transport to support different partitioning strategies. 
component: ASB
tags:
- Cloud
- Azure
- Transports 
reviewed: 2016-04-26
---

Azure Service Bus transport enables different _namespace partitioning strategies_ to cover various scenarios such as High Availability and multiple Data Center support or to overcome [service limits](https://azure.microsoft.com/en-us/documentation/articles/service-bus-quotas/).  

_Namespace partitioning strategies_can be configured using Azure Service Bus configuration API, specifying namespace(s) to use.

## Single namespace partitioning

The default _namespace partitioning strategy_ is `SingleNamespacePartitioning`. With this strategy, transport uses only a single namespace to send and receive messages.  
Namespace connection string can be defined using `ConnectionString` extension method:

snippet: single_namespace_partitioning_strategy_with_default_connection_string

or by providing a map between namespace name and namespace's connection string.  

snippet: single_namespace_partitioning_strategy_with_add_namespace

Only one namespace can be configured when using SingleNamespacePartitioning strategy. When more than one namespace is specified, then NServiceBus throws a `ConfigurationErrorsException` at startup.   
`SingleNamespacePartitioning` strategy is the easiest one but it's the less reliable: the only configured namespace is a single point of failure for the entire system.

## Round robin namespace partitioning

Selecting `RoundRobinNamespacePartitioning` multiple namespace are used. Messages are sent to a single namespace and received from all the namespace. For sending operations, namespaces are cycled through.  
This strategy is designed to avoid throttling by the service.   
Multiple namespaces have to be configured when using `RoundRobinNamespacePartitioning` strategy. When only one namespace is specified, then NServiceBus throws a `ConfigurationErrorsException` at startup.
 
snippet: round_robin_partitioning_strategy

## Fail over namespace partitioning

`FailOverNamespacePartitioning` strategy provides transport High Availability by using two different namespaces, a primary and a secondary. The transport uses the primary namespace. When the primary namespace is not available, the transport will switch to the secondary namespace.   
Exactly two namespaces have to be configured when using `FailOverNamespacePartitioning` strategy. When only one namespace is specified, then NServiceBus throws a `ConfigurationErrorsException` at startup.

snippet: fail_over_partitioning_strategy