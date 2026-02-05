---
title: Send Metrics data to ServiceControl
summary: Install the Metrics plugin to send NServiceBus monitoring data to ServiceControl for centralized performance tracking
reviewed: 2024-11-07
component: MetricsServiceControl
related:
  - samples/logging/metrics
redirects:
  - nservicebus/operations/metrics/service-control
---

The `NServiceBus.Metrics.ServiceControl` component enables sending monitoring data gathered with `NServiceBus.Metrics` to a `ServiceControl.Monitoring` service.

> [!NOTE]
> The metrics feature can't be used on send-only endpoints

> [!NOTE]
> When using MSMQ, the additional `NServiceBus.Metrics.ServiceControl.Msmq` package must also be installed.

## Configuration

The package allows collection and propagation of metrics to ServiceControl.

It can be enabled via:

snippet: SendMetricDataToServiceControl

### Service Control Metrics Address

The default instance name is `particular.monitoring` which is also used as the input queue for ServiceControl monitoring.

partial: interval

partial: ttbr

### Instance ID

An override for `$.diagnostics.hostid` and `$.diagnostics.hostdisplayname`.

It is recommended to [override the *host id* and *host display name* via NServiceBus core](/nservicebus/hosting/override-hostid.md) and to use the API without the `InstanceId` argument. By default, the monitoring plug-in will use these values to identify the monitored endpoint instances in the user-interface.

> [!NOTE]
> Make sure that the `InstanceId` value is unique and human readable.

A human-readable value is passed in the following example:

snippet: SendMetricDataToServiceControlHostId

> [!NOTE]
> It is **not** required to add a process identification. The `InstanceId` is not required to be physically identifying the running instance uniquely. The plugin uses its own internal unique session identifier for this.
