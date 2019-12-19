---
title: Gateway Upgrade Version 3 to 3.1
summary: How to upgrade the Gateway from version 3 to 3.1
component: Gateway
reviewed: 2019-12-17
related:
 - nservicebus/upgrades/6to7
isUpgradeGuide: true
---

The gateway supports a new storage configuration API:

snippet: 3to31StorageAPI

This replaces:

snippet: 2to3EnableGatewayAfter


NOTE: Please check the persistence if this supports the new gateway deduplication storage API.

