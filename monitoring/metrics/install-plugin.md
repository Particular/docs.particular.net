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

The `NServiceBus.Metrics.ServiceControl` component enables sending monitoring data collected  by `NServiceBus.Metrics` to a `ServiceControl.Monitoring` instance.

> [!NOTE]
> This plugin can be enabled and configured with the [ServicePlatform Connector plugin](/platform/connecting.md).

> [!NOTE]
> The metrics feature can't be used on send-only endpoints

> [!NOTE]
> For endpoints using the MSMQ transport, an additional [`NServiceBus.Metrics.ServiceControl.Msmq`](/monitoring/metrics/msmq-queue-length.md)  package is required to report queue length, as ServiceControl cannot determine MSMQ queue length on its own. All other metrics are reported without this package.

## Configuration

To install the plugin in an endpoint, reference the [NServiceBus.Metrics.ServiceControl  package](https://www.nuget.org/packages/NServiceBus.Metrics.ServiceControl/), which allows collection and propagation of metrics to ServiceControl.

It can be enabled via:

snippet: SendMetricDataToServiceControl

### Service Control Metrics Address

The default instance name is `particular.monitoring` which is also used as the input queue for ServiceControl monitoring.

partial: interval

partial: ttbr

### Instance ID

The `InstanceId` parameter overrides the value ServiceControl monitoring uses to identify the endpoint instance, in place of `$.diagnostics.hostid` and `$.diagnostics.hostdisplayname`.

It is recommended to [override the *host id* and *host display name* via NServiceBus core](/nservicebus/hosting/override-hostid.md) and to use the API without the `InstanceId` argument. By default, the monitoring plugin will use these values to identify the monitored endpoint instances in the user interface.

> [!NOTE]
> If an explicit `InstanceId` is provided ,ensure that the `InstanceId` value is unique, human-readable, and stable between restarts. As ServiceControl monitoring identifies instances by this value, a value that changes on restart appears will appear as a new instance.

A human-readable value is passed in the following example:

snippet: SendMetricDataToServiceControlHostId
