---
title: Azure Functions with Azure Service Bus Upgrade Version 1 to 2
summary: How to upgrade Azure Functions with Azure Service Bus from version 1 to 2
component: ASBFunctions
reviewed: 2021-08-30
related:
 - nservicebus/upgrades/7to8
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
 - 8
---

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

### Connection strings

Specifying a connection string by name has been deprecated.

Instead of

```csharp
functionsHostBuilder.UseNServiceBus(
    () => new ServiceBusTriggeredEndpointConfiguration(endpointName, connectionStringName)
);
```

use:

```csharp
functionsHostBuilder.UseNServiceBus(endpointName, nsb =>
{
    var connectionString = Environment.GetEnvironmentVariable(connectionStringName);
    nsb.ServiceBusConnectionString = connectionString;
});
```

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