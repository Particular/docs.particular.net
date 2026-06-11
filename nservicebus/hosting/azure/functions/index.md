---
title: Azure Functions hosting
component: AzureFunctions
summary: Hosting NServiceBus endpoints in Azure Functions with the AzureServiceBus package
reviewed: 2026-06-05
related:
  - transports/azure-service-bus
  - nservicebus/hosting/startup-diagnostics
  - nservicebus/hosting/override-hostid
  - samples/azure-functions/service-bus
  - nservicebus/upgrades/azure-functions-service-bus-isolated-to-azure-functions  
---

NServiceBus endpoints can be hosted in an Azure Functions app using the [`NServiceBus.AzureFunctions.AzureServiceBus`](https://www.nuget.org/packages/NServiceBus.AzureFunctions.AzureServiceBus) package.

Integration is enabled in `Program.cs` by calling `AddNServiceBusFunctions`:

snippet: azure-functions-program-builder

Endpoints are declared using the `[NServiceBusFunction]` attribute:

snippet: azure-functions-basic-endpoint

NServiceBus enabled functions must be declared in a partial class and are composed of two parts:

- A partial method decorated with a `[NServiceBusFunction]` and a `[Function("MyFunction")]` attribute declaring an [Azure Service Bus trigger](https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-service-bus-trigger).
  - The trigger must set `AutoCompleteMessages = false` since the NServiceBus pipeline takes responsibility for completing or abandoning each message based on handler outcomes; the [`NSBFUNC005` analyzer](analyzers.md) enforces this requirement.
  - A source generator will emit the method body that forwards each incoming message to the NServiceBus pipeline.
- A static `Configure{FunctionName}` method that configures the NServiceBus endpoint for the function.

For endpoint configuration, supported transport options, and explicit handler and saga registration, etc, see [Configuration](/nservicebus/hosting/azure/functions/configuration.md).

## Hosting multiple endpoints

Multiple methods decorated with `[NServiceBusFunction]` can co-exist in one Functions app. Each is registered as an independent NServiceBus endpoint with its own queue, handlers, and configure method:

snippet: azure-functions-multiple-endpoints

Each endpoint has its own `Configure{FunctionName}` method; the source generator routes each function to its matching configure method. Endpoints share the host's service provider but maintain independent message-handling pipelines.

## Send-only endpoints

A [send-only endpoint](/nservicebus/endpoints/#send-only) can be declared for components that need to dispatch messages without receiving incoming messages. Send-only endpoints can be used, for example, from a `TimerTrigger` or an `HttpTrigger` function serving as an HTTP API:

snippet: azure-functions-sendonly-registration

The attribute name becomes the [keyed-services](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection#keyed-services) key used to resolve `IMessageSession` and any endpoint-scoped services. Set the optional `Connection` property to override the default connection setting name.

Send-only endpoints are discovered and registered automatically by `builder.AddNServiceBusFunctions()`.

snippet: azure-functions-sendonly-usage
