---
title: Configuring endpoints for monitoring
summary: Steps to configure endpoints to be monitored by ServicePulse
reviewed: 2026-08-26
component: ServicePulse
---

**ServicePulse monitors NServiceBus endpoints for:**

 1. Endpoint availability
     - To enable, configure the [NServiceBus.Heartbeat](/monitoring/heartbeats/install-plugin.md) package
 1. Custom checks (defined and developed according to application needs)
     - To enable, configure the [NServiceBus.CustomChecks](/monitoring/custom-checks/install-plugin.md) package
 1. Failed messages (by monitoring the [error queue](/nservicebus/recoverability/configure-error-handling.md#configure-the-error-queue-address) defined for the endpoints)
 1. Audited messages. Having auditing enabled for all endpoints allows for seeing the entire conversation associated with any message included in that conversation
     - To enable, configure auditing for all monitored endpoints (see [auditing with NServiceBus](/nservicebus/operations/auditing.md)). Forward audit data to a single audit and error queue that is monitored by a ServiceControl instance.
 1. Performance metrics
     - To enable, configure the [NServiceBus.Metrics.ServiceControl](/monitoring/metrics/install-plugin.md) package

![ServicePulse dashboard](images/dashboard.png 'width=500')

Alternatively, these steps can all be managed with the [NServiceBus.ServicePlatform.Connector](/platform/connecting.md) package.
