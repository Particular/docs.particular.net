---
title: ServicePlatform Connector Upgrade Version 3 to 4
reviewed: 2025-08-29
component: PlatformConnector
isUpgradeGuide: true
upgradeGuideCoreVersions:
  - 9
  - 10
---

## TimeToBeReceived has been deprecated

Up until version 3, the setting to configure the maximum time to live for [Metrics messages](/platform/json-schema.md#metrics), was called `TimeToBeReceived`.
In version 4, its name has been updated to `TimeToLive` to align with the naming convention of similar settings for other sections of the configuration.

snippet: PlatformConnector-3to4-TimeToBeReceived