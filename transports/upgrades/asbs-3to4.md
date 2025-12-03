---
title: Azure Service Bus Transport Upgrade Version 3 to 4
summary: Instructions on how to upgrade Azure Service Bus transport from version 3 to 4
reviewed: 2023-12-12
component: ASBS
related:
 - transports/azure-service-bus
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 8
 - 9
---

## Topic name configuration

The API to configure the [topic name used for publish and subscribe](/transports/azure-service-bus/configuration.md#entity-creation-topology) has been changed, instead of:

snippet: 3to4-asbs-topic-name-old

Use:

snippet: 3to4-asbs-topic-name-new

For more advanced topology configurations, see the [topology documentation](/transports/azure-service-bus/topology.md).
