---
title: Windows Performance Counters
summary: Monitoring through the use of Windows performance counters.
reviewed: 2025-10-22
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
 - transports/msmq/management-using-powershell
 - samples/performance-counters
---

> [!NOTE]
> The NServiceBus.Metrics.PerformanceCounters package is only available for Windows. Newer projects may benefit more from monitoring via [OpenTelemetry](/nservicebus/operations/opentelemetry.md).

The Windows Performance Counters package provides additional performance counters to better understand how long it takes for messages to flow through queues, not just the number of messages in the queue.

Since all performance counters in Windows are exposed via Windows Management Instrumentation (WMI), it is very straightforward to pull this information into the existing monitoring infrastructure.

partial: counters

partial: installing

> [!NOTE]
> [Send-only endpoints](/nservicebus/hosting/#self-hosting-send-only-hosting) are currently not supported since they don't receive messages.
