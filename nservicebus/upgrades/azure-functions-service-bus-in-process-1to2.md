---
title: Azure Functions (in-process) for Service Bus Upgrade Version 1 to 2
summary: How to upgrade Azure Functions (in-process) with Azure Service Bus from version 1 to 2
component: ASBFunctions
reviewed: 2021-12-16
related:
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---

This version relies on [`NServiceBus.Transport.AzureServiceBus` Version 2](/transports/azure-service-bus/) which uses the new [`Azure.Messaging.ServiceBus` SDK from Microsoft](https://docs.microsoft.com/en-us/dotnet/api/overview/azure/messaging.servicebus-readme). See the [transport upgrade guide for more details](/transports/upgrades/asbs-1to2.md).

## Registering NServiceBus

All public constructors of `ServiceBusTriggeredEndpointConfiguration` have been made internal.

Instead of

```csharp
functionsHostBuilder.UseNServiceBus(() => new ServiceBusTriggeredEndpointConfiguration(endpointName));
```

use:

```csharp
functionsHostBuilder.UseNServiceBus(endpointName);
```

The endpoint name can be inferred from `NServiceBusTriggerFunctionAttribute` if present.

```csharp
[assembly: NServiceBusTriggerFunction("MyEndpoint")]

// ...

functionsHostBuilder.UseNServiceBus(); // Will use the name MyEndpoint
```

NOTE: The constructed instance of `NServiceBusTriggeredEndpointConfiguration` already contains a reference to an `IConfiguration` instance from the host environment. It is not required to pass one in.

### Routing

Instead of

```csharp
functionsHostBuilder.UseNServiceBus(() =>
{
    var serviceBusTriggeredEndpointConfiguration = new ServiceBusTriggeredEndpointConfiguration(endpointName);
    var routing = serviceBusTriggeredEndpointConfiguration.Transport.Routing();
    routing.RouteToEndpoint(typeof(SomeMessage), "AnotherEndpoint");

    return serviceBusTriggeredEndpointConfiguration;
});
```

use:

```csharp
functionsHostBuilder.UseNServiceBus(endpointName, nsb =>
{
    nsb.Routing.RouteToEndpoint(typeof(SomeMessage), "AnotherEndpoint");
});
```

## Static hosting model has been deprecated

The `ServiceBusTriggeredEndpointConfiguration.FromAttributes()` static factory method that was used in the preview package has been deprecated. Instead, use [the host builder configuration model](/nservicebus/hosting/azure-functions-service-bus/#basic-usage-endpoint-configuration).
