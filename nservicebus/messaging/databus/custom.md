---
title: Customizing the data bus
summary: Demonstrates how to register and plug in a customized data bus implementation into an endpoint.
component: Core
reviewed: 2021-05-11
redirects:
- nservicebus/plugin-custom-databus
related:
- samples/databus/custom-serializer
---

Endpoints support sending and receiving large chunks of data via the [data bus](./).

It is possible to create a custom data bus implementation. This is done by making use of the [Features](/nservicebus/pipeline/features.md) extension.

## Implement the `IDataBus` interface

This new class will provide the custom implementations for the `Get` and `Put` methods for the data bus.

snippet: CustomDataBus

This new implementation needs to be registered as a new feature.

## Define a feature

Define a [new feature](/nservicebus/pipeline/features.md) that registers the custom data bus implementation class.

snippet: CustomDataBusFeature

## Define a DataBusDefinition

Define a new class which inherits from the `DataBusDefinition` class.

snippet: CustomDataBusDefinition

## Configure the endpoint

Configure the endpoint to use the custom data bus implementation instead of the default data bus:

snippet: PluginCustomDataBus
