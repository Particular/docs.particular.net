---
title: Azure Service Bus Send/Reply Sample
summary: Demonstrates the send/reply pattern with Azure Service Bus
reviewed: 2020-05-27
component: ASBS
related:
- transports/azure-service-bus
---


## Prerequisites

include: asb-connectionstring-xplat


## Code walk-through

This sample shows a basic two-endpoint scenario exchanging messages with each other so that:

 * `Endpoint1` sends a `Message1` message to `Endpoint2`.
 * `Endpoint2` replies to `Endpoint1` with a `Message2` instance.


### Transport configuration

snippet: config


## Viewing messages in-flight

The following queues for the two endpoints can be seen in the Azure Portal or a third-party tool:

 * `samples.asbs.sendreply.endpoint1`
 * `samples.asbs.sendreply.endpoint2`
 * `error`
