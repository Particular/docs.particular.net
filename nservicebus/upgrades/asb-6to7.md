---
title: Azure Service Bus Transport Upgrade Version 6 to 7
summary: Instructions on how to upgrade Azure Service Bus Transport Version 6 to 7.
reviewed: 2016-04-19
tags:
 - upgrade
 - migration
related:
- nservicebus/azure-service-bus
- nservicebus/upgrades/5to6
---


## [Topology](/nservicebus/azure-service-bus/topologies/) is mandatory

In version 7 and above the topology selection is mandatory:

snippet:topology-selection-upgrade-guide


When the `EndpointOrientedTopology` is selected, it is also necessary to configure [publisher names](/nservicebus/azure-service-bus/publisher-names-configuration.md), in order to ensure that subscribers receive event messages:

snippet:publisher_names_mapping_upgrade_guide
