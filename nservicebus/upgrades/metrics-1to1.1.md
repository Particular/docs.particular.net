---
title: Metrics Upgrade Version 1.0 to 1.1
reviewed: 2017-09-05
component: metrics
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
---


## RegisterObservers replaces basic methods

The `EnableMetricTracing`, `EnableCustomReport` and `EnableLogTracing` methods have been deprecated and replaced with the more extensible [`RegisterObservers`](/nservicebus/operations/metrics.md#reporting-metrics-data-to-any-external-storage).

Also note that the older methods supported an `interval` TimeSpan. In the new API batching up metrics, and flushing them at a given interval, is the domain of the calling code.


### EnableMetricTracing

Replace with explicit calls to [Trace.WriteLine](https://msdn.microsoft.com/en-us/library/system.diagnostics.trace.writeline.aspx).

snippet: 1to11EnableToTrace


### EnableLogTracing

Replace with explicit calls to an [NServiceBus logger](/nservicebus/logging/usage.md).

snippet: 1to11EnableToLog


### EnableCustomReport

Replace with explicit calls to the custom method.

snippet: 1to11Custom
