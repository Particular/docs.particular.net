---
title: Metrics Upgrade Version 1 to 2
reviewed: 2017-09-05
component: metrics
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---


## RegisterObservers replaces basic methods

If upgrading from version 1.0 the `EnableMetricTracing`, `EnableCustomReport` and `EnableLogTracing` methods have been deprecated and replaced with the more extensible [`RegisterObservers`](/nservicebus/operations/metrics/#reporting-metrics-data-to-any-external-storage). See [Upgrade Version 1 to 1.1](/nservicebus/upgrades/metrics-1to1.1.md#registerobservers-replaces-basic-methods).


## Changes in RegisterObservers

The [RegisterObservers API](/nservicebus/operations/metrics/#reporting-metrics-data-to-any-external-storage) has changed to take `ref` events.

snippet: 1to2RegisterObservers
