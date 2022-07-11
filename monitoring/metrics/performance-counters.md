---
title: Windows Performance Counters
summary: Monitoring through the use of Windows performance counters.
reviewed: 2020-11-11
component: PerfCounters
redirects:
 - nservicebus/monitoring-nservicebus-endpoints
 - nservicebus/operations/monitoring-endpoints
 - nservicebus/operations/fail-or-hang-during-performance-counter-setup
 - nservicebus/operations/performance-counters
related:
 - nservicebus/operations
 - nservicebus/recoverability
 - nservicebus/operations/auditing
 - nservicebus/operations/management-using-powershell
 - samples/performance-counters
---

When a system is broken down into multiple processes - each with its own queue - it allows identifying which process is the bottleneck by examining how many messages (on average) are in each queue. However, it is not possible to know how long messages are waiting in each queue - which is the primary indicator of a bottleneck - without knowing the rate of messages coming into each queue, and the rate at which messages are being processed from each queue.

Despite the many performance counters Microsoft provides for MSMQ (including messages in queues, machine-wide incoming and outgoing messages per second, and the total messages in all queues), there is no built-in performance counter for the time it takes a message to get through each queue.

NServiceBus includes several performance counters to address this. They are installed in the `NServiceBus` category.

Since all performance counters in Windows are exposed via Windows Management Instrumentation (WMI), it is very straightforward to pull this information into the existing monitoring infrastructure.

While the standard performance counters in the NServiceBus 6 package are marked obsolete, for compatibility reasons they are still enabled by default.

## Counters

All counters are defined in the NuGet package dependency [NServiceBus.Metrics](https://www.nuget.org/packages/NServiceBus.Metrics/). The dependency is automatically pulled in.

For more information about the metrics defined consult the [Metrics](.) documentation page.


### Configuration

The counters can be enabled using the the following code:

snippet: enable-criticaltime

In the NServiceBus host the counters are enabled by default.

Setting up an SLA value can be done using the following code:

snippet: enable-sla


partial: installing

NOTE: [Send-only endpoints](/nservicebus/hosting/#self-hosting-send-only-hosting) are currently not supported since they don't receive messages.
