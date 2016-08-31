---
title: Long running operations with Azure Service Bus Transport
reviewed: 2016-09-07
component: ASB
related:
- nservicebus/azure-service-bus
---


## Prerequisites

include: asb-connectionstring


include: asb-transport


## Code walk-through

RE-WORK!
This sample shows a simple two endpoint scenario.

 * `Endpoint1` sends a `Message1` message to `Endpoint1`.
 * `Endpoint2` replies to `Endpoint1` with a `Message2`.


### Performing processing outside of a message handler

Explanation
V*: external process or a background thread theory + sample