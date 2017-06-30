---
title: Azure Service Bus Transport Upgrade Version 7 to 8
summary: Instructions on how to upgrade Azure Service Bus Transport Version 7 to 8.
reviewed: 2017-05-13
component: ASB
related:
 - transports/azure-service-bus
 - nservicebus/upgrades/5to6
redirects:
 - nservicebus/upgrades/asb-7to8
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---


## [Forwarding Topology](/transports/azure-service-bus/topologies/) number of entities in bundle removed

In Versions 8 and above the API to configure bundle prefix and number of entities in a bundle has been removed:

snippet: 7to8-number-of-entities-bundle

The bundle is set to one entity. For existing endpoints running with multiple entities in a bundle, the transport automatically picks up previously configured entities. The default topic name for bundle is set to `bundle-1`.

See also [Azure Service Bus Transport Topologies](/transports/azure-service-bus/topologies/).


## BrokeredMessage conventions

API to specify how the `BrokeredMessage` body is stored and retrieved by overriding the default conventions is obsoleted and the following methods were deprecated:

snippet: asb-incoming-message-convention

snippet: asb-outgoing-message-convention
