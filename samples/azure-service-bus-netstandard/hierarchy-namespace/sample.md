---
title: Azure Service Bus Hierarchy Namespace Sample
summary: Demonstrates the hierarchical namespaces with Azure Service Bus
reviewed: 2026-02-05
component: ASBS
related:
- transports/azure-service-bus/configuration
---

## Prerequisites

include: asb-connectionstring-xplat

## Code walk-through

This sample shows a usage of hierarchy namespace with Azure Service Bus. Both endpoints (`Endpoint1` and `Endpoint2`) are configured using `HierarchyNamespace`:

snippet: namespaceOptions

This means that the endpoints will be effectively:
* `Endpoint1`: `my-hierarchy/Samples.ASBS.HierarchyNamespace.Endpoint1`
* `Endpoint2`: `my-hierarchy/Samples.ASBS.HierarchyNamespace.Endpoint2`

The behavior is as follows:
* `Endpoint1` sends a `Message1` message to `Endpoint2`.
* `Endpoint2` replies to `Endpoint1` with a `Message2` instance.

### Transport configuration

snippet: config

## Viewing messages in-flight

The following queues for the two endpoints can be seen in the Azure Portal or a third-party tool:

* `my-hierarchy/samples.asbs.hierarchynamespace.endpoint1`
* `my-hierarchy/samples.asbs.hierarchynamespace.endpoint2`
* `error`
