---
title: Azure Functions with Azure Service Bus
component: ASBFunctions
summary: Hosting NServiceBus endpoints with Azure Functions triggered by Azure Service Bus
related:
 - samples/azure-functions/service-bus
redirects:
 - previews/azure-functions-service-bus
reviewed: 2021-04-19
---

Host NServiceBus endpoints with [Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/) and [Azure Service Bus](https://azure.microsoft.com/en-us/services/service-bus/) triggers.

## Basic usage

### Endpoint configuration

NServiceBus can be registered and configured on the host builder using the `UseNServiceBus` extension method in the startup class:

snippet: asb-function-hostbuilder

Any services registered via the `IFunctionsHostBuilder` will be available to message handlers via dependency injection. The startup class must be declared via the `FunctionStartup` attribute: `[assembly: FunctionsStartup(typeof(Startup))]`.

### Azure Function definition

To access `IFunctionEndpoint` from the Azure Function trigger, inject the `IFunctionEndpoint` via constructor-injection into the containing class:

snippet: asb-function-hostbuilder-trigger

### Dispatching outside a message handler

Triggering a message using HTTP function:

snippet: asb-dispatching-outside-message-handler

## Configuration

### License

The license is provided via the `NSERVICEBUS_LICENSE` environment variable, which is set via the Function settings in the Azure Portal.
For local development, use `local.settings.json`. In Azure, specify a Function setting using the environment variable as the key.

include: license-file-local-setting-file

### Custom diagnostics

[NServiceBus startup diagnostics](/nservicebus/hosting/startup-diagnostics.md) are disabled by default when using Azure Functions. Diagnostics can be written to the logs via the following snippet:

snippet: asb-enable-diagnostics

### Error queue

For recoverability to move the continuously failing messages to the error queue rather than to the Azure Service Bus dead-letter queue, the error queue must be created in advance and configured using the following API:

snippet: asb-configure-error-queue

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

WARN: If the `asb-tranport` command-line tool is not used to create the queue, it is recommended to set the `MaxDeliveryCount` setting to the maximum value.

### Subscribing to events

```
asb-transport endpoint subscribe <queue name> <eventtype>
```

See the [full documentation](/transports/azure-service-bus/operational-scripting.md#operational-scripting-asb-transport-endpoint-subscribe) for the `asb-transport endpoint subscribe` command for more details.
