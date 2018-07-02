---
title: Azure Service Bus .NET Standard Transport
reviewed: 2018-06-21
component: ASBS
related:
- transports/azure-service-bus-netstandard
---


## Prerequisites

include: asb-connectionstring-xplat


## Code walk-through

This sample shows a simple two endpoint scenario.

 * `Endpoint1` sends a `Message1` message to `Endpoint2`.
 * `Endpoint2` replies to `Endpoint1` with a `Message2`.


### Transport configuration

snippet: config


## Viewing message in-flight

The following queues for the two endpoints can be seen in the Azure Portal or a 3rd party tool:

 * `samples.asbs.sendreply.endpoint1`
 * `samples.asbs.sendreply.endpoint2`
 * `error`
