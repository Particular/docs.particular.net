---
title: Metrics
summary: 
reviewed: 2018-01-12
component: Metrics
versions: 'Metrics:*'
---

The Metrics plugin collects metric data about the performance of running endpoints. This data can be forwarded to a ServiceControl Monitoring instance and then viewed in ServicePulse.

To see performance monitoring in action, try our [standalone demo](/tutorials/monitoring-demo/).

For a full list of the performance metrics captured and their formal definitions, see [Metric definitions](definitions.md).

```mermaid
graph LR
	
subgraph Endpoint
  Metrics[Metrics<br>Plugin] -- Metric Data --> MetricsSC[Metrics<br>ServiceControl<br>Plugin] 
end

MetricsSC -- Metric Data --> MQ

MQ[Metrics Queue] -- Metric Data --> Monitoring[ServiceControl<br>Monitoring<br>Instance]

Monitoring -- Endpoint<br>performance<br>data --> ServicePulse
```


## Set up Metrics

To enable collecting metrics in an environment:

1. [Install a ServiceControl Monitoring instance](/servicecontrol/monitoring-instances/)
2. [Install and configure the ServiceControl Metrics plugin in each endpoint](install-plugin.md)
3. [View the performance data collected for endpoints in ServicePulse](in-servicepulse.md)

NOTE: [Monitoring NServiceBus solutions: Getting started](/tutorials/monitoring-setup/) is an in-depth, step-by-step tutorial about installing and configure everything to get the most out of performance monitoring.


## Reporting metric data to other places

- Metrics can be reported to [Windows Performance Counters](performance-counters.md).
- [Observing raw metric data allows reporting to any 3rd party metric database](raw.md).