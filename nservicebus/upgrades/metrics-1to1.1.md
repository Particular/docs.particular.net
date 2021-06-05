---
title: Metrics Upgrade Version 1.0 to 1.1
reviewed: 2021-06-05
component: metrics
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
---


include: metrics-registerobservers


### EnableMetricTracing

Replace with explicit calls to [Trace.WriteLine](https://msdn.microsoft.com/en-us/library/system.diagnostics.trace.writeline.aspx).

snippet: 1to11EnableToTrace


### EnableLogTracing

Replace with explicit calls to an [NServiceBus logger](/nservicebus/logging/usage.md).

snippet: 1to11EnableToLog


### EnableCustomReport

Replace with explicit calls to the custom method.

snippet: 1to11Custom
