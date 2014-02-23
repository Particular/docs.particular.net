---
title: HOWTO Configure endpoints for monitoring by ServicePulse
summary: Steps to configure endpoints to be monitored by ServicePulse
tags:
- ServicePulse
- HowTo
- Endpoint Configuration
---

#### HOWTO: Configure endpoints for monitoring by ServicePulse

**ServicePulse monitors NServiceBus endpoints for:**

1. Endpoint availability (using heartbeat signals sent from the endpoint);
* Failed Messages (by monitoring the error queue defined for the endpoints);
* Custom Checks (defined and developed per application needs);
* And more (see [An Introduction to ServicePulse for NServiceBus](http://particular.net/blog/an-introduction-to-servicepulse-for-nservicebus) for additional upcoming monitoring features;

![ServicePulse dashboard](../images/ServicePulse/dashboard.png)

**Prerequisites for ServicePulse monitoring of endpoints:**

1. An Endpoint Plugin dll must be deployed in the binaries directory of each NServiceBus endpoint (required for endpoint availability and Custom Checks monitoring);
* Endpoints must use NServiceBus version 4.0.0 or higher (support for earlier releases will be added in a future release);
* Auditing must be enabled for all monitored endpoints (see [Auditing With NServiceBus](http://particular.net/articles/auditing-with-nservicebus));
* All endpoints must forward audited data to a single audit and error queue, that is monitored by a ServiceControl instance;

**Deploying Endpoint Plugins in each Endpoint**

1. The Endpoint Plugin consists of two Nuget Packages:
   * ```ServiceControl.Plugin.Heartbeat.dll```
   * ```ServiceControl.Plugin.CustomChecks.dll```
* Get the Endpoint Heartbeat and CustomChecks Plugins using the NuGet console: 
      * ```install-package ServiceControl.Plugin.Heartbeat -pre```
      * ```install-package ServiceControl.Plugin.CustomChecks -pre```
* For manual deployment, copy the Endpoint Plugin dll files to each endpoint bin directory (and restart the endpoint to load the plugin);
* By default, the Endpoint Plugin sends a heartbeat indication to ServiceControl every 30 seconds;
   * If a heartbeat indication is not recevied within 30 seconds, ServicePulse raises an event that indicates the issue;
