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

The setting `TimeToBeReceived` for the [Metrics feature](/platform/json-schema.md#metrics) has been deprecated in favor of `TimeToLive`.

snippet: PlatformConnector-3to4-TimeToBeReceived