---
title: Azure Functions (Isolated Worker) Upgrade Version 4 to 5
summary: How to upgrade Azure Functions (Isolated Worker) with Service Bus from version 4 to 5
component: ASBFunctionsWorker
reviewed: 2024-04-29
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 8
 - 9
---

## Default serializer has changed

The default serializer has changed from [Json.NET](/nservicebus/serialization/newtonsoft.md) to [System.Text.Json](/nservicebus/serialization/system-json.md).

The Json.NET serializer can continue to be used by adding a package reference to the `NServiceBus.Newtonsoft.Json` package and setting `NewtonsoftJsonSerializer` as the serializer:

snippet: ASBFunctionsWorker-4to5-serializer