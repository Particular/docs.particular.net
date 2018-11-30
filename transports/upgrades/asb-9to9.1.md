---
title: Azure Service Bus Transport Upgrade Version 9 to 9.1
summary: Tips when upgrading Azure Service Bus transport from version 9 to 9.1.
reviewed: 2018-11-29
component: ASB
related:
 - transports/azure-service-bus
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
 ---


## Migrating from Endpoint-Oriented to Forwarding topology

Customers willing to migrate to the new Azure Service Bus Transport and currently using Endpoint-Oriented topology are required to first migrate off Endpoint-Oriented topology to Forwarding topology. Migration is a side-by-side migration and steps are described in [Migration](/transports/azure-service-bus/legacy/migration.md) article.
