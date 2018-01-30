---
title: Externalize Windows Performance Counters
reviewed: 2017-03-17
component: PerfCounters
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
related: 
 - monitoring/metrics/performance-counters
---

include: externalize-perfcounters

The API was also modified.


## Changed APIs

The NServiceBus Performance Counter APIs have been marked as obsolete and have one-for-one equivalents in the NServiceBus.Metrics.PerformanceCounters package.


### Enabling Critical Time Counter

snippet: 6to1-enable-criticaltime


### Enabling SLA Counter

snippet: 6to1-enable-sla


## Compatibility

The NServiceBus.Metrics.PerformanceCounters package is fully compatible with endpoints that use NServiceBus package's Performance Counters functionality.
