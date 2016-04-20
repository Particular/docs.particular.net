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

// INTRODUCTION HERE!!!

To configure _namespace partitioning strategy_ properly users have to select desired `INamespacePartitioningStrategy` implementation and map related namespaces, as showed in the next snippets, for each supported strategies.

### Single namespace partitioning #

The default _namespace partitioning strategy_ is `SingleNamespacePartitioning`. With the default setup, transports uses only a single namespace to send and receive messages.  
Users can provide namespace connection string directly with `ConnectionString` extension method:

snippet: single_namespace_partitioning_strategy_with_default_connection_string

or selecting explicitly the `SingleNamespacePartitioning` strategy and providing a map between namespace name and namespace connection string.  
Transport uses namespace name to avoid sharing namespace secrets.

snippet: single_namespace_partitioning_strategy_with_add_namespace

With this strategy, only one namespace has to be configured. Configuration throws a `ConfigurationErrorsException` if more than one namespace has been configured.   
`SingleNamespacePartitioning` strategy is the easiest one but it's the less relyable: the only configured namespace is a single point of failure for the entire system.

### Round robin namespace partitioning #

snippet: round_robin_partitioning_strategy

### Fail over namespace partitioning #

snippet: fail_over_partitioning_strategy

### Replicated namespace partitioning #

snippet: replicated_partitioning_strategy

### Sharded namespace partitioning #

snippet: sharded_partitioning_strategy_configuration

