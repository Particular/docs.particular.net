---
title: Externalize Windows Performance Counters
reviewed: 2017-03-17
component: PerfCounters
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
related: 
 - nservicebus/operations/performance-counters
---

The [Windows Performance Counters feature](/nservicebus/operations/performance-counters.md) is being removed from the NServiceBus package. It is now available as a separate NuGet package, [NServiceBus.WindowsPerformanceCounters](https://www.nuget.org/packages/NServiceBus.WindowsPerformanceCounters/). The new package should be used to write to performance counters when using NServiceBus Versions 6.2 and above.

The API was also modified.


## Changed APIs

The NServiceBus Performance Counter APIs have been marked as obsolete and have one-for-one equivalents in the NServiceBus.WindowsPerformanceCounters package.


### Enabling Critical Time Counter

snippet: 6to1-enable-criticaltime


### Enabling SLA Counter

snippet: 6to1-enable-sla


## Compatibility

The NServiceBus.WindowsPerformanceCounters package is fully compatible with endpoints that use NServiceBus package's Performance Counters functionality.
