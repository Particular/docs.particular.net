---
title: Custom ASB Namespace Partitioning
summary: Determine which namespace Azure Service Bus Transport uses for partitioning.
component: ASB
reviewed: 2018-01-05
related:
 - transports/azure-service-bus
 - transports/azure-service-bus/sanitization
 - transports/azure-service-bus/topologies
 - nservicebus/messaging/publish-subscribe
 - samples/azure/azure-service-bus
---


## Prerequisites

Two environment variables named `AzureServiceBus.ConnectionString1` and `AzureServiceBus.ConnectionString2` with a different connection string to an Azure Service Bus namespace each.

include: asb-transport


## Code walk-through

This sample has three endpoints

* `Publisher`
* `Subscriber1`
* `Subscriber2`


## Publisher

`Publisher` publishes `SomeEvent`.

snippet: SomeEvent

using a custom partitionining strategy registered at startup time. Events are published to the appropreate namespace(s) based on the registered strategy which decides that events should be published to multiple namespaces.

By default, the sample is configured to use data distribution strategy. Data distribution strategy publishes events to all registered namespaces. 

Alternative strategy is Round-Robin with failover strategy. This strategy demonstrates how to achieve high availability and disaster recoverability. 


## Subscriber

There are 2 identical instances of `Subscriber`, each instance subscribes to only one of the namespaces using the default `SingleNamespacePartitioningStrategy` and handles `SomeEvent`. 


### Creating a custom partitioning strategy

#### Data distribution strategy

`DataDistributionPartitioningStrategy` is used which simply decides that all namespaces configured should be used for all intents. In other words: all sends, receives and create operations will be executed on all registered namespaces.

snippet: data-distribution-strategy


#### Round-Robin with failover strategy

`RoundRobinWithFailoverPartitioningStrategy` is alternating between two namespaces to distribute load among namespaces and ensure no throttling is taking place on the Azure Service Bus service level. This achieves high availability. Should one of the namespace fail, the strategy will fail over to use the other namespace. This achieves Disaster Recovery.

snippet: roundrobin-with-failover-strategy


#### Registering custom strategy

A custom namespace strategy is registered using `NamespacePartitioning().UseStrategy<T>`. Note that multiple namespaces can be registered using the `NamespacePartitioning().AddNamespace()` API:

snippet: CustomPartitioning
snippet: CustomPartitioning_DataDistributionStrategy
or
snippet: CustomPartitioning_RoundRobinWithFailoverStrategy


## Running the sample

 * Start 2 instances of this subscriber each with a different namespace.
 * Start 1 instance of the publisher
 * Both subscribers will receive the event

### Choosing partitioning strategy

`DataDistributionPartitioningStrategy` is the default strategy and can be replaced with  `RoundRobinWithFailoverPartitioningStrategy` upon `Publisher` startup.

### Emulating namespace failure

The namespace failure can be toggled to emulate an outage for one of the namespaces represented by `AzureServiceBus.ConnectionString1` connection string. When namespace is in failed state, events published to namespace with connection string `AzureServiceBus.ConnectionString1` should be delivered to the second namespace (namespace with connection string `AzureServiceBus.ConnectionString2`).  

