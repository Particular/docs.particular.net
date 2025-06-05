---
title: Azure Functions with Azure Service Bus (In Process)
component: ASBFunctions
summary: Hosting NServiceBus endpoints with Azure Functions (in-process hosting model) triggered by Azure Service Bus
redirects:
 - previews/azure-functions-service-bus
 - nservicebus/hosting/azure-functions
 - nservicebus/hosting/azure-functions/service-bus
related:
 - samples/azure-functions/service-bus
reviewed: 2025-06-05
---

Host NServiceBus endpoints with [Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/) and [Azure Service Bus](https://azure.microsoft.com/en-us/services/service-bus/) triggers.

> [!WARNING]
> Microsoft announced that .NET 8 will be [the last release supporting the in-process hosting model](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/net-on-azure-functions-august-2023-roadmap-update/ba-p/3910098). New projects should use the [isolated worker model](/nservicebus/hosting/azure-functions-service-bus/) instead.

## Basic usage

### Endpoint configuration

NServiceBus can be registered and configured on the host builder using the `UseNServiceBus` extension method in the startup class:

partial: endpoint-configuration

Any services registered via the `IFunctionsHostBuilder` will be available to message handlers via dependency injection. The startup class must be declared via the `FunctionStartup` attribute: `[assembly: FunctionsStartup(typeof(Startup))]`.

### Azure Function queue trigger for NServiceBus

partial: queue-trigger-wiring

### Dispatching outside a message handler

Triggering a message using HTTP function:

snippet: asb-dispatching-outside-message-handler

## Transport configuration constraints and limitations

The configuration API exposes NServiceBus transport configuration options via the `configuration.Transport` property to allow customization. However, not all options will be applicable for execution within Azure Functions (for more details, see [analysers](./analyzers.md)).

Concurrency-related settings are controlled via the Azure Function `host.json` configuration file. See [Concurrency in Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/functions-concurrency#service-bus) for details.

partial: no-use-transport

## Message consistency

partial: message-consistency

## Configuration

partial: configuration

### Custom diagnostics

[NServiceBus startup diagnostics](/nservicebus/hosting/startup-diagnostics.md) are disabled by default when using Azure Functions. Diagnostics can be written to the logs via the following snippet:

snippet: asb-enable-diagnostics

Diagnostics data will be written with logger identification `StartupDiagnostics` with log level *Informational*.

### Error queue

For recoverability to move the continuously failing messages to the error queue rather than to the Azure Service Bus dead-letter queue, the error queue must be created in advance and configured using the following API:

snippet: asb-configure-error-queue

## Preparing the Azure Service Bus namespace

Function endpoints cannot create their own queues or other infrastructure in the Azure Service Bus namespace.

Use the [`asb-transport` command line (CLI) tool](/transports/azure-service-bus/operational-scripting.md) to provision the entities in the namespace for the Function endpoint.

### Creating the endpoint queue

```txt
asb-transport endpoint create <queue name>
```

See the [full documentation](/transports/azure-service-bus/operational-scripting.md#available-commands-asb-transport-endpoint-create) for the `asb-transport endpoint create` command for more details.

> [!WARNING]
> If the `asb-tranport` command-line tool is not used to create the queue, it is recommended to set the `MaxDeliveryCount` setting to the maximum value.

### Subscribing to events

```txt
asb-transport endpoint subscribe <queue name> <eventtype>
```

See the [full documentation](/transports/azure-service-bus/operational-scripting.md#available-commands-asb-transport-endpoint-subscribe) for the `asb-transport endpoint subscribe` command for more details.

partial: topology

partial: assembly-scanner

## Package requirements

`NServiceBus.AzureFunctions.Worker.ServiceBus` requires Visual Studio 2019 and .NET SDK version `5.0.300` or higher. Older versions of the .NET SDK might display the following warning which prevents the trigger definition from being auto-generated:

```txt
CSC : warning CS8032: An instance of analyzer NServiceBus.AzureFunctions.SourceGenerator.TriggerFunctionGenerator cannot be created from NServiceBus.AzureFunctions.SourceGenerator.dll : Could not load file or assembly 'Microsoft.CodeAnalysis, Version=3.10.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'. The system cannot find the file specified..
```

Starting in version 4.1.0 of `NServiceBus.AzureFunctions.Worker.ServiceBus` and `NServiceBus.AzureFunctions.InProcess.ServiceBus`
, warning CS8032 is treated as an error. To suppress this error, update the project's .csproj file to include the following line in the `<PropertyGroup>` section at the top:

```xml
  <WarningsAsErrors></WarningsAsErrors>
```

This will revert CS8032 to a warning status so that it does not stop the build process.
