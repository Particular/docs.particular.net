---
title: Azure Functions with Azure Service Bus
component: ASBFunctions
summary: Hosting NServiceBus endpoints with Service Bus triggered Azure Functions
related:
 - samples/previews/azure-functions
reviewed: 2020-07-06
---

Host NServiceBus endpoints with [Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/) and [Azure Service Bus](https://azure.microsoft.com/en-us/services/service-bus/) triggers.

## Basic Usage

### Endpoint configuration

snippet: endpoint-configuration

The endpoint is automatically configured with the endpoint name and the transport connection string based on the values defined in the `[ServiceBusTrigger]` attribute using the `ServiceBusTriggeredEndpointConfiguration.FromAttributes` method.

Alternatively, the endpoint name can be passed in manually:

snippet: alternative-endpoint-setup

### Azure Function definition

Pass the incoming message to the NServiceBus endpoint:

snippet: function-definition

NServiceBus can directly log into the Azure Functions logging infrastructure by passing the `ILogger` instance from the function parameters to the endpoint's `Process` method.

## Configuration

### License

The license can be provided via the `NSERVICEBUS_LICENSE` environment variable which can be set via the Function settings in the Azure Portal.
For local development, `local.settings.json` can be used. In Azure, specify a Function setting using environment variable as the key.

include: license-file-local-setting-file

### Custom diagnostics

[NServiceBus startup diagnostics](nservicebus/hosting/startup-diagnostics.md) are disabled by default when using Azure Functions. Diagnostics can be written to the logs via the following snippet:

snippet: enable-diagnostics

## Known constraints and limitations

When using Azure Functions with Azure Service Bus (ASB) the following points must be taken into consideration:

- There is no support for the `SendsAtomicWithReceive` [transport transaction mode](/transports/transactions.md#transactions-transport-transaction-sends-atomic-with-receive).
- The Configuration API exposes NServiceBus transport configuration options via the `configuration.Transport` method to allow customization; however, not all of the options will be applicable to execution within Azure Functions. 

## Preparing the Azure Service Bus namespace

Function endpoints cannot create their own queues or other infrastructure in the Azure Service Bus namespace.

Use the [`asb-transport` command line (CLI) tool](/transports/azure-service-bus/operational-scripting.md) to provision the entities in the namespace for the Function endpoint.

### Creating the endpoint queue

```
asb-transport endpoint create <queue name>
```

See the [full documentation](/transports/azure-service-bus/operational-scripting.md#operational-scripting-asb-transport-endpoint-create) for the `asb-transport endpoint create` command for more details.

WARN: When not using the `asb-tranport` command-line tool to create the queue, it is recommended to set the `MaxDeliveryCount` setting to the maximum value.

### Subscribing to events

```
asb-transport endpoint subscribe <queue name> <eventtype>
```

See the [full documentation](/transports/azure-service-bus/operational-scripting.md#operational-scripting-asb-transport-endpoint-subscribe) for the `asb-transport endpoint subscribe` command for more details.
