---
title: Connecting multiple SQL Server instances with Switch
summary: Use Switch to connect endpoints that use different SQL Server instances
reviewed: 2018-01-12
component: Bridge
related:
- transports/sql
- nservicebus/bridge
---

The sample demonstrates how to use Switch from NServiceBus.Bridge package to connect endpoints running SQL Server transport that use different instances of SQL Server. This is an alternative to the multi-instance mode of SQL Server transport which has been removed in Version 4.


## Prerequisites

include: sql-prereq

This sample automatically creates three databases: `switch_blue`, `switch_red` and `switch_green`


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

In order to route messages to Sales, Client needs to configure bridge connection

snippet: ClientBridgeRouting


### Sales and Shipping

The Sales and Shipping endpoints are configured to use the Red database for the transport. As Sales only publishes events, it does not need any routing or bridge configuration.

Shipping subscribes for events published by Sales and it uses the same transport database so regular logical routing is enough

snippet: ShippingRouting


### Billing

The Billing endpoint requires even more enhanced security. It uses its own database, Green. In order to subscribe to Sales event it need to register the publisher in the bridge configuration

snippet: BillingBridgeRouting


### Switch

Similar to the Bridge, Switch connects transports. Unlike the Bridge, Switch can connect more than two transports. Each transport is added as a Port. In this sample the Switch is configured with three ports, all using SQL Server transport

snippet: SwitchConfig

While the Bridge simply forwards messages from one side to the other, the Switch needs explicit forwarding configuration to find out the destination port for each message.

snippet: SwitchForwarding

The code above maps endpoints to ports.
