---
title: Reporting Metrics data to ServiceControl
reviewed: 2017-11-22
component: MetricsServiceControl
related:
 - nservicebus/operations
related:
 - samples/logging/metrics
redirects:
 - nservicebus/operations/metrics-service-control
---


This component enables sending monitoring data gathered with `NServiceBus.Metrics` to an instance of `ServiceControl.Monitoring` service.

## Configuration

After adding package to the project, metrics are sent to ServiceControl once enabled.

It can be enabled via:

snippet: SendMetricDataToServiceControl


### Service Control Metrics Address

The default instance name is `particular.monitoring` which is also used as the input queue for ServiceControl Monitoring.

partial: interval

### Instance Id

An override for `$.diagnostics.hostid` and `$.diagnostics.hostdisplayname`.

This value is shown in the list of instances on the endpoint details page in ServicePulse Monitoring. By default the monitoring plug-in will use the [NServiceBushost host id hash identifier](/nservicebus/hosting/override-hostid.md) to identify the monitored endpoint instance.

Note: Make sure that the `InstanceId` is logically unique and human readable.

A human readable value can be passed like in the following example:

snippet: SendMetricDataToServiceControlHostId


Note: It is **not** required to add a process identification. The `InstanceId` does not require to be physically identifying the running instance uniquely. The plugin uses its own internal unique session  identifier for this.

#### Azure host

The value of `$.diagnostics.hostdisplayname` is expected to be human readable. This might be problematic when using the [Azure Host](/nservicebus/hosting/cloud-services-host/faq.md#host-identifier) as it uses Role Name for the `$.diagnostics.hostdisplayname`, which results in the same display name for all instances.

