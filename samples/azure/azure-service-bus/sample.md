---
title: Azure Service Bus Transport (Legacy)
reviewed: 2020-08-18
component: ASB
related:
- transports/azure-service-bus
---

include: legacy-asb-warning


## Prerequisites

include: asb-connectionstring


include: asb-transport


## Code walk-through

This sample shows a simple two endpoint scenario.

 * `Endpoint1` sends a `Message1` message to `Endpoint2`.
 * `Endpoint2` replies to `Endpoint1` with a `Message2`.


### Azure Service Bus configuration

The `Server` endpoint is configured to use the Azure Table persistence in two locations.

snippet: Config

Some things to note:

 * The use of the `SamplesAzureServiceBusConnection` environment variable mentioned above.
partial: things-to-note


## Viewing message in-flight

The following queues for the two endpoints can be seen in the Azure Portal or a 3rd party tool:

 * `samples.azure.servicebus.endpoint1`
 * `samples.azure.servicebus.endpoint2`
 * `error`

partial: note-on-extra-queues
