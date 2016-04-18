---
title: Azure Service Bus Transport
summary: Using Azure Service Bus Transport.
reviewed: 2016-03-21
component: ASB
tags:
related:
- nservicebus/azure-servicebus
---

## Prerequisites

An environment variable named `SamplesAzureServiceBusConnection` that contains the connection string for the Azure Service Bus namespace.


## Azure Service Bus Transport

This sample utilizes the [Azure Service Bus Transport](/nservicebus/azure-servicebus/).


## Code walk-through

This sample shows a simple two endpoint scenario.

* `Endpoint1` sends a `Message1` message to `Endpoint1`
* `Endpoint2` replies to `Endpoint1` with a `Message2`.


### Azure Service Bus configuration

The `Server` endpoint is configured to use the Azure Storage persistence in two locations.

snippet:Config

Some things to note:

 * The use of the `SamplesAzureServiceBusConnection` environment variable mention above.
 * The use of `UseSingleBrokerQueue` prevents the Azure transport individualizing queue names by appending the machine name.


## The Data in Azure Storage

The queues for the two endpoints can be seen in the Azure Portal.

![](queues.png)