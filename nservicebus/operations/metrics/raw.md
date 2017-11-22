---
title: Consume raw data from Metrics
summary: Consume raw data from Metrics.
reviewed: 2017-11-22
component: Metrics
related:
 - nservicebus/operations
related:
 - samples/logging/metrics
---

When [Performance Counters](./performance-counters.md) reporting and [Service Control](./service-control.md) reporting is not enough, it's possible to consume raw metrics data, directly attaching to the public API provided by the package. First, the Metrics themselves need to be enabled. Then, a custom reporter can be attached to send data to any collector.

## Enabling NServiceBus.Metrics

snippet: Metrics-Enable

partial: reporting
