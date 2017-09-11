---
title: Azure Service Bus Transport Upgrade Version 7 to 8
summary: Instructions on how to upgrade Azure Service Bus Transport Version 7 to 8.
reviewed: 2017-08-31
component: ASB
related:
 - transports/azure-service-bus
 - nservicebus/upgrades/6to7
redirects:
 - nservicebus/upgrades/asb-7to8
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
 - 8
---


## [Forwarding Topology](/transports/azure-service-bus/topologies/) number of entities in bundle removed

In Versions 8 and above the API to configure bundle prefix and number of entities in a bundle has been removed:

snippet: 7to8-number-of-entities-bundle

The bundle is set to one entity. For existing endpoints running with multiple entities in a bundle, the transport automatically picks up previously configured entities. The default topic name for bundle is set to `bundle-1`.

See also [Azure Service Bus Transport Topologies](/transports/azure-service-bus/topologies/).


## Controlling entity creation

Controlling entity creation has been greatly simplified. Instead of having to provide a full implementation of `DescriptionFactory` where all settings on the description object had to be provided, a customizer has been introduced `DescriptionCustomizer` where only the to be customized property can be changed. `DescriptionCustomizer` can be found on the `Queues`, `Topics` and `Subscriptions` API extension points.

## BrokeredMessage conventions

Due to complexity of implementation, the API to specify how the `BrokeredMessage` body is stored and retrieved by overriding the default conventions is obsoleted and the following methods were deprecated:

snippet: asb-incoming-message-convention

snippet: asb-outgoing-message-convention