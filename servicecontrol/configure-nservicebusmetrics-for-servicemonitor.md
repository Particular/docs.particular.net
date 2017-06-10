---
title: Configuring NServiceBus.Metrics for ServiceControl Monitor
summary: How to configure NServiceBus.Metrics to report data to ServiceControl Monitor
reviewed: 2017-06-09
---

In order for ServiceControl Monitor to receive metrics data endpoints must be configured to use [NServiceBus.Metrics](/nservicebus/operations/metrics.md) and to send that data to the ServiceControl Monitor input queue.

## Enabling NServiceBus.Metrics

snippet: Metrics-Enable

## Reporting to ServiceMonitor

To configure an endpoint to report metrics data to ServiceMonitor set the ServiceControl Monitor input queue:

```c#
//Convert this to a snippet
metrics.SendMetricDataToServiceControl("Particular.ServiceMonitor", TimeSpan.FromSeconds(1));
```
