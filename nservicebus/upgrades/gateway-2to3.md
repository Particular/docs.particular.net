---
title: Gateway Upgrade Version 2 to 3
summary: Instructions on how to upgrade the Gateway from Version 2 to 3.
component: Gateway
reviewed: 2017-10-03
related:
 - nservicebus/upgrades/6to7
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---

## Code first API

The gateway now allows full configuration via code. Use:

snippet: 2to3EnableGatewayAfter

to enable and configure sites and channels instead of:

snippet: 2to3EnableGatewayBefore
