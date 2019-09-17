---
title: How Queue Length metric is working
summary: How queue length metric is working
reviewed: 2019-09-16
related:
  - monitoring/metrics/definitions
  - monitoring/metrics
  - servicecontrol/monitoring-instances
---

The queue length metric is calculated differently depending on whether the transport uses a centralized broker or is federated.
Both approaches are described [here](https://github.com/Particular/ServiceControl.Monitoring/blob/master/docs/queue-length.md).