---
title: Azure Functions with Azure Service Bus
component: ASBFunctions
summary: Azure Functions Preview
related:
 - samples/previews/azure-functions
reviewed: 2020-06-18
---

Host NServiceBus endpoints with [Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/).

## Basic Usage

### Endpoint configuration

snippet: endpoint-configuration

The endpoint is automatically configured with the endpoint name and the transport connection string based on the values defined in the `[ServiceBusTrigger]` attribute using the `ServiceBusTriggeredEndpointConfiguration.FromAttributes` method.

Alternatively, the endpoint name can be passed in manually:

snippet: alternative-endpoint-setup

### Azure Function definition

snippet: function-definition

## Configuration

## License

The license can be provided via the `NSERVICEBUS_LICENSE` environment variable which can be set via the Function settings in the Azure Portal.
For local development, `local.settings.json` can be used. In Azure, specify a Function setting using environment variable as the key.

include: license-file-local-setting-file

### Custom diagnostics

snippet: custom-diagnostics

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

WARN: When not using the `asb-tranport` command-line tool to create the queue, it is recommended to set the `MaxDeliveryCount` setting to the maximum value.

### Subscribing to events

```
asb-transport endpoint subscribe name eventtype
```

For full documentation see the [asb-transport endpoint subscribe]()
