---
title: Connect multiple SQL Server instances with a backplane
summary: Use a backplane to connect endpoints that use different SQL Server instances
component: Router
reviewed: 2024-01-12
related:
- transports/sql
- transports/rabbitmq
- nservicebus/router
- samples/router/sql-switch
redirects:
- samples/bridge/backplane
---

include: bridge-to-router-note

The sample demonstrates how to use NServiceBus.Router to connect endpoints using the SQL Server transport, and that use different instances of SQL Server. This is an alternative to the multi-instance mode of SQL Server transport which has been removed in version 4.

The RabbitMQ broker is used as a backplane in this sample.

include: switch-vs-backplane

NOTE: This sample uses a single SQL Server instance with multiple catalogs to approximate the multi-instance scenario. If the production scenario does indeed uses a single SQL Server instance and multiple catalogs, use the SQL Server transport [multi-catalog](/transports/sql/addressing.md#resolution-catalog) feature instead of the Router.

## Prerequisites

include: sql-prereq

This sample automatically creates three databases: `backplane_blue`, `backplane_red` and `backplane_green`

## Running the project

 1. Start the solution.
 1. The text `Press <enter> to send a message` should be displayed in the Client's console window.
 1. Press <kbd>enter</kbd> several times to send some messages.

### Verifying that the sample works correctly

 1. The Sales console display information about accepted orders in round-robin fashion.
 1. The Shipping endpoint displays information that orders were shipped.
 1. The Billing endpoint displays information that orders were billed.
 1. The Client endpoint displays information that orders were placed.

## Code walk-through

This sample contains four endpoints: Client, Sales, Shipping and Billing. The Client endpoint sends a `PlaceOrder` command to Sales. When a `PlaceOrder` command is processed, Sales publishes the `OrderAccepted` event which is subscribed to by the Shipping and Billing endpoints.

In addition to the four business endpoints, the sample contains three Router endpoints that connect the three databases, Blue, Red and Green, to the RabbitMQ backplane.

### Client

The Client endpoint is configured to use its own database (named Blue) to harden the security of the solution. This database does not contain sensitive data.

In order to route messages to Sales, the Client endpoint must configure the router connection

snippet: ClientRouterConnector

### Sales and Shipping

The Sales and Shipping endpoints are configured to use the Red database for the transport. Since Sales only publishes events and sends replies, it does not need any router configuration.

Shipping subscribes to events published by the Sales endpoint and it uses the same transport database so the router is not involved.

### Billing

The Billing endpoint requires even more enhanced security. It uses its own database, Green. In order to subscribe to Sales events, it must register the publisher in the router configuration

snippet: BillingRouterConnector

### Routers

Each database is connected to the backplane via a separate router. All three routers share the same configuration

snippet: RouterConfig

Forwarding messages between the databases is governed by an endpoint naming convention: the first part of the endpoint name is used as the destination interface name.

snippet: RoutingTopology

Each router has routes to all other SQL databases going through the backplane interface. Each of these routes has a *designated gateway* set so that messages are not routed directly to the destination, but to the next router.

In addition to that, each router has a route that instructs it to forward all messages coming via the backplane interface to the local SQL interface.

#### Consistency

The backplane transport (RabbitMQ) offers lower consistency guarantees than the endpoints' transport (SQL Server). The messages can get duplicated while traveling between the databases. This is simulated by the `RandomDuplicator` router extension which creates duplicates with 50% chance. Each time a duplicate is created, the router logs a warning.

In order to preserve *exactly-once* message processing guarantees that SQL Server transport offers, messages forwarded from the backplane to the database need to be de-duplicated. The SQL Server de-duplication router extension addresses this problem.

snippet: Deduplication

NOTE: De-duplication requires adding the `NServiceBus.Router.SqlServer` package and is considered an experimental feature.
