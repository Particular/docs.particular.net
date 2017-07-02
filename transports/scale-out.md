---
title: Scaling out endpoints
reviewed: 2016-11-07
component: Core
tags:
- scalability
- Routing
redirects:
- nservicebus/scale-out
---

## When to scale out

### Throughput

Scaling out is only useful for where the work being done by a single machine takes time and therefore more computing resources helps. To help with this, monitor the [CriticalTime performance counter](/nservicebus/operations/performance-counters.md) on the endpoint.

### High-availability

Some scaling out techniques improve the availability characteristic as well as throughput. When scaling out for higher availability keep in mind the components that are single points of failure e.g.
 * Distributor when using MSMQ
 * Message broker

and use appropriate risk mitigation technique (e.g. use failover cluster to host Distributor or cluster mode if a given broker supports one).  

## Bus transports

In bus transports like MSMQ there is no central place from which multiple instances of an endpoint can receive messages concurrently. Each instance has its own queue so scaling out requires distributing of messages between the queues. The process of distributing the messages can be performed either on sender side or by a specialized proxy. Refer to the [MSMQ documentation](/transports/msmq/scaling-out.md) for more details regarding this particular transport.


## Broker transports

The main difference when using broker transports (SQL Server, RabbitMQ, ASB, ASQ and SQS) is that queues are not attached to machines but rather are maintained by a central server (or cluster of servers). Such design enables usage of the competing consumers pattern for scaling out processing power. All instances connect to the same queue.

partial: concurrency