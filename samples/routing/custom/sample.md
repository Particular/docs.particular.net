---
title: Custom routing
summary: Customizing NSertviceBus message routing
component: Core
tags:
- Routing
redirects:
---


## Prerequisites

 1. Make sure SQL Server Express is installed and accessible as `.\SQLEXPRESS`. 
 1. Create database called `AutomaticRouting`. 

## Running the project

 1. Start all the projects by hitting F5.
 1. The text `Press <enter> to send a message` should be displayed in the Client's console window.
 1. Hit `<enter>` several times to send some messages.

## Verifying that the sample works correctly

 1. The Sales endpoint displays information that an order was accepted.
 2. The Shipping endpoint displays information that an order was shipped.
 3. The Billing endpoint displays information that an order was billed.

## Code walk-through

This sample contains four projects:

 * Shared - A class library containing common routing code including the message definitions.
 * Client - A console application responsible for sending the initial `PlaceOrder` message.
 * Sales - A console application responsible for processing the `PlaceOrder` command and generating `OrderAccepted` event.
 * Shipping - A console application responsible for processing the `OrderAccepted` message.
 * Billing - Another console application responsible for processing the `OrderAccepted` message.

### Client project

The Sender does not store any data. It mimics the front-end system where orders are submitted by the users and passed via the bus to the back-end. Apart from the standard NServiceBus configuration it enables the custom automatic routing:

snippet:EnableAutomaticRouting

### Shared project

The shared project contains definitions for messages and the custom routing logic. 

### Custom automatic routing

The automatic routing is based on the idea of endpoints exchanging information about types of messages they handle. They do it via a table in SQL Server database (each endpoint instance own one row keyed by this endpoint's transport address). Following code configures the custom routing:

snippet:Feature

In short, it disables the default behavior of sending subscribe/unsubscribe messages. Instead of that, it wires the routing behavior for both sends and publishes to a single place where information about message types and associated endpoints is cached. This cache consists of three structures. The first maps message types to sets of endpoint which are known to have handlers for these message types. It is used in the logical routing phase.

snippet:FindEndpoint

The second structure maps endpoint names to sets of known endpoint instances. It is used in the physical routing phase.

snippet:FindInstance

The third structure holds information about the known endpoint instances. This information is used to optimize the physical routing phase by detecting inactive instances.