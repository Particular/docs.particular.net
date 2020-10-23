---
title: Connecting multiple SQL Server instances with NServiceBus.Router
summary: Use NServiceBus.Router to connect endpoints that use different SQL Server instances
reviewed: 2020-10-24
component: Router
related:
- transports/sql
- nservicebus/router
- samples/router/backplane
redirects:
- samples/bridge/sql-switch 
---

The sample demonstrates how to use NServiceBus.Router to connect endpoints running SQL Server transport that use different instances of SQL Server. This is an alternative to the multi-instance mode of SQL Server transport which has been removed in Version 4.

include: switch-vs-backplane

NOTE: This sample uses a single SQL Server instance with multiple catalogs to approximate the multi-instance scenario. If the production scenario does indeed uses a single SQL Server instance and multiple catalogs, use SQL Server transport [multi-catalog](/transports/sql/addressing.md#resolution-catalog) instead of the Router.


## Prerequisites

include: sql-prereq

This sample automatically creates four databases: `sqlswitch`, `sqlswitch_blue`, `sqlswitch_red` and `sqlswitch_green`


## Running the project

 1. Start the solution.
 1. The text `Press <enter> to send a message` should be displayed in the Client's console window.
 1. Hit enter several times to send some messages.


### Verifying that the sample works correctly

 1. The Sales console display information about accepted orders in round-robin fashion.
 1. The Shipping endpoint displays information that orders were shipped.
 1. The Billing endpoint displays information that orders were billed.


## Code walk-through

This sample contains four endpoints, Client, Sales, Shipping and Billing. The Client endpoint sends a `PlaceOrder` command to Sales. When `PlaceOrder` is processed, Sales publishes the `OrderAccepted` event which is subscribed by Shipping and Billing.


### Client

The Client endpoint is configured to use its own, Blue, database to harden the security of the solution. This database does not contain sensitive data.

In order to route messages to Sales, Client needs to configure the router connection

snippet: ClientRouterConfig


### Sales and Shipping

The Sales and Shipping endpoints are configured to use the Red database for the transport. As Sales only publishes events, it does not need any routing configuration.

Shipping subscribes for events published by Sales and it uses the same transport database so the router is not involved.

### Billing

The Billing endpoint requires even more enhanced security. It uses its own database, Green. In order to subscribe to Sales event it need to register the publisher in the router configuration

snippet: BillingRouterConfig


### Switch

The Switch project hosts the NServiceBus.Router instance that has three interfaces, one for each SQL Server transport database.

snippet: SwitchConfig

The forwarding of messages between the databases is governed by an endpoint naming convention: the first part of the endpoint name is used as the destination interface name.

snippet: SwitchForwarding

