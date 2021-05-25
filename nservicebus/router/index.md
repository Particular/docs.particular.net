---
title: NServiceBus Router
summary: How to connect parts of the system that use different transports 
component: Router
related:
- samples/azure-service-bus-netstandard/azure-service-bus-msmq-bridge
- samples/msmq/msmqtosqlrelay
redirects:
- nservicebus/bridge
reviewed: 2021-05-18
---

`NServiceBus.Router` is a universal component that connects parts of an NServiceBus-based solution that otherwise could not talk to each other (e.g. because they use different transports or transport settings).

The router is transparent to the publishing and replying endpoint. That is:

 * The endpoint that *replies* to a message does not have to know if the initiating message came through a router. The reply will be routed automatically to the correct router and then forwarded to the initiating endpoint.
 * The endpoint that *publishes* events does not have to know if the subscribers are behind the router.


## Connecting to the router

Regular endpoints connect to the router using *connectors* that allow them to configure the routing

snippet: connector

The snippet above tells the endpoint that a designated router listens on queue `MyRouter` and that messages of type `MyMessage` should be sent to the endpoint `Receiver` via the router. It also tells the subscription infrastructure that the event `MyEvent` is published by the endpoint `Publisher` that is hosted behind the router.

NOTE: The NServiceBus.Router.Connector package targets NServiceBus 7. In order to connect to the router from NServiceBus 6 use NServiceBus.Bridge.Connector package.


## Router configuration

NServiceBus.Router is packaged as a host-agnostic library. It can be hosted e.g. inside a console application or a Windows service. It can also be co-hosted with regular NServiceBus endpoints in the same process.

The following snippet shows a simple MSMQ-to-RabbitMQ router configuration

snippet: two-way-router

The router has a simple life cycle:

snippet: lifecycle

The router can be configured to create all required queues on startup:

snippet: queue-creation


## Error handling

The router has a built-in retry strategy for error handling. It retries forwarding each message a number of times (*immediate retries*) and then moves it to the back of the input queue incrementing a *delayed retry* counter. If that counter reaches the maximum configured value, the message is moved to the poison message queue. The following snippet shows how it can be configured:

snippet: recoverability

In addition to immediate and delayed retries, the router has built-in outage detection through a *circuit breaker*. After a number of consecutive failures, the circuit breaker is triggered which causes the interface to enter the *throttled mode*. In this mode, the interface processes a single message at a time and pauses after each processing attempt. The interface goes back to the normal mode after the first successful processing attempt. When in the throttled mode, the router does not increment the delayed retries counter to prevent messages being sent to the poison message queue due to infrastructure outages.

## Topologies

A router consists of multiple [NServiceBus.Raw](/nservicebus/rawmessaging/) endpoints, called *interfaces*, and a *routing protocol* that controls how messages should be forwarded between them. This design is very flexible and allows for various topologies to be implemented. Here are some examples:

 * [Two-way bridge](bridge.md)
 * [Multi-way bridge](multi-way.md)
 * [Backplane](backplane.md)
