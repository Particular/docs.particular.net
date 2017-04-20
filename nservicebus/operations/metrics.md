---
title: Metrics
summary: Measuring the performance and health of an endpoint.
reviewed: 2017-04-04
component: Metrics
related:
 - samples/metrics
---

Metrics collects information to measure endpoint health and performance. When a system is broken down into multiple processes, each with its own queue, it allows identifying which process is the bottleneck by examining how many messages (on average) are in each queue. The only issue is that without knowing the rate of messages coming into each queue, and the rate at which messages are being processed from each queue, it is not possible to know how long messages are waiting in each queue, which is the primary indicator of a bottleneck.

## Enabling NServiceBus.Metrics

snippet: Metrics-Enable


## Reporting metrics data

Metrics can be reported to a number of different locations. Each location is updated on a separate interval. 

### To NServiceBus log

Metrics data can be written to the [NServiceBus Log](/nservicebus/logging/).

snippet: Metrics-Log

NOTE: By default metrics will be written to the log at the `DEBUG` log level. The API allows this parameter to be customized.

snippet: Metrics-Log-Info

### To trace log

Metrics data can be written to [System.Diagnostics.Trace](https://msdn.microsoft.com/en-us/library/system.diagnostics.trace.aspx).

snippet: Metrics-Tracing

### To custom function

Metrics data can be consumed by a custom function.

snippet: Metrics-Custom-Function

### To Windows Performance Counters

Some of the data captured by the NServiceBus.Metrics component can be forwarded to Windows Performance Counters. See [Performance Counters](./performance-counters.md) for more information.


## Metrics captured

The NServiceBus.Metrics component captures a number of different metrics about a running endpoint.

### Processing time

Processing time is the time it takes for an endpoint to process a single message.

### Critical time

Critical time is the time between when a message is sent and when it is fully processed. It is a combination of:
- Network send time: The time a message spends on the network before arriving in the destination queue
- Queue wait time: The time a message spends in the destination queue before being picked up and processed
- Processing time: The time it takes for the destination endpoint to process the message

### Messages received performance statistics

These statistics encompass a number of different metrics, including:

- Number of messages pulled from queue
- Number of message processing failures
- Number of messages successfully processed


