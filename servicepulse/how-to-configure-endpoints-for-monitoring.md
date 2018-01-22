---
title: Configuring endpoints for monitoring
summary: Steps to configure endpoints to be monitored by ServicePulse
reviewed: 2016-09-02
component: ServicePulse
related:
- servicecontrol/plugins
---

**ServicePulse monitors NServiceBus endpoints for:**

 1. Endpoint availability (using heartbeat signals sent from the endpoint)
 1. Failed messages (by monitoring the error queue defined for the endpoints)
 1. Custom checks (defined and developed per application needs)
 1. And more (see [An Introduction to ServicePulse for NServiceBus](https://particular.net/blog/an-introduction-to-servicepulse-for-nservicebus) for additional upcoming monitoring features)

![ServicePulse dashboard](images/dashboard.png 'width=500')

**Prerequisites for ServicePulse monitoring of endpoints:**

1. [NServiceBus.Heartbeat](/montoring/heartbeats/install-plugin.md) and/or [NServiceBus.CustomChecks](/monitoring/custom-checks/install-plugin.md) packages must be configured.
1. Auditing must be enabled for all monitored endpoints (see [Auditing With NServiceBus](/nservicebus/operations/auditing.md)).
1. All endpoints must forward audited data to a single audit and error queue that is monitored by a ServiceControl instance.
