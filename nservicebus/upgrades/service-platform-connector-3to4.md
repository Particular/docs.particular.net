---
title: ServicePlatform Connector Upgrade Version 3 to 4
summary: Instructions on how to upgrade the ServicePlatform Connector from version 3 to 4
reviewed: 2025-08-29
component: PlatformConnector
isUpgradeGuide: true
upgradeGuideCoreVersions:
  - 9
  - 10
---

## Metrics TimeToBeReceived renamed to TimeToLive

In version 4, the setting to configure the maximum time to live for [Metrics messages](/platform/json-schema.md#metrics) has been renamed from `TimeToBeReceived` to `TimeToLive`. This aligns it with the naming convention of similar settings for other sections of the configuration.

snippet: PlatformConnector-3to4-TimeToBeReceived