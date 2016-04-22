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

`Azure Service Bus` transport enables different _namespace partitioning strategies_ to cover different scenarios like High Availability and multiple Data Center support or to overtake [service limits](https://azure.microsoft.com/en-us/documentation/articles/service-bus-quotas/).  

Configuration APIs give the possibility to select desired _namespace partitioning strategies_ and then to configure specific namespace(s) to use to send and to receive messages through.

## Single namespace partitioning ##

The default _namespace partitioning strategy_ is `SingleNamespacePartitioning`. With the default setup, transport uses only a single namespace to send and receive messages.  
Namespace connection string can be defined via code using `ConnectionString` extension:

snippet: single_namespace_partitioning_strategy_with_default_connection_string

or selecting explicitly the `SingleNamespacePartitioning` strategy and providing a map between namespace name and namespace connection string.  
Transport uses namespace name to avoid sharing namespace secrets.

snippet: single_namespace_partitioning_strategy_with_add_namespace

With this strategy, only one namespace has to be configured. Configuration throws a `ConfigurationErrorsException` if more than one namespace has been configured.   
`SingleNamespacePartitioning` strategy is the easiest one but it's the less relyable: the only configured namespace is a single point of failure for the entire system.

## Round robin namespace partitioning ##

Another possible strategy is `RoundRobinNamespacePartitioning`. With this strategy, transport uses each registered namespace cyclically to send messages and it receives messages from all of them.  
This strategy is useful to avoid throttling by Azure infrastructure for capacity limit.
`RoundRobinNamespacePartitioning` strategy needs more than one configured namespace. Configuration throws a `ConfigurationErrorsException` if only one namespace has been configured. 

snippet: round_robin_partitioning_strategy

## Fail over namespace partitioning ##

`FailOverNamespacePartitioning` strategy covers High Availability scenario: configuring two different namespaces, a primary and a secondary, transport uses the first one to read or send messages if it's reachable. Otherwise transport switches to secondary.
`FailOverNamespacePartitioning` strategy needs exactly two namespaces. Configuration throws a `ConfigurationErrorsException` if zero, one or more than two namespace has been configured. 

snippet: fail_over_partitioning_strategy 


## Replicated namespace partitioning ##

Scenario covered by `RepliactedNamespacePartitioning` is to support publish messages across different data centers. With this strategy, transport uses all configured namespaces to send and receive messages.  
When transport has to publish a message, it publishes the same message to all namespaces.
`RepliactedNamespacePartitioning` strategy needs more than one configured namespace. Configuration throws a `ConfigurationErrorsException` if only one namespace has been configured. 

snippet: replicated_partitioning_strategy

## Sharded namespace partitioning ##

`ShardedNamespacePartitioning` is a specification of `RoundRobinNamespacePartitioning`: transport doesn't decide where to publish messages but it's up to user define a `ShardingKey` to route messages to a namespace or to another before publishing a message.
`ShardedNamespacePartitioning` strategy needs more than one configured namespace. Configuration throws a `ConfigurationErrorsException` if only one namespace has been configured. 

snippet: sharded_partitioning_strategy_configuration



