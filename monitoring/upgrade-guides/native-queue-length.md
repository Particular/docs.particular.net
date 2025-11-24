---
title: Upgrade from Estimated to Native Queue Length Metric
summary: Upgrade NServiceBus endpoints and ServiceControl from the estimated to the native queue length metric for improved monitoring accuracy
isUpgradeGuide: true
reviewed: 2022-10-31
---

The experimental queue length metrics feature based on an estimation algorithm has been replaced by the native queue length metric. This document describes the migration path for systems using the old metrics.

### Upgrading the monitoring instance

ServiceControl should be upgraded to version 1.48.0+. Upgrading endpoints first will prevent old monitoring instance versions from processing new, unsupported metric reports. Also, new monitoring instances produce `WARN` log entries when receiving legacy queue length reports:

> Legacy queue length report received from {InstanceName} instance of {EndpointName}.

That enables identification of the endpoints running `NServiceBus.Metrics.ServiceControl` packages that do not support the native queue length metric.

### Upgrading ServicePulse

ServicePulse should be upgraded to the latest version. This step ensures compatibility with the updated version of the monitoring instance and ServicePulse.

### Upgrading endpoints

All endpoints that send monitoring data must be upgraded to the latest version of `NServiceBus.Metrics.ServiceControl`. The exact version depends on the `NServiceBus` endpoint's version. The table below shows the version ranges.

|NServiceBus|NServiceBus.Metrics.ServiceControl|
|--|--|
| 5.*       | [1.3.0, 2.0)|
| 6.*       | [2.1.0, 3.0)|
| 7.*       | [3.0,)|

> [!WARNING]
> When running the MSMQ transport, it is necessary to install the latest [`NServiceBus.Metrics.ServiceControl.Msmq`](/monitoring/metrics/msmq-queue-length.md) package.
