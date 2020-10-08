---
title: Send Metrics data to ServiceControl
reviewed: 2019-11-08
component: MetricsServiceControl
related:
  - samples/logging/metrics
redirects:
  - nservicebus/operations/metrics/service-control
---


The `NServiceBus.Metrics.ServiceControl` component enables sending monitoring data gathered with `NServiceBus.Metrics` to a `ServiceControl.Monitoring` service.

## Configuration

After adding the package to the project, metrics are sent to ServiceControl once enabled.

It can be enabled via:

snippet: SendMetricDataToServiceControl

Note: The metrics feature can't be used on send-only endpoints


### Service Control Metrics Address

The default instance name is `particular.monitoring` which is also used as the input queue for ServiceControl monitoring.

partial: interval

partial: ttbr

### Instance ID

An override for `$.diagnostics.hostid` and `$.diagnostics.hostdisplayname`.

It is recommended to [override the *host id* and *host display name* via NServiceBus core](/nservicebus/hosting/override-hostid.md) and to use the API without the `InstanceId` argument. By default, the monitoring plug-in will use these values to identify the monitored endpoint instances in the user-interface.

Note: Make sure that the `InstanceId` value is unique and human readable.

A human readable value is being passed in the following example:

snippet: SendMetricDataToServiceControlHostId

Note: It is **not** required to add a process identification. The `InstanceId` is not required to be physically identifying the running instance uniquely. The plugin uses its own internal unique session identifier for this.

#### Azure host

The value of `$.diagnostics.hostdisplayname` is expected to be human readable. This might be problematic when using the [Azure Host](/nservicebus/hosting/cloud-services-host/faq.md#host-identifier) as it uses Role Name for the `$.diagnostics.hostdisplayname`, which results in the same display name for all instances.
