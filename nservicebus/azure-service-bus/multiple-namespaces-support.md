---
title: Multiple namespace support
summary: Configuring Azure ServiceBus transport to support different partitioning strategies. 
component: ASB
tags:
- Cloud
- Azure
- Transports 
reviewed: 2016-04-18
---

`Azure Service Bus` transport enables different _namespace partitioning strategies_ to cover different scenarios such as High Availability and multiple Data Center support or to overcome [service limits](https://azure.microsoft.com/en-us/documentation/articles/service-bus-quotas/).  

_Namespace partitioning strategies_can be configured using Azure Service Bus configuration API, specifying namespace(s) to use.

## Single namespace partitioning #

The default _namespace partitioning strategy_ is `SingleNamespacePartitioning`. With this strategy, transport uses only a single namespace to send and receive messages.  
Namespace connection string can be defined via code using `ConnectionString` extension:

snippet: single_namespace_partitioning_strategy_with_default_connection_string

or selecting explicitly the `SingleNamespacePartitioning` strategy and providing a map between namespace name and namespace connection string.  
Transport uses namespace name to avoid sharing namespace secrets.

snippet: single_namespace_partitioning_strategy_with_add_namespace

With this strategy, only one namespace has to be configured. Configuration throws a `ConfigurationErrorsException` if more than one namespace has been configured.   
`SingleNamespacePartitioning` strategy is the easiest one but it's the less relyable: the only configured namespace is a single point of failure for the entire system.

## Round robin namespace partitioning #

Selecting `RoundRobinNamespacePartitioning` multiple namespace are used. Messages are sent to a single namespace and received from all the namespace. For sending operations, namespaces are cycled through.  
This strategy is useful to avoid throttling by Azure infrastructure for capacity limit.
`RoundRobinNamespacePartitioning` strategy requires more than one configured namespace. Configuration throws a `ConfigurationErrorsException` if only one namespace has been configured. 

snippet: round_robin_partitioning_strategy

## Fail over namespace partitioning #

`FailOverNamespacePartitioning` strategy provides transport High Availability by using two different namespaces, a primary and a secondary. The transport uses the primary namespace. When the primary namespace is not available, the transport will switch to the secondary namespace.
`FailOverNamespacePartitioning` strategy requires exactly two namespaces. Configuration throws a `ConfigurationErrorsException` if number of namespaces is different than two.

snippet: fail_over_partitioning_strategy 


## Replicated namespace partitioning #

Scenario covered by `RepliactedNamespacePartitioning` is to support publish messages across different data centers. With this strategy, transport uses all configured namespaces to send and receive messages.  
When transport has to send a message, it sends the same message to all namespaces.
`RepliactedNamespacePartitioning` strategy requires more than one configured namespace. Configuration throws a `ConfigurationErrorsException` if only one namespace has been configured. 

snippet: replicated_partitioning_strategy

## Sharded namespace partitioning #

With `ShardedNamespacePartitioning` messages are sent to the namespace selected by the custom `ShardingRule` defined and provided to the strategy.
`ShardedNamespacePartitioning` strategy requires more than one configured namespace. Configuration throws a `ConfigurationErrorsException` if only one namespace has been configured. 

snippet: sharded_partitioning_strategy_configuration



