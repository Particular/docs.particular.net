---
title: Metrics Upgrade Version 1 to 2
reviewed: 2021-06-05
component: metrics
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---


include: metrics-registerobservers


## Changes in RegisterObservers

The [RegisterObservers API](/monitoring/metrics/raw.md#reporting-metrics-data-to-any-external-storage) has changed to take `ref` events.

snippet: 1to2RegisterObservers
