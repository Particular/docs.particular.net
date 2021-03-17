---
title: Azure Storage Queues Transport Upgrade Version 9 to 10
summary: Instructions on how to upgrade the Azure Storage Queues transport from version 9 to 10.
reviewed: 2021-03-12
component: ASQ
related:
- transports/azure-storage-queues
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---

## Cross Account Routing

### Sending a message

To send a message to a receiver on another storage account, the configuration changed as follows

snippet: storage_account_routing_registered_endpoint


### Subscribing on an event

To subscribe to an event, coming from a publisher on another storage account, the configuration changed as follows

snippet: storage_account_routing_registered_publisher


### Publishing an event

To publish an event to a subscriber in another storage account, the configuration as follows

snippet: storage_account_routing_registered_subscriber