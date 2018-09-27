---
title: Azure Service Bus Transport Upgrade Version 8 to 9
summary: Instructions on how to upgrade Azure Service Bus Transport Version 8 to 9.
reviewed: 2018-10-27
component: ASB
related:
 - transports/azure-service-bus
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 8
 - 9
---


## .NET Framework 4.6 requirement

Azure Service Bus Transport version 9 depends on Azure Service Bus client version 5 which requires .NET Framework 4.6.


## Client-side batching (flush interval) aligned with Microsoft Azure Service Bus client default

The default value for client-side batching configured using [`BatchFlushInterval(TimeSpan)` API](/transports/azure-service-bus/configuration/full.md#controlling-connectivity-messaging-factories) has changed from 0.5 sec to 20 ms to align better with Azure Service Bus client [default value](https://docs.microsoft.com/en-us/dotnet/api/microsoft.servicebus.messaging.messagesender.batchflushinterval]. 