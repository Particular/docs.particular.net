---
title: Scaling out NServiceBus endpoints
summary: How to scale out NServiceBus endpoints
tags:
- Scale Out
- Routing
---

## Versions 5 and below

In Version 5 and below NServiceBus scale out capabilities are dependent on the transport being used.


### MSMQ

Because of limitations of MSMQ related to remote receive, in order to scale out an MSMQ Version 5 (and below) endpoint have to use the [distributor](/nservicebus/scalability-and-ha/distributor/). The role of the distributor is to forward incoming messages to a number of workers in order to balance the load. The workers are "invisible" to the outside world because all the outgoing messages contain the distributor's (not the worker's) address in the `reply-to` header. 

The main issue with distributor is the throughput limitation due to the fact that, for each message forwarded to worker, there were additional two messages exchanged between the worker and the distributor.


### SQL Server and RabbitMQ

Up to version 5 (inclusive) both SQL Server and RabbitMQ transports scale out by adding more receivers to the same queue, taking advantage of the competing consumers capability built into these transports. All the instances feeding off the queue have same *endpoint name* and *address* so they appear to the outside world as a single instance.

The benefit of this approach is zero configuration. New instances can be added by `xcopy`-ing the deployment folder. The potential downside is that total throughput of the endpoint is capped to the maximum throughput of a single queue in the underlying infrastructure.


### Azure Storage Queues and Azure Service Bus

Up to version 5 (inclusive) both Azure transports behave similarly to other broker transports (SQL Server and RabbitMQ): they scale out by adding competing consumers receiving from a single queue. An additional feature of Azure transports is ability to [use individualized queues](individualizing-queues-when-scaling-out.md) (based on the *role instance ID*)

This feature is, however, *not intended* for scaling out as there is no built-in distribution mechanism between that would route messages to the individualized queues.


## Versions 6 and above

Version 6 of NServiceBus comes with a unified scalability model which is based on the concept of endpoint instance ID. Each deployment of NServiceBus can (but does not have to) be assigned an *instance ID*.

When instance ID is assigned, NServiceBus spins up an additional receiver for the queue which name is based on that ID, e.g. if the endpoint name is `Sales` and instance ID is `Green` that instance will try to receive from queues `Sales` and `Sales-Green` (actual queue names depend on the underlying transport).

It is up to the sender to choose if it is going to treat the endpoint as a whole (and send its messages to `Sales` queue) or address individual instances (e.g. `Sales-Red`, `Sales-Green`, `Sales-Blue`). 

In the first case the the scaling out happens by means of competing consumers. In the second case it is realized by sender using a round-robin algorithm to balance the load on receiver instances. Both scale out approaches are supported by all transports, but some transports are better suited for one or the other.


### MSMQ

Because MSMQ does not allow performant remote receives in most cases scaling out requires sender-side round-robin distribution. When using MSMQ different instances are usually deployed to different (virtual) machines. The following routing file (see [file-based routing](/nservicebus/messaging/file-based-routing.md)) shows scaling out of the `Sales` endpoint.

snippet:Routing-FileBased-MSMQ

The corresponding logical routing is

snippet:Routing-StaticRoutes-Endpoint-Msmq

NOTE: System administrators are able to spin-up new instances of the endpoint should the load increase and the only requirement is adding an entry to the routing file. No changes in the source code are required.


### Broker transports

The main difference when using broker transports is that queues are not attached to machines but are rather maintained by a central server (or cluster of servers). When running on a broker transport such as RabbitMQ or SQL Server, it is enough to specify the logical routing:

snippet:Routing-StaticRoutes-Endpoint-Broker

Scaling out can be done, as in Version 5, by `xcopy`-ing the binaries. In this case all instances have empty IDs and there is only one queue in use.

Should the need to go past the throughput of a single infrastructure queue or have to address each instance separately, instance IDs can be specified for each deployment of the endpoint. In this case, in addition to the shared `Sales` queue, there will be two instance-specific queues used by the `Sales` endpoint.

Some upstream endpoints might decide to still treat `Sales` as a single *thing* and depend on the logical routing only. These endpoints will continue to send their messages to the `Sales` queue. Others might include routing file:

snippet:Routing-FileBased-Broker

In that case the sender will use round-robin distribution like in the MSMQ case.

NOTE: Even when physical routing is specified (e.g. in form of the routing file) the transport is able to ignore it if it is not possible to implement a particular scenario. An example is RabbitMQ which delivers the events always to the shared queue. The reason for that is lack of built-in `round-robin` exchange type that could be used to bind event exchanges to individual queues.