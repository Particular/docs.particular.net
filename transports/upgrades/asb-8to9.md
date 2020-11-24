---
title: Azure Service Bus Transport (Legacy) Upgrade Version 8 to 9
summary: Tips when upgrading Azure Service Bus transport from version 8 to 9.
reviewed: 2020-11-23
component: ASB
related:
 - transports/azure-service-bus
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---


## .NET Framework 4.6 required

Azure Service Bus Transport version 9 depends on Azure Service Bus client version 5 which requires .NET Framework 4.6.


## Client-side batching (flush interval) aligned with Microsoft Azure Service Bus client default

The default value for client-side batching configured using [`BatchFlushInterval(TimeSpan)` API](/transports/azure-service-bus/legacy/configuration/full.md#controlling-connectivity-messaging-factories) has changed from 0.5 sec to 20 ms to align better with the Azure Service Bus client [default value](https://docs.microsoft.com/en-us/dotnet/api/microsoft.servicebus.messaging.messagesender.batchflushinterval).
