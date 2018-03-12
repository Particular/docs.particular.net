---
title: Connecting multiple SQL Server instances with a backplane
summary: Use a backplane to connect endpoints that use different SQL Server instances 
component: Bridge
reviewed: 2018-01-10
related:
- transports/sql
- transports/rabbitmq
- nservicebus/bridge
- samples/bridge/sql-switch
---

The sample demonstrates how to use Switch from NServiceBus.Bridge package to connect endpoints running SQL Server transport that use different instances of SQL Server. This is an alternative to the multi-instance mode of SQL Server transport which has been removed in Version 4.

The RabbitMQ broker is used as a backplane in this sample. 

include: switch-vs-backplane
 

## Prerequisites

include: sql-prereq

This sample automatically creates three databases: `backplane_blue`, `backplane_red` and `backplane_green`


## Running the project

 1. Start the solution.
 1. The text `Press <enter> to send a message` should be displayed in the Client's console window.
 1. Hit enter several times to send some messages.


### Verifying that the sample works correctly

 1. The Sales console display information about accepted orders in round-robin fashion.
 1. The Shipping endpoint displays information that orders were shipped.
 1. The Billing endpoint displays information that orders were billed.
 1. The Client endpoint displays information that orders were placed.


## Code walk-through

This sample contains four endpoints, Client, Sales, Shipping and Billing. The Client endpoint sends a `PlaceOrder` command to Sales. When `PlaceOrder` is processed, Sales publishes the `OrderAccepted` event which is subscribed by Shipping and Billing.

In addition to the four business endpoints, the sample contains three Bridge endpoints that connect the three databases, Blue, Red and Green, to the RabbitMQ backplane.


### Client

The Client endpoint is configured to use its own, Blue, database to harden the security of the solution. This database does not contain sensitive data.

In order to route messages to Sales, Client needs to configure bridge connection

snippet: ClientBridgeRouting


### Sales and Shipping

The Sales and Shipping endpoints are configured to use the Red database for the transport. As Sales only publishes events and sends replies, it does not need any routing or bridge configuration.

Shipping subscribes for events published by Sales and it uses the same transport database so regular logical routing is enough

snippet: ShippingRouting


### Billing

The Billing endpoint requires even more enhanced security. It uses its own database, Green. In order to subscribe to Sales event it need to register the publisher in the bridge configuration

snippet: BillingBridgeRouting


### Bridges

Each database is connected to the backplane via a separate bridge. All three bridges share the same configuration

snippet: BridgeConfig

They differ only with regards to forwarding configuration. The Blue bridge is configured to forward `PlaceOrder` messages to the Red database

snippet: BlueForwarding

And the Green bridge is configured to forward subscribe requests for `OrderAccepted` event to the Red database.

snippet: GreenForwarding

The Red bridge does not need forwarding configuration as it only routes published events and replies.

#### Consistency

The backplane transport (RabbitMQ) offers lower consistency guarantees than the endpoints' transport (SQL Server). The messages can get duplicated while travelling between the databases. This is simulated by the `DuplicateRabbitMQMessages` interceptor

snippet: Duplicate

In order to preserver *exactly-once* message processing guarantees that SQL Server transport offers, messages forwarded from the backplane to the database need to be de-duplicated. Another interceptor takes care of this task

snippet: Deduplicate

It stores the IDs of incoming messages in the destination database, atomically with sending messages. If a message has already been sent, it is ignored.

NOTE: In real worlds the `ReceivedMessages` table used for de-duplication would need some kind of eviction policy based on a retention period that removes old de-duplication records.


