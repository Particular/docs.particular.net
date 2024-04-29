---
title: Azure Functions (Isolated Worker) Upgrade Version 3 to 4
summary: How to upgrade Azure Functions (Isolated Worker) with Service Bus from version 3 to 4
component: ASBFunctionsWorker
reviewed: 2021-12-16
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
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
