---
title: Azure Service Bus Transport Upgrade Version 5 to 6
summary: Instructions on how to upgrade Azure Service Bus transport from version 5 to 6
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

The `NServiceBus.Transport.Encoding` header is longer used, but the transport could still send it for compatibility with the legacy Azure Service Bus transport. The ability to send this header has been removed and the `SendTransportEncodingHeader` APIs to opt in to sending it have been deprecated.
Remove all references to these APIs.