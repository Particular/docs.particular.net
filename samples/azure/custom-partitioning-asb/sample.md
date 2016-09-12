---
title: Custom ASB Namespace Partitioning
summary: Determine which namespace Azure Service Bus Transport uses for partitioning.
component: ASB
reviewed: 2016-09-06
related:
 - nservicebus/azure-service-bus
 - nservicebus/azure-service-bus/sanitization
 - nservicebus/azure-service-bus/topologies
 - nservicebus/messaging/publish-subscribe
 - samples/azure/azure-service-bus
---


## Prerequisites

include: asb-connectionstrings


include: asb-transport


## Code walk-through

This sample has two endpoints

* `Publisher`
* `Subscriber`


## Publisher

`Publisher` publishes `SomeEvent`.

snippet: SomeEvent

using a custom partitionining strategy, named replicated partitioning strategy, which decides that events should be published to multiple namespaces.


## Subscriber

There are 2 identical instances of `Subscriber`, each instance subscribes to only one of the namespaces using the default `SingleNamespacePartitioningStrategy` and handles `SomeEvent`. 


### Creating a custom partitioning strategy

For the purpose of this sample, a custom partitioning strategy is used which simply decides that all namespaces configured should be used for all intents. In other words: all sends, receives and create operations will be executed on all registered namespaces.

snippet: replicated-namespace-partitioning-strategy


#### Registering custom strategy

A custom namespace strategy is registered using `NamespacePartitioning().UseStrategy<T>`. Note that multiple namespaces can be registered using the `NamespacePartitioning().AddNamespace()` API:

snippet: CustomPartitioning


## Running the sample

 * Start 2 instances of this subscriber each with a different namespace.
 * Start 1 instance of the publisher
 * Both subscribers will receive the event
