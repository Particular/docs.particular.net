---
title: Queue Length Metric Troubleshooting
summary: Validating queue length metrics monitoring setup.
reviewed: 2018-05-14
related:
  - monitoring/metrics/definitions
---

If queue length metric is not being reported in `ServicePulse` the following validation checks might help finding the root cause of the problem:

* Check that other metrics, such as Processing Time, are being reported. Otherwise follow setup steps described in performance monitoring setup [tutorial](/tutorials/monitoring-setup/).
* When running the MSMQ transport, check if endpoints being monitored have the MSMQ queue length [reporter](/monitoring/metrics/msmq-queue-length.md) installed.
* Check that endpoints being monitored are using service control monitoring [plugin](/monitoring/metrics/install-plugin.md) compatible with the monitoring instance. 

NOTE: `NServiceBus.Metrics.ServiceControl` versions 1.0.0-1.2.1 and 2.0.0-2.0.1 were using an **experimental queue length metrics feature** that has been removed in the newer versions. `ServiceControl` monitoring instance does not support the estimated queue length metrics starting from version 1.48.0. Both the plugins and the monitoring instance have to be running compatible versions in order for the queue length metric to be reported. 
