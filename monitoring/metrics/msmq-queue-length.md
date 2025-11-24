---
title: Setup Queue Length Metrics Reporting for the MSMQ Transport
summary: Setup MSMQ queue length metrics reporting for NServiceBus endpoints and forward data to ServiceControl for improved monitoring
reviewed: 2025-01-30
component: MetricsServiceControl
related:
  - samples/logging/metrics  
---

The `NServiceBus.Metrics.ServiceControl.Msmq` component monitors endpoints' queue length and passes that data to `NServiceBus.Metrics.ServiceControl` which in turn sends it to an instance of the `ServiceControl.Monitoring` service.

```mermaid
graph LR

subgraph Endpoint
  Metrics[NServiceBus<br>Metrics<br>ServiceControl<br>Msmq] -- Queue Length Data --> MetricsSC[Metrics<br>ServiceControl<br>Plugin]
end

MetricsSC -- Metric Data --> Monitoring
Monitoring[ServiceControl<br>Monitoring<br>Instance]
```

## Configuration

No configuration is required. Reporting is configured automatically.
