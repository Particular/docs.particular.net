---
title: Using centralized routing configuration
summary: Using file to control message routing in entire NServiceBus system
reviewed: 2016-12-16
component: FileBasedRouting
tags:
 - Routing
 - MSMQ
related:
- nservicebus/messaging/file-based-routing
---


NOTE: Centralized file-based routing is an experimental project hosted on [ParticularLabs](https://github.com/ParticularLabs). Particular Software does not provide any support guarantee for such projects.

The sample demonstrates how to use a file to describe the logical routing topology.


## Prerequisites

Make sure MSMQ is set up as described in the [MSMQ Transport - NServiceBus Configuration](/nservicebus/msmq/#nservicebus-configuration) section.


## Running the project

 1. Start the solution.
 1. The text `Press <enter> to send a message` should be displayed in the Client's console window.
 1. Hit `<enter>` several times to send some messages.


### Verifying that the sample works correctly

 1. The Sales console display information about accepted orders in round-robin fashion.
 1. The Shipping endpoint displays information that orders were shipped.
 1. The Billing endpoint displays information that orders were billed.


## Code walk-through

This sample contains four projects. All these project make use of a shared routing file.

snippet: endpoints

For the purpose of the sample, the file is stored in the local file system. The routing mechanism based on the contents of the file is enabled by following code:

snippet: FileBasedRouting
