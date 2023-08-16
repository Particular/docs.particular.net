---
title: N-tier architecture style on Azure
summary:
reviewed: 2023-07-18
---

![](azure-layered-architecture.png)

N-tiered architectures, often also called layered architectures, is a very common architectural style which groups code into technical partitions (layers) that often represent organizational structures too. Layers have a strict vertical order and lower level layers are not allowed to call into higher level layers. Layers can be deployed together or individually, which can make this architectural style work for both monolithic as well as distributed applications. Layered architectures are often chosen for small, simple applications with tight budget or time constraints. Once layered applications grow beyond a certain level, switching to domain-partitioned architectural styles like microservices and event-driven architecture is a common evolutionary step.

### Components

Common components of N-tiered architectures:
* Front end layer: Often these are web applications or Desktop UIs phyiscally separated from the other layers.
* Data layer: One or more databases containing all data models of the application.
* Business logic layer: Contains the business logic
* Message queue: pass down commands from upper to lower layers or to raise events from lower layers

### Challenges

Physically separating the tiers improves scalability of the application but also introduces higher exposure to network related issues that might affect availability. Message queues can help to decouple the layers and increase resiliency across the layers.

Typically, layered architectures heavily use synchronous requests across all layers to execute business processes. Long-running or heavy workloads can negatively impact the user experience and overall system performance. Asynchronous processes free up the caller from the workload.

Front end layers often need to react to changes that were triggered by other users or processes. Notification from the lower layers can be tricky due to the one-way dependency constraints, especially on phyiscally separated layers. Message queues provide a simple approach to event notifications from lower layers without breaking the dependency hierarchy.

### Technology choices

TBD

## Related content

* [Azure Architecture Center—N-tier architecture style](https://learn.microsoft.com/en-us/azure/architecture/guide/architecture-styles/n-tier)
