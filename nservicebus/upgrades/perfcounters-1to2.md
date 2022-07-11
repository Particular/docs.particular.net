---
title: Upgrade PerformanceCounters Version 1 to 2
summary: Instructions on how to upgrade PerformanceCounters Version 1 to 2.
component: PerfCounters
reviewed: 2021-07-23
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---


## UpdateCounterEvery

PerformanceCounters are now updated directly, with no intermediate stores. Therefore, the reporting period is no longer needed and the call to `UpdateCounterEvery` can be removed.

```csharp
var performanceCounters = endpointConfiguration.EnableWindowsPerformanceCounters();
// the below line can be removed
performanceCounters.UpdateCounterEvery(TimeSpan.FromSeconds(2));
```
