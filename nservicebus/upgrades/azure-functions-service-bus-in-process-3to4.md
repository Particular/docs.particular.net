---
title: Azure Functions (in-process) for Service Bus Upgrade Version 3 to 4
summary: How to upgrade Azure Functions (in-process) with Azure Service Bus from version 3 to version 4
component: ASBFunctions
reviewed: 2021-12-16
related:
 - nservicebus/upgrades/7to8
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---

## Code first API to set connection string

Setting the connection string can now be done when calling `UseNServiceBus()`

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
