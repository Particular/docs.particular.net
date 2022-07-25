---
title: Externalize Windows Performance Counters
reviewed: 2020-11-09
component: PerfCounters
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
related: 
 - monitoring/metrics/performance-counters
---

include: externalize-perfcounters

## Changed APIs

The NServiceBus Performance Counter APIs have been marked as obsolete and have one-for-one equivalents in the new `NServiceBus.Metrics.PerformanceCounters` package.

### Enabling Critical Time Counter

```csharp
// For NServiceBus version 6.x
endpointConfiguration.EnableCriticalTimePerformanceCounter();

// For Performance Counters version 1.x
var performanceCounters = endpointConfiguration.EnableWindowsPerformanceCounters();
```

### Enabling SLA Counter

```csharp
// For NServiceBus version 6.x
endpointConfiguration.EnableSLAPerformanceCounter(TimeSpan.FromMinutes(3));

// For Performance Counters version 1.x
var performanceCounters = endpointConfiguration.EnableWindowsPerformanceCounters();
performanceCounters.EnableSLAPerformanceCounters(TimeSpan.FromMinutes(3));
```

## Compatibility

The `NServiceBus.Metrics.PerformanceCounters` package is fully compatible with endpoints that use the NServiceBus package's Performance Counters functionality.
