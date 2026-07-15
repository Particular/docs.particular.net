---
title: Send Metrics data to ServiceControl
summary: Install the Metrics plugin to send NServiceBus monitoring data to ServiceControl for centralized performance tracking
reviewed: 2026-07-15
component: MetricsServiceControl
related:
  - samples/logging/metrics
redirects:
  - nservicebus/operations/metrics/service-control
---

The `NServiceBus.Metrics.ServiceControl` component enables sending monitoring data gathered with `NServiceBus.Metrics` to a `ServiceControl.Monitoring` service.

> [!NOTE]
> This plugin can be enabled and configured with the [ServicePlatform Connector plugin](/platform/connecting.md).

> [!NOTE]
> The metrics feature can't be used on send-only endpoints

> [!NOTE]
> When using the MSMQ transport, the additional [`NServiceBus.Metrics.ServiceControl.Msmq` package](/monitoring/metrics/msmq-queue-length.md) is required to report queue length, which ServiceControl cannot measure for MSMQ itself. All other metrics are reported without it.

## Configuration

To install the plugin in an endpoint, reference the [NServiceBus.Metrics.ServiceControl NuGet package](https://www.nuget.org/packages/NServiceBus.Metrics.ServiceControl/), which allows collection and propagation of metrics to ServiceControl.

It can be enabled via:

snippet: SendMetricDataToServiceControl

### Service Control Metrics Address

The default instance name is `particular.monitoring` which is also used as the input queue for ServiceControl monitoring.

partial: interval

partial: ttbr

### Instance ID

Overrides the value ServiceControl monitoring uses to identify this endpoint instance, in place of `$.diagnostics.hostid` and `$.diagnostics.hostdisplayname`.

It is recommended to [override the *host id* and *host display name* via NServiceBus core](/nservicebus/hosting/override-hostid.md) and to use the API without the `InstanceId` argument. By default, the monitoring plugin will use these values to identify the monitored endpoint instances in the user interface.

> [!NOTE]
> Make sure that the `InstanceId` value is unique, human-readable, and stable between restarts. ServiceControl monitoring identifies instances by this value, so a value that changes on restart appears as a new instance.

A human-readable value is passed in the following example:

snippet: SendMetricDataToServiceControlHostId
