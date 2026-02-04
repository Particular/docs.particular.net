---
title: Azure Service Bus Hierarchy Namespace Escape Sample
summary: Demonstrates how to escape a hierarchy namespace with Azure Service Bus
reviewed: 2026-02-05
component: ASBS
related:
- transports/azure-service-bus/configuration
---

## Prerequisites

include: asb-connectionstring-xplat

## Code walk-through

This sample shows a usage of hierarchy namespace with Azure Service Bus with a hierarchy escape mechanism included. Note the following line in the configuration for `Endpoint1`:

snippet: excludedMessage

This makes sure that sending a message of a type `MessageExcluded` does not take hierarchy (`my-hierarchy`) into consideration.

Pressing `2` tries sending a message (of the `MessageExcluded` type) to `Endpoint2`. This endpoint is included in the hierarchy, but the escape mechanism excludes this type of a message. Effectively, the message is tried to be sent to `Samples.ASBS.HierarchyNamespaceEscape.Endpoint2`. Such an endpoint doesn't exist. This is why this attempts ends with an exception: `Azure.Messaging.ServiceBus.ServiceBusException: Put token failed. status-code: 404, status-description: The messaging entity 'sb://xxx.servicebus.windows.net/Samples.ASBS.HierarchyNamespaceEscape.Endpoint2' could not be found.`.

Pressing `3` sends a message of the same type to `Endpoint3`. This endpoint is not included in the hierarchy. This is why this attempts succeeds.

### Transport configuration

snippet: config

Two endpoints (`Endpoint1` and `Endpoint2` are configured to be included in the hierarchy `my-hierarchy`). `Endpoint3` is not included in the hierarchy.

snippet: namespaceOptions

This means that the endpoints will be effectively:
* `Endpoint1`: `my-hierarchy/Samples.ASBS.HierarchyNamespaceEscape.Endpoint1`
* `Endpoint2`: `my-hierarchy/Samples.ASBS.HierarchyNamespaceEscape.Endpoint2`
* `Endpoint3`: `Samples.ASBS.HierarchyNamespaceEscape.Endpoint3`

## Viewing messages in-flight

The following queues for the two endpoints can be seen in the Azure Portal or a third-party tool:

* `my-hierarchy/samples.asbs.hierarchynamespaceescape.endpoint1`
* `my-hierarchy/samples.asbs.hierarchynamespaceescape.endpoint2`
* `samples.asbs.hierarchynamespaceescape.endpoint3`
* `error`
