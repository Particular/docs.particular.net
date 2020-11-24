---
title: Azure Service Bus Transport (Legacy) Upgrade Version 7 to 8
summary: Instructions on how to upgrade Azure Service Bus Transport Version 7 to 8.
reviewed: 2020-11-23
component: ASB
related:
 - transports/azure-service-bus
 - nservicebus/upgrades/6to7
redirects:
 - nservicebus/upgrades/asb-7to8
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---


## [Forwarding topology](/transports/azure-service-bus/legacy/topologies.md) number of entities in bundle removed

In Versions 8 and above the API to configure bundle prefix and number of entities in a bundle has been removed:

snippet: 7to8-number-of-entities-bundle

The bundle is set to one entity. For existing endpoints running with multiple entities in a bundle, the transport automatically picks up previously configured entities. The default topic name for bundle is set to `bundle-1`.

See also [Azure Service Bus Transport Topologies](/transports/azure-service-bus/legacy/topologies.md).


## Controlling entity creation

Controlling entity creation has been simplified. Instead of having to provide a full implementation of `DescriptionFactory` where all settings on the description object had to be provided, a customizer (`DescriptionCustomizer`) has been introduced where only the customized properties can be changed. `DescriptionCustomizer` can be found on the `Queues`, `Topics` and `Subscriptions` API extension points.


## Queue and Subscription MaxDeliveryCount

In version 7 the transport sets `MaxDeliveryCount` to match immediate retries specified by each endpoint to ensure messages are not dead-lettered accidentally.

In version 8 and above, `MaxDeliveryCount` is set to `int.MaxValue` to ensure messages are not dead-lettered accidentally and to remove the dependency on the endpoints' immediate retries configuration.

Customization of `MaxDeliveryCount` is strongly discouraged, yet can be performed using `DescriptionCustomizer` API for queues and topics.  

## BrokeredMessage conventions

Due to complexity, and resultant risk, of implementation, the API to specify how the `BrokeredMessage` body is stored and retrieved by overriding the default conventions is obsoleted and the following methods were deprecated:

snippet: 7to8_asb-incoming-message-convention

snippet: 7to8_asb-outgoing-message-convention


## Serialization is mandatory

In Versions 7 and below, the transport was setting the default serialization. In Versions 8 and above, the transport is no longer sets the default serialization. Instead, it should be configured.

For backwards compatibility, `NServiceBus.Newtonsoft.Json` serializer should be used.

snippet: 7to8_asb-backwardscompatible-serializer


## Send/publish namespaces caching

In Version 7 to control sending/publishing namespaces caching, the transport must provide an implementation of two contracts. 

snippet: custom-namespace-partitioning-strategy-with-caching

From Versions 8 and above, only a single contract is required.

snippet: 7to8_asb-namespace-partitioning-caching
