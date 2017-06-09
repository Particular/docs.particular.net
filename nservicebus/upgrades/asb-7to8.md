---
title: Azure Service Bus Transport Upgrade Version 7 to 8
summary: Instructions on how to upgrade Azure Service Bus Transport Version 7 to 8.
reviewed: 2017-05-05
component: ASB
related:
 - nservicebus/azure-service-bus
 - nservicebus/upgrades/5to6
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---


## [Forwarding Topology](/nservicebus/azure-service-bus/topologies/) number of entities in bundle removed

In Versions 8 and above the API to configure number of entities in a bundle has been removed:

snippet: 7to8-number-of-entities-bundle

The default number of entities in a bundle is set to one. For existing transports which have been running with multiple bundles the transport automatically picks up previously configured bundles.

For more details on topologies refer to the [Azure Service Bus Transport Topologies](/nservicebus/azure-service-bus/topologies/) article.