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

## Configuration

When running on .NET Core, configuration options in configuration files will **no longer be automatically detected**. Use the code first API instead.

When running on the .NET Framework, settings in configuration files will still be used, however a warning will be logged indicating that it should be explicitly configured using the code first API instead.
