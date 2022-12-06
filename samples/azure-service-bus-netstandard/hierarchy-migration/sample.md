---
title: Azure Service Bus Hierarchy Migration Sample
summary: Demonstrates how to life migrate an endpoint from one topic to another
reviewed: 2022-12-06
component: ASBS
related:
- transports/azure-service-bus
---

NOTE: We are providing this sample in order to find out whether there is interest in this approach. Depending on the feedback of this approach the functionality of migrating an endpoint could be built into the [operational scripting tool](/transports/azure-service-bus/operational-scripting) of the Azure Service Bus transport. Comment on [the issue](https://github.com/Particular/NServiceBus.Transport.AzureServiceBus/issues/718) or reach out using the [support options available](https://particular.net/support).

## Prerequisites

include: asb-connectionstring-xplat

## Code walk-through

This sample demonstrates how an endpoint can be "life" migrated from one topic to another:

 * `Endpoint1` publishes `Event1` messages and subscribes to `Event2` messages.
 * `Endpoint2` subscribes to `Event1` messages and publishes `Event2` messages.
 * `Migration` does a step by step life migration of `Endpoint2` while the endpoints are running and exchanging messages with each other.

## Viewing messages in-flight

The following queues for the two endpoints can be seen in the Azure Portal or a third-party tool:

 * `samples.asbs.hierarchymigration.endpoint1`
 * `samples.asbs.hierarchymigration.endpoint2`
 * `samples.asbs.hierarchymigration.endpoint2.migration`