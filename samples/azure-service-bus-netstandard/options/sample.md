---
title: Azure Service Bus Topology Options
summary: How to use topology options to layout the topic topology in app settings.
component: ASBS
reviewed: 2025-02-12
related:
 - transports/azure-service-bus
---

This sample shows how to leverage topology options to layout the topic topology used within the app settings. The sample uses the generic host for convenient loading of appsettings.json via the built-in configuration provider model. Appsettings is used for demonstration purposes and technically the options could be loaded from other sources.

## Prerequisites

include: asb-connectionstring-xplat

## Code walk-through

The sample contains three executable projects:

 * `Publisher`: an NServiceBus endpoint that publishes `EventOne` to topic `event-one` and `EventTwo` to `event-two`.
 * `Subscriber`: an NServiceBus endpoint subscribing to the `EventOne` and `EventTwo` event published by the `Publisher`.

## Running the sample

1. First, run the `Subscriber` project by itself. This will create all the necessary publish/subscribe infrastructure in Azure Service Bus.
2. Run the projects normally so that all endpoints start.
3. The `Publisher` endpoint continuously published two events with a short pause in between.
    * The endpoint in the `Subscriber` window will receive both `EventOne` and `EventTwo`.

TBD