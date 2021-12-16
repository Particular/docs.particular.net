---
title: Azure Functions (in-process) for Service Bus Upgrade Version 2 to 3
summary: How to upgrade Azure Functions (in-process) with Azure Service Bus from version 2 to 3
component: ASBFunctions
reviewed: 2021-12-16
related:
 - nservicebus/upgrades/7to8
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 8
---

## Code first API to set connection string

Setting the connection string can now be done as part of calling `UseNServiceBus()`

Instead of

```csharp
functionsHostBuilder.UseNServiceBus(() => new ServiceBusTriggeredEndpointConfiguration(endpointName){
    ConnectionString = "CONNECTIONSTRING"
});
```

use:

```csharp
functionsHostBuilder.UseNServiceBus(endpointName, "CONNECTIONSTRING");
```

## Injecting FunctionEndpoint

`FunctionEndpoint` can no longer be injected, use `IFunctionEndpoint` instead.
