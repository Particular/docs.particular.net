---
title: Azure Service Bus Pub/Sub Native Integration
summary: How to consume event messages published by non-NServiceBus endpoints.
component: ASBS
isLearningPath: true
reviewed: 2025-02-14
related:
 - transports/azure-service-bus
---

This sample shows how to subscribe to events published by an NServiceBus endpoint using the Azure Service Bus API.

## Prerequisites

include: asb-connectionstring-xplat

## Code walk-through

The sample contains three executable projects:

* `Publisher`: an NServiceBus endpoint that publishes `EventOne` and `EventTwo` events.
* `NativeSubscriberA`: an executable subscribing to the `EventOne` event published by the `Publisher`.
* `NativeSubscriberB`: an executable subscribing to both events published by the `Publisher`.

Partial: per-event
