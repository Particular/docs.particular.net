---
title: Configuring endpoints for monitoring
summary: Steps to configure endpoints to be monitored by ServicePulse
reviewed: 2025-01-30
component: ServicePulse
---

**ServicePulse monitors NServiceBus endpoints for:**

 1. Endpoint availability (using heartbeat signals sent from the endpoint)
 2. Failed messages (by monitoring the error queue defined for the endpoints)
 3. Custom checks (defined and developed according to application needs)
 4. Performance metrics

![ServicePulse dashboard](images/dashboard.png 'width=500')

The following actions are required in order to monitor endpoints in ServicePulse:

1. Configure the [NServiceBus.Heartbeat](/monitoring/heartbeats/install-plugin.md) and/or [NServiceBus.CustomChecks](/monitoring/custom-checks/install-plugin.md) packages.
1. Enable auditing for all monitored endpoints (see [auditing with NServiceBus](/nservicebus/operations/auditing.md)). Forward audit data to a single audit and error queue that is monitored by a ServiceControl instance.
1. Configure the [NServiceBus.Metrics.ServiceControl](/monitoring/metrics/install-plugin.md) package.

These steps can all be managed with the [NServiceBus.ServicePlatform.Connector](/platform/connecting.md) package.