---
title: Azure Functions with Azure Service Bus
summary: Azure Functions Preview
related:
 - samples/previews/azure-functions
reviewed: 2020-06-18
---

## Basic Usage

## Configuration

## Known constraints and limitations

When using Azure Functions with Azure Service Bus (ASB) the following points must be taken into consideration:

- There is no support for the `SendsAtomicWithReceive` [transport transaction mode](/transports/transactions.md#transactions-transport-transaction-sends-atomic-with-receive).
- The Configuration API exposes NServiceBus transport configuration options via the `configuration.Transport` method to allow customization; however, not all of the options will be applicable to execution within Azure Functions. 

## Preparing the Azure Service Bus namespace

Function endpoints cannot create their own queues or other infrastructure in the Azure Service Bus namespace.

Use the [`asb-transport` command line (CLI) tool](/transports/azure-service-bus/operational-scripting.md) to provision the entities in the namespace for the Function endpoint.

### Creating the endpoint queue

```
asb-transport endpoint create 
```

WARN: When not using the `asb-tranport` command line tool to create the queue it is recommended the `MaxDeliveryCount` be set to the maximum value.

### Subscribing to events

```
asb-transport endpoint subscribe name eventtype
```

For full documentation see the [asb-transport endpoint subscribe]()