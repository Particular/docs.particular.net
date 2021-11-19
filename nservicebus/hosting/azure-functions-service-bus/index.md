---
title: Azure Functions with Azure Service Bus
component: ASBFunctions
summary: Hosting NServiceBus endpoints with Azure Functions triggered by Azure Service Bus
redirects:
 - previews/azure-functions-service-bus
 - nservicebus/hosting/azure-functions
 - nservicebus/hosting/azure-functions/service-bus
related:
 - samples/azure-functions/service-bus
reviewed: 2021-07-14
---

Host NServiceBus endpoints with [Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/) and [Azure Service Bus](https://azure.microsoft.com/en-us/services/service-bus/) triggers.

include: scheduled-message-scaling-warning

## Basic usage

### Endpoint configuration

NServiceBus can be registered and configured on the host builder using the `UseNServiceBus` extension method in the startup class:

partial: configuration

Any services registered via the `IFunctionsHostBuilder` will be available to message handlers via dependency injection. The startup class must be declared via the `FunctionStartup` attribute: `[assembly: FunctionsStartup(typeof(Startup))]`.

### Azure Function queue trigger for NServiceBus

partial: queue-trigger-wiring

### Dispatching outside a message handler

Triggering a message using HTTP function:

snippet: asb-dispatching-outside-message-handler

## Message consistency

partial: message-consistency

## Configuration

`ServiceBusTriggeredEndpointConfiguration` loads certain configuration values from the Azure Function host environment in the following order:

- `IConfiguration` passed in via the constructor
- Environment variables

| Key                      | Value      | Notes     |
|--------------------------|------------|-----------|
| `AzureWebJobsServiceBus` | Connection string for the Azure ServiceBus namespace to connect to | This value is required for `ServiceBusTriggerAttribute`. An alternative key can be passed into the constructor. |
| `ENDPOINT_NAME`          | The name of the NServiceBus endpoint to host | A value can be provided directly to the constructor. |
| `NSERVICEBUS_LICENSE`    | The NServiceBus license | Can also be provided via `serviceBusTriggeredEndpointConfig.EndpointConfiguration.License(...)`. |
| `WEBSITE_SITE_NAME`      | The name of the Azure Function app. Provided when hosting the function in Azure. | Used to set the NServiceBus [host identifier](/nservicebus/hosting/override-hostid.md). Local machine name is used if not set. |

For local development, use `local.settings.json`. In Azure, specify a Function setting using the environment variable as the key.

include: license-file-local-setting-file

### Custom diagnostics

[NServiceBus startup diagnostics](/nservicebus/hosting/startup-diagnostics.md) are disabled by default when using Azure Functions. Diagnostics can be written to the logs via the following snippet:

snippet: asb-enable-diagnostics

### Error queue

For recoverability to move the continuously failing messages to the error queue rather than to the Azure Service Bus dead-letter queue, the error queue must be created in advance and configured using the following API:

snippet: asb-configure-error-queue

### Known constraints and limitations

The Configuration API exposes NServiceBus transport configuration options via the `configuration.Transport` property to allow customization; however, not all of the options will be applicable to execution within Azure Functions.

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

partial: assembly-scanner

## Package requirements

`NServiceBus.AzureFunctions.Worker.ServiceBus` requires Visual Studio 2019 and .NET SDK version `5.0.300` or higher. Older versions of the .NET SDK might display the following warning which prevents the trigger definition from being auto-generated:

```
CSC : warning CS8032: An instance of analyzer NServiceBus.AzureFunctions.SourceGenerator.TriggerFunctionGenerator cannot be created from NServiceBus.AzureFunctions.SourceGenerator.dll : Could not load file or assembly 'Microsoft.CodeAnalysis, Version=3.10.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'. The system cannot find the file specified..
```
