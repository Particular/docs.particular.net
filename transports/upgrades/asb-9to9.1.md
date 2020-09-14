---
title: Azure Service Bus Transport Upgrade Version 9 to 9.1
summary: Tips when upgrading Azure Service Bus transport from version 9 to 9.1.
reviewed: 2020-09-14
component: ASB
related:
 - transports/azure-service-bus
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
 ---


## Migrating from endpoint-oriented topology to forwarding topology

Before migrating to the new Azure Service Bus transport, a system using the endpoint-oriented topology must be migrated to the forwarding topology. Migration works side-by-side and the steps are described in the [migration documentation](/transports/azure-service-bus/legacy/migration.md).
