---
title:  NServiceBus Router backplane topology
summary: How to connect parts of the system using a shared backplane
component: Router
related:
- samples/router/backplane
reviewed: 2023-12-06
---

include: bridge-to-router-note

Two or more routers can be connected to form a _backplane_ topology.

![Backplane](backplane.svg)

This setup often makes sense for geo-distributed systems. The following snippet configures the Router hosted in the European part of the globally distributed system to route messages coming from outside via the Azure Storage Queues interface directly to the local endpoints and to route messages sent by local endpoints to either east or west United States through *designated gateway* routers.

NOTE: The *designated gateway* concept is unrelated to the `NServiceBus.Gateway` package. When the *designated gateway* is specified in the route, the message is forwarded to it instead of the actual destination.

snippet: backplane

NOTE: For example, the routing rules here use the `Site` property that can be set through the `SendOptions` object when sending messages. The backplane topology does not require site-based routing and can be configured, e.g., using endpoint-based convention like in the multi-way routing example.

To learn more about using the Router in the backplane topology, see the [backplane sample](/samples/router/backplane), which shows how parts of the system (a.k.a. services) that use their own SQL Server databases can be connected with a RabbitMQ-based backplane.

### Case study: Geo-distributed system

The selected transport must work well across the public Internet when system parts are geographically distributed. Unfortunately, these transports tend to provide the lowest consistency guarantees. This makes ensuring the correctness of message handling code in all failure modes more challenging.

Instead of using the lowest common denominator transport for the whole system, the Router can connect the transport on the inside (the one offering high consistency guarantees) with the transport on the outside (the one suitable for usage in the public Internet).

### Case study: Sharding SQL Server transport

SQL Server transport offers unique features like sharing transactions between message-related operations and data access. This feature makes writing correct message-handling logic easy, as there are fewer failure modes. Either both publishing and updating data succeed, or none of them succeeds. Unfortunately, it is not designed for high throughput systems.

Instead of switching to a different transport offering higher throughput but providing lower consistency guarantees, the Router can connect *islands* of SQL Server transport. This sharding approach works when most messages are exchanged within islands. 
