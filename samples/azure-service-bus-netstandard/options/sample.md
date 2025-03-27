---
title: Azure Service Bus Topology Options
summary: How to use topology options to layout the topic topology in app settings.
component: ASBS
reviewed: 2025-02-12
related:
 - transports/azure-service-bus
---

This sample shows how to leverage topology options to layout the topic topology used within the Application configuration. The sample uses the generic host for convenient loading of configuration via the built-in configuration provider model. The `appsettings.json` is used for demonstration purposes and technically the options could be loaded from other sources.

## Prerequisites

include: asb-connectionstring-xplat

## Code walk-through

The sample contains three executable projects:

* `Publisher`: an NServiceBus endpoint that publishes `EventOne` to topic `event-one` and `EventTwo` to `event-two`.
* `Subscriber`: an NServiceBus endpoint subscribing to the `EventOne` and `EventTwo` event published by the `Publisher`.

### Configuration from options

With the generic hosts ability to load configuration sections it is a matter of loading the topology options from the section in the Application configuration as shown below:

Snippet: OptionsLoading

In this example the publisher overrides the default topic destination to a custom conventions instead of using the default fullname of the event type:

Snippet: PublisherAppsettings

The subscriber needs to override the subscription mapping accordingly:

Snippet: SubscriberAppsettings

> [!NOTE]
> While in the vast majority of cases there is a one-to-one correspondence between event types and topic names, the `SubscribedEventToTopicsMap` maps arrays of topics in order to support [event inheritence](/transports/azure-service-bus/topology.md#subscription-rule-matching-interface-based-inheritance). Due to a limitation of the .NET configuration infrastructure, it is not possible to provide a converter that simplifies the syntax in a way similar to when [loading the topology configuration directly from json](/transports/azure-service-bus/configuration.md#entity-creation-topology-mapping-options).

### Validation

The transport provides integration with Microsoft.Extensions.Options and has a built-in options validator. With the generic host it is possible to register the validator to make sure the configuration loaded fulfills the requirements of the broker (e.g. topic name length) and is self-consistent.

Snippet: OptionsValidation

## Running the sample

1. First, run the `Subscriber` project by itself. This will create all the necessary publish/subscribe infrastructure in Azure Service Bus.
2. Run the projects normally so that all endpoints start.
3. The `Publisher` endpoint continuously publishes two events with a short pause in between.
    * The endpoint in the `Subscriber` window receives both `EventOne` and `EventTwo`.
