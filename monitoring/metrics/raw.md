---
title: Consume raw data from Metrics
reviewed: 2019-09-16
component: Metrics
related:
 - nservicebus/operations
related:
 - samples/logging/metrics
 - samples/logging/application-insights
 - samples/logging/prometheus-grafana
 - samples/logging/new-relic
redirects:
 - nservicebus/operations/metrics/raw
---

When [Performance Counters](./performance-counters.md) reporting and [Service Control](./install-plugin.md) reporting is not enough, it's possible to consume raw metrics data by directly attaching to the public API provided by the package. First, the Metrics themselves need to be enabled. Then, a custom reporter can be attached to send data to any collector e.g. Service Control, Azure Application Insights, etc.

## Enabling NServiceBus.Metrics

snippet: Metrics-Enable

partial: reporting
