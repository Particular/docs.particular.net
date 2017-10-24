---
title: Mapping endpoint instances with a shared file
summary: Mapping logical endpoints to physical instances with a shared file
reviewed: 2016-10-26
component: Core
tags:
 - Routing
 - MSMQ
---

The sample demonstrates how to use a file to describe the mapping between logical endpoints and their physical instances (deployments of given logical endpoint to a concrete VM).


## Prerequisites

Make sure MSMQ is installed and configured as described in the [MSMQ Transport - MSMQ Configuration](/transports/msmq/#msmq-configuration) section.


## Running the project

 1. Start the solution
 1. The text `Press <enter> to send a message` should be displayed in the Client's console window.
 1. Hit enter several times to send some messages.


### Verifying that the sample works correctly

 1. The Sales.1 and Sales.2 consoles display information about accepted orders in round-robin fashion.
 1. The Shipping endpoint displays information that orders were shipped.
 1. The Billing endpoint displays information that orders were billed.


## Code walk-through

This sample contains four projects.


### Instance Mapping File

Shared between all endpoints.

snippet: instanceMapping


### Client

The Client application submits the orders for processing by the back-end systems by sending a `PlaceOrder` command. The client, as well as all other endpoints, uses the file based instance mapping:

snippet: FileInstanceMapping


### Sales

The Sales application accepts clients' orders and publishes the `OrderAccepted` event.

NOTE: In real-world scenarios NServiceBus endpoints are scaled out by deploying multiple physical instances of a single logical endpoint to multiple machines. For simplicity, in this sample the scale out is simulated by having two separate projects, Sales and Sales2.


### Shipping and Billing

Shipping and Billing applications subscribe to `OrderAccepted` event in order to execute their business logic.


### Shared project

The shared project contains definitions for messages.


## Real-world scenario

For the sake of simplicity, in this sample all the endpoints run on a single machine. In real world is is usually best to run each instance on a separate virtual machine. In such case the instance mapping file would contain `machine` attributes mapping instances to their machines' host names instead of `queue` attributes used to run more than one instance on a single box.
