---
title: Azure Service Bus Pub/Sub Native Integration
summary: How to consume event messages published by non-NServiceBus endpoints.
component: ASBS
isLearningPath: true
reviewed: 2024-03-01
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

## Running the sample

1. First, run the `Publisher` project by itself. This will create all the necessary publish/subscribe infrastructure in Azure Service Bus, including the default `bundle-1` topic.
2. Run the projects normally so that all endpoints start.
3. In the `Publisher` window, press any key to publish an event.
    * The endpoint in the `NativeSubscriberA` window will receive `EventOne`.
    * The endpoint in the `NativeSubscriberB` window will receive both `EventOne` and `EventTwo`.
 
## Setting up namespace entities

Each subscriber requires a topic subscription to receive the events published by the `Publisher`. The subscriptions are created on the `bundle-1` topic, which is [the default](/transports/azure-service-bus/configuration.md#entity-creation) name used by NServiceBus endpoints. 

snippet: SubscriptionCreation

### Subscription filters

Subscriptions created by `NativeSubscriberA` and `NativeSubscriberB` contain a single filtering rule. `NativeSubscriberA` subscribes to the `EventOne` events only by specifying a SQL subscription rule (`event-one`) that matches the event type name stored in the event properties collection:

snippet: EventOneSubscription

The other subscriber uses [`TrueFilter`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.servicebus.messaging.truefilter?view=azure-dotnet) in the `all-events` rule which ensures that both `EventOne` and `EventTwo` events are routed to its subscription.
