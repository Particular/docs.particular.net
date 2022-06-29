---
title: Newtonsoft Serializer Upgrade Version 2 to 3
summary: Serialization configuration changes from NServiceBus 7 to 8.
reviewed: 2022-06-22
component: Newtonsoft
related:
 - nservicebus/serialization
 - nservicebus/upgrades/7to8
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
 - 8
---

## NewtonsoftSerializer Obsolete

The `NewtonsoftSerializer` is obsolete in NServiceBus version 8. It uses `TypeNameHandling.Auto` as its default value which can be a security risk as it allows the message payload to control the deserialization target type. See [CA2326: Do not use TypeNameHandling values other than None](https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca2326) for further details on this vulnerability.

A new serializer `NewtonsoftJsonSerializer` has been introduced which uses `TypeNameHandling.None` as its default value.

Instead of:

```csharp
var serialization = endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
```

Use:

```csharp
var serialization = endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
```

If the use of `TypeNameHandling.Auto` is required, it can be achieved by customizing the instance of [JsonSerializerSettings](https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonSerializerSettings.htm) used for serialization.
See [the Json.Net Serializer documentation](/nservicebus/serialization/newtonsoft.md) for more information.
