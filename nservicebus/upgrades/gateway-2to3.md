---
title: Gateway Upgrade Version 2 to 3
summary: How to upgrade the Gateway from version 2 to 3.
component: Gateway
reviewed: 2021-08-04
related:
 - nservicebus/upgrades/6to7
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---

## Code-first API

The gateway now allows full configuration via code. To enable the gateway and configure sites and channels:

snippet: 2to3EnableGatewayAfter

The above code replaces the previous method of configuring the gateway so the following should no longer be used: 

snippet: 2to3EnableGatewayBefore

## Configuration

When running on .NET Core, options in configuration files are **not automatically detected**. Use the code-first API instead.

When running on the .NET Framework, settings in configuration files will still be used. However a warning will be logged indicating that it should be explicitly configured using the code-first API instead.
