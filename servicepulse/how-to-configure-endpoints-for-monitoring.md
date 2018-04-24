---
title: Configuring endpoints for monitoring
summary: Steps to configure endpoints to be monitored by ServicePulse
reviewed: 2018-04-24
component: ServicePulse
related:
- servicecontrol/plugins
---

**ServicePulse monitors NServiceBus endpoints for:**

 1. Endpoint availability (using heartbeat signals sent from the endpoint)
 1. Failed messages (by monitoring the error queue defined for the endpoints)
 1. Custom checks (defined and developed according to application needs)

![ServicePulse dashboard](images/dashboard.png 'width=500')

**Prerequisites for monitoring endpoints with ServicePulse:**

1. [NServiceBus.Heartbeat](/monitoring/heartbeats/install-plugin.md) and/or [NServiceBus.CustomChecks](/monitoring/custom-checks/install-plugin.md) packages must be configured.
1. Auditing must be enabled for all monitored endpoints (see [auditing with NServiceBus](/nservicebus/operations/auditing.md)).
1. All endpoints must forward audited data to a single audit and error queue that is monitored by a ServiceControl instance.
