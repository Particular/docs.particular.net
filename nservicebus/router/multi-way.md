---
title: NServiceBus Router multi-way bridge topology
summary: How to connect more than two transports with a single router
component: Router
related:
- samples/router/sql-switch
reviewed: 2020-09-17
---

![Multi-way](multi-way.svg)

The router is not limited to only two interfaces but, in case there are more than two interfaces, the routing protocol rules will be more complex and specific. The following snippet configures the built-in *static routing protocol* to forward messages to interfaces based on the prefix of the destination endpoint's name.

snippet: three-way-router

NOTE: All three interfaces use the same transport type (SQL Server Transport) but may use different settings, e.g. different database instances. This way, each part of the system (Sales, Shipping and Billing) can be autonomous and own its database server yet they can still exchange messages in the same way as if they were connected to a single shared instance.

For more information about using the Router in multi-way bridge topology with SQL Server transport, see [the sample](/samples/router/sql-switch).

### Case study: SQL Server multi-instance mode migration

In the past, prior to Version 4, the SQL Server transport supported the multi-instance mode. In this mode a single NServiceBus endpoint was able to send messages to queues hosted in SQL Server instances different than the one that hosts that endpoint's own input queue. Due to complex configuration requirements this mode of operation has been removed.

The Router in the multi-way topology can be used to forward messages between multiple SQL Server instances after upgrading to the version of SQL Server transport that no longer supports the multi-instance mode.
