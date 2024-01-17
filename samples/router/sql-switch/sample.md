---
title: Connecting multiple SQL Server instances with NServiceBus.Router
summary: A sample demonstrating the use of NServiceBus.Router to connect endpoints that use different SQL Server instances
reviewed: 2024-01-16
component: Router
related:
- transports/sql
- nservicebus/router
- samples/router/backplane
redirects:
- samples/bridge/sql-switch
---

include: bridge-to-router-note

The sample demonstrates how to use NServiceBus.Router to connect endpoints running the SQL Server transport and that use different instances of SQL Server. This is an alternative to the SQL Server transport's multi-instance mode which was removed in version 4.

include: switch-vs-backplane

NOTE: This sample uses a single SQL Server instance with multiple catalogs to approximate the multi-instance scenario. If the production scenario does indeed use a single SQL Server instance and multiple catalogs, use the SQL Server transport [multi-catalog](/transports/sql/addressing.md#resolution-catalog) feature instead of the Router.

## Prerequisites

include: sql-prereq

This sample automatically creates four databases: `sqlswitch`, `sqlswitch_blue`, `sqlswitch_red` and `sqlswitch_green`

## Running the project

 1. Start the solution.
 1. The text `Press <enter> to send a message` should be displayed in the Client's console window.
 1. Press <kbd>enter</kbd> several times to send some messages.

### Verifying that the sample works correctly

 1. The Sales console display information about accepted orders in a round-robin fashion.
 1. The Shipping endpoint displays information that orders were shipped.
 1. The Billing endpoint displays information that orders were billed.

## Code walk-through

The sample contains four endpoints: Client, Sales, Shipping and Billing. The Client endpoint sends a `PlaceOrder` command to Sales. When a `PlaceOrder` message is processed, the Sales endpoint publishes an `OrderAccepted` event which is subscribed to by the Shipping and Billing endpoints.

### The Client endpoint

The Client endpoint is configured to use its own database (named Blue) to harden the security of the solution. This database does not contain sensitive data.

In order to route messages to Sales, the Client must configure the router connection

snippet: ClientRouterConfig

### The Sales and Shipping endpoints

The Sales and Shipping endpoints are configured to use the Red database for the transport. Since the Sales endpoint only publishes events, it does not need any routing configuration.

Shipping subscribes to events published by Sales and it uses the same transport database so the router is not involved.

### The Billing endpoint

The Billing endpoint requires even more enhanced security. It uses its own database, named Green. In order to subscribe to Sales event it must register the publisher in the router configuration

snippet: BillingRouterConfig

### The Switch project

The Switch project hosts the NServiceBus.Router instance that has three interfaces, one for each SQL Server transport database.

snippet: SwitchConfig

The forwarding of messages between the databases is governed by an endpoint naming convention: the first part of the endpoint name is used as the destination interface name.

snippet: SwitchForwarding
