---
title: Configuring NServiceBus.Metrics for ServiceMonitor
summary: How to configure NServiceBus.Metrics to report data to ServiceMonitor
reviewed: 2017-06-09
---

In order for ServiceMonitor to receive metrics data endpoints must be configured to use [NServiceBus.Metrics](/nservicebus/operations/metrics.md) and to send that data to the ServiceMonitor input queue.

## Enabling NServiceBus.Metrics

snippet: Metrics-Enable

## Reporting to ServiceMonitor

To configure your endpoint to report metrics data to ServiceMonitor configure the endpoint with the ServiceMonitor input queue:

```c#
//Convert this to a snippet
metrics.SendMetricDataToServiceControl("Particular.ServiceMonitor", TimeSpan.FromSeconds(1));
```
