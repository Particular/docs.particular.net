---
title: Mapping endpoint instances using a shared file
summary: Mapping logical endpoints to physical instances using a shared file
reviewed: 2016-08-01
component: Core
tags:
 - Routing
 - MSMQ
---

The sample demonstrates how to use a file to describe the mapping between logical endpoints and their physical instances (deployments of given logical endpoint to a concrete VM).


## Prerequisites

 1. Make sure SQL Server Express is installed and accessible as `.\SQLEXPRESS`.
 1. Create database called `AutomaticRouting`.


## Running the project

 1. Start the solution
 1. The text `Press <enter> to send a message` should be displayed in the Client's console window.
 1. Wait until all the endpoints exchange their routing information. Notice each endpoint logs the routing info as it discovers other endpoints.
 1. Hit `<enter>` several times to send some messages.


### Verifying that the sample works correctly

 1. The Sales.1 and Sales.2 consoles display information about accepted orders in round-robin fashion.
 1. The Shipping endpoint displays information that orders were shipped.
 1. The Billing endpoint displays information that orders were billed.
 1. Notice that queues created by running the sample all have `-at-<machine>` suffix. This is the result of simulating a multi-machine system on a single machine.


## Code walk-through

This sample contains four projects:

 * Shared - A class library containing common routing code including the message definitions.
 * Client - A console application responsible for sending the initial `PlaceOrder` message.
 * Sales - A console application responsible for processing the `PlaceOrder` command and generating `OrderAccepted` event.
 * Shipping - A console application responsible for processing the `OrderAccepted` message.
 * Billing - Another console application responsible for processing the `OrderAccepted` message.


### Instance Mapping File

Shared between all endpoints.

snippet: instanceMapping


### Client

The Client does not store any data. It mimics the front-end system where orders are submitted by the users and passed via the bus to the back-end. The client, as well as all other endpoints, uses the file based instance mapping

snippet:FileInstanceMapping


### Sales

The Sales project mimics the back-end system where orders are accepted.

In real-world scenarios NServiceBus is scaled out by deploying multiple physical instances of a single logical endpoint to multiple machines (e.g. `Sales` in this sample). For simplicity in this sample the scale-out is simulated by having two separate endpoints `Sales` and `Sales2`.


### Shipping and Billing

Shipping and Billing mimic back-end systems subscribing to events published by Sales endpoints.


### Shared project

The shared project contains definitions for messages and the custom routing logic.


### Real-world scenario

For the sake of simplicity, in this sample all the endpoints run on a single machine. In real world usually one would run each instance on a separate physical or virtual machine. In such case the instance mapping file would contain only the `machine` attributes mapping instances to their machines' host names.
