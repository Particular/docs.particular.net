---
title: NServiceBus Router two-way bridge topology
summary: How to connect two transports with a router
component: Router
related:
- samples/router/mixed-transports
reviewed: 2018-12-28
---

In the simplest form, the Router can be used to create a bi-directional bridge between two NServiceBus transports.

![Bridge](bridge.svg)

The arrows show the path of messages sent from `Endpoint A` to `Endpoint C` and from `Endpoint D` to `Endpoint B`. Each message is initially sent to the router queue and then forwarded to the destination queue. There is one additional *hop* compared to a direct communication between endpoints. The following snippet configures the built-in *static routing protocol* to forward messages between the router's interfaces.

snippet: simple-routing

To learn more about simple bridge topology see [the mixed transports sample](/samples/router/mixed-transports). 

### Case study: Legacy

When a system is being maintained and developed over an extended period of time, the original decision about the transport technology, which was sound at the time, might become a burden or even an obstacle e.g. usage of MSMQ prohibits hosting on Linux.

Instead of a big bang migration from the old transport to the new, the Router allows for gradual migration. New endpoints added to the system use the new transport and communicate with the legacy part via a router. Over time the legacy endpoints get decommissioned or migrated to the new transport and eventually the router might even become redundant. Most systems will, however, retain a legacy part for extended periods of time, if not forever.

### Case study: Cloud

When a new component of an existing system is being built in the cloud, it is likely going to use a different transport than the existing on-premises components. The reason for this is the fact that cloud-native messaging infrastructure such as SQS, Azure Storage Queues or Azure ServiceBus are much more cost-effective to use compared to self-hosting traditional messaging systems in the cloud.

The transport used on premises is still up to the task and there is no reason to replace it. The Router allows both parts of the system to use transports optimized for their hosting environment while being able to seamlessly communicate with each other.

### Case study: Atomic update-and-publish

The business logic of a complex system if often split between a frontend web application and a number of backend services. That frontend application then needs to store or update some data (e.g. an order) in a database atomically with publishing a message.

Atomic update-and-publish is possible with NServiceBus via distributed transactions, the [Outbox](/nservicebus/outbox) or connection/transaction sharing available only in SQL Server transport. The first option might not be feasible due to infrastructure constraints. The second works only in the context of a message handler so does not apply to a web application.

In cases where SQL Server transport is not a good option for the whole system, the Router can be used to connect the frontend and the backend parts of the system. The frontend can use the SQL Server transport and take advantage of connection/transaction sharing between update and publish while the backend is free to use whatever the transport best suits the system's needs. 

To learn more about this case, see the [atomic update-and-publish sample](/samples/router/update-and-publish).
