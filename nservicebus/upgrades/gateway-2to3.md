---
title: Gateway Upgrade Version 2 to 3
summary: Instructions on how to upgrade the Gateway from Version 2 to 3.
component: Gateway
related:
 - nservicebus/upgrades/6to7
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---

## Code first API

The gateway now allows full configuration via code. Use:

snippet: EnableGatewayAfter

to enable and confgure sites and channels instead of:

snippet: EnableGatewayBefore
