---
title: Setup Queue Length Metrics Reproting for the MSMQ Transport
reviewed: 2018-05-11
component: MetricsServiceControl
related:
  - samples/logging/metrics  
---

The component `NServiceBus.Metrics.ServiceControl.Msmq` monitors endpoints' queue length and passes that data to `NServiceBus.Metrics.ServiceControl` which in turn sends it to an instance of `ServiceControl.Monitoring` service.

```mermaid
graph LR

subgraph Endpoint
  Metrics[NServiceBus<br>Metrics<br>ServiceControl<br>Msmq] -- Queue Length Data --> MetricsSC[Metrics<br>ServiceControl<br>Plugin]
end

MetricsSC -- Metric Data --> Monitoring
Monitoring[ServiceControl<br>Monitoring<br>Instance]
```

## Configuration

No configuration is required. The reporting will be configured automatically.
