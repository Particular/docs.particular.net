---
title: Azure Service Bus Pub/Sub Native Integration
summary: How to consume event messages published by non-NServiceBus endpoints.
component: ASBS
isLearningPath: true
reviewed: 2021-01-07
related:
 - transports/azure-service-bus
---

This sample shows how to subscribe to events published by NServiceBus endpoint using the Azure Service Bus API

## Prerequisites

include: asb-connectionstring-xplat

## Code walk-through

The sample contains three executable projects:

 * `Publisher` - NServiceBus endpoint that publishes `EventOne` and `EventTwo` events.
 * `NativeSubscriberA` - subscribes to `EventOne` event published by the `Publisher`.
 * `NativeSubscriberB` - subscribes to both events published by the `Publisher`.
 
## Setting up namespace entities

Each of the subscribers require a topic subscription to receive the events published by the `Publisher`. The subscriptions are created on the `bundle-1` topic which is [the default](/transports/azure-service-bus/configuration.md#entity-creation) name used by NServiceBus endpoints. 

snippet: SubscriptionCreation

### Subscriptions filters

Subscriptions created by `NativeSubscriberA` and `NativeSubscriberB` differ by the mesage filter. The first subscribes to the `EventOne` events only. It is done by specifying sql subscripiton rule that matches the event type name stored in the event properites collection:

snippet: SubscriberAFilter

The other subscriber uses [`TrueFilter`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.servicebus.messaging.truefilter?view=azure-dotnet) ensuring that both `EventOne` and `EventTwo` events are routed to it's input queue.

## Things to note

 * The use of the `AzureServiceBus_ConnectionString` environment variable mentioned above.
 * Execute `Publisher` first to ensure that the topic exists. 
