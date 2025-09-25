---
title: Azure Service Bus Transport Upgrade Version 5 to 6
summary: Migration instructions on how to upgrade the Azure Service Bus transport from version 5 to 6
reviewed: 2025-09-18
component: ASBS
related:
 - transports/azure-service-bus
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 9
 - 10
---

## Transport Encoding Header

The `AzureServiceBusTransport.SendTransportEncodingHeader` API has been deprecated. The transport now relies on the [ContentType property](https://learn.microsoft.com/azure/service-bus-messaging/service-bus-messages-payloads#payload-serialization) of Azure Service Bus messages instead of a custom header. Remove any usage of this API.