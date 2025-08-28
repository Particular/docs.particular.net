---
title: Consume raw data from Metrics
summary: Use the public API in the Metrics package to consume raw metrics data
reviewed: 2025-08-28
component: Metrics
related:
 - nservicebus/operations
 - nservicebus/operations/opentelemetry
 - samples/logging/metrics
 - samples/logging/new-relic
 - samples/logging/datadog
redirects:
 - nservicebus/operations/metrics/raw
---

When [Performance Counters](./performance-counters.md) reporting and [ServiceControl](./install-plugin.md) reporting are not enough, it's possible to consume raw metrics data by directly attaching them to the public API provided by the package. First, the Metrics themselves need to be enabled. Then, a custom reporter can be attached to send data to any collector e.g. Service Control, Azure Application Insights, etc.

## Enabling NServiceBus.Metrics

snippet: Metrics-Enable

partial: reporting
