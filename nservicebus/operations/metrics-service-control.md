---
title: Metrics data to ServiceControl
summary: Reporting metrics to ServiceControl.
reviewed: 2017-09-26
component: MetricsServiceControl
related:
 - nservicebus/operations
related:
 - samples/logging/metrics
---


This component enables sending monitoring data to ServiceControl.Monitoring instance.


## Enabling

It can be enabled via:

snippet: SendMetricDataToServiceControl


#### Service Control Metrics Address:

The default is "particular.monitoring" which is ServiceControl monitoring instance input queue.


#### Instance Id:

An override for `$.diagnostics.hostid` and `$.diagnostics.hostdisplayname`.

By default the monitoring plug-in will use [host identifiers](/nservicebus/hosting/override-hostid.md) to identify the monitored endpoint instance.

The value of `$.diagnostics.hostdisplayname` is expected to be human readable. This might be problematic when using the [Azure Host](/nservicebus/hosting/cloud-services-host/faq.md#host-identifier) as it uses Role Name for the `$.diagnostics.hostdisplayname`, which results in the same display name for all instances.

Note: The `InstanceId` needs to be both unique a d human readable