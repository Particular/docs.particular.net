---
title: Mapping Endpoint Instances With a Shared File
summary: Mapping logical endpoints to physical instances with a shared file
reviewed: 2020-05-27
component: Core
---

The sample demonstrates how to use a file to describe the mapping between logical endpoints and their physical instances (i.e. deployments of a given logical endpoint to a concrete virtual machine).


## Prerequisites

Ensure MSMQ is installed and configured as described in the [MSMQ transport - MSMQ configuration](/transports/msmq/#msmq-configuration) section.


## Running the project

 1. Start the solution
 1. The text `Press <enter> to send a message` should be displayed in the Client's console window.
 1. Press <kbd>enter</kbd> several times to send some messages.


### Verifying that the sample works 

 1. The Sales.1 and Sales.2 consoles display information about accepted orders in round-robin fashion.
 1. The Shipping endpoint displays information that orders were shipped.
 1. The Billing endpoint displays information that orders were billed.


## Code walk-through

This sample contains four projects.


### Instance mapping file

Shared between all endpoints.

snippet: instanceMapping


### Client

The client application submits the orders for processing by the back-end systems by sending a `PlaceOrder` command. The client, as well as all other endpoints, uses the file-based instance mapping:

snippet: FileInstanceMapping


### Sales

The Sales application accepts client orders and publishes the `OrderAccepted` event.

NOTE: In real-world scenarios, NServiceBus endpoints are scaled out by deploying multiple physical instances of a single logical endpoint to multiple machines. For simplicity, the scale out in this sample is simulated by having two separate projects, Sales and Sales2.


### Shipping and Billing

The Shipping and Billing applications subscribe to `OrderAccepted` events in order to execute their business logic.


### Shared project

The shared project contains definitions for messages.


## Real-world scenario

For the sake of simplicity, all the endpoints in this sample run on a single machine. In production, it is recommended to run each instance on a separate virtual machine. In this case, the instance mapping file would contain `machine` attributes mapping instances to their machines' host names instead of `queue` attributes used to run more than one instance on a single box.