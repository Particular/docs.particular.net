---
title: Metrics
summary: Measuring the performance and health of an endpoint.
reviewed: 2017-03-24
component: Metrics
related:
 - samples/metrics
---

Metrics collects information to measure endpoint health and performance.


## Enabling Metrics

snippet: Metrics-Enable


## Reporting Metrics data

Metrics can be reported to a number of different locations. Each location is updated on a separate interval. If an interval is not specified then the default interval is used. The default interval is 30 seconds but it can adjusted:

snippet: Metrics-DefaultInterval

### To NServiceBus Log

Metrics data can be written to the [NServiceBus Log](/nservicebus/logging/).

snippet: Metrics-Log

NOTE: Metrics will be written to the log at the Info log level.

### To trace log

Metrics data can be written to [System.Diagnostics.Trace](https://msdn.microsoft.com/en-us/library/system.diagnostics.trace.aspx).

snippet: Metrics-Tracing

### To ServiceControl

Metrics data can be sent to ServiceControl.

snippet: Metrics-ServiceControl

### To Windows Performance Counters

Some of the data captured by the Metrics component can be forwarded to Windows Performance Counters. See [Performance Counters](./performance-counters.md) for more information.


## Metrics Captured

The Metrics component captures a number of different metrics about a running endpoint.

### Processing Time

Processing Time is time it takes for an endpoint to process a single message.