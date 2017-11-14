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

1. An endpoint plugin DLL must be deployed in the binaries directory of each NServiceBus endpoint (required for endpoint availability and custom checks monitoring).
1. Supported NServiceBus Endpoints:
    * NServiceBus Version 6.0.0 or higher;
    * NServiceBus Version 5.0.0 or higher;    
    * NServiceBus Version 4.0.0 or higher;
    * NServiceBus Version 3.0.4 or higher;
1. Auditing must be enabled for all monitored endpoints (see [Auditing With NServiceBus](/nservicebus/operations/auditing.md)).
1. All endpoints must forward audited data to a single audit and error queue that is monitored by a ServiceControl instance.

**Deploying Endpoint Plugins in each Endpoint**

1. The endpoint plugin consists of these NuGet packages:
    * NServiceBus Version 6.x:
        * [`ServiceControl.Plugin.Nsb6.Heartbeat`](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb6.Heartbeat/)
        * [`ServiceControl.Plugin.Nsb6.CustomChecks`](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb6.CustomChecks/)
        * [`ServiceControl.Plugin.Nsb6.SagaAudit`](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb6.SagaAudit/)
    * NServiceBus Version 5.x:
        * [`ServiceControl.Plugin.Nsb5.Heartbeat`](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb5.Heartbeat/)
        * [`ServiceControl.Plugin.Nsb5.CustomChecks`](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb5.CustomChecks/)
        * [`ServiceControl.Plugin.Nsb5.SagaAudit`](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb5.SagaAudit/)
    * NServiceBus Version 4.x:
        * [`ServiceControl.Plugin.Nsb4.Heartbeat`](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb4.Heartbeat/)
        * [`ServiceControl.Plugin.Nsb4.CustomChecks`](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb4.CustomChecks/)
        * [`ServiceControl.Plugin.Nsb4.SagaAudit`](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb4.SagaAudit/)
    * NServiceBus Version 3.0.4 or higher:
        * [`ServiceControl.Plugin.Nsb3.Heartbeat`](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb3.Heartbeat/)
        * [`ServiceControl.Plugin.Nsb3.CustomChecks`](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb3.CustomChecks/)

1. Install the plugins from NuGet in the endpoints:
     * `install-package ServiceControl.Plugin.Nsb6.Heartbeat`
     * `install-package ServiceControl.Plugin.Nsb6.CustomChecks`
  
     If saga visualization in ServiceInsight,
     * `install-package ServiceControl.Plugin.Nsb6.SagaAudit`

     * or use the appropriate Version 4 package if the endpoint targets NServiceBus Version 5:
	     * `install-package ServiceControl.Plugin.Nsb5.Heartbeat`
	     * `install-package ServiceControl.Plugin.Nsb5.CustomChecks`
             * `install-package ServiceControl.Plugin.Nsb5.SagaAudit`

     * or use the appropriate Version 4 package if the endpoint targets NServiceBus Version 4:
	     * `install-package ServiceControl.Plugin.Nsb4.Heartbeat`
	     * `install-package ServiceControl.Plugin.Nsb4.CustomChecks`
     	     * `install-package ServiceControl.Plugin.Nsb4.SagaAudit`

     * or use the appropriate Version 3 package if the endpoint targets NServiceBus Version 3:
	     * `install-package ServiceControl.Plugin.Nsb3.Heartbeat`
	     * `install-package ServiceControl.Plugin.Nsb3.CustomChecks`

**NOTE**: Saga Visualization plugin is only available from Version 4 and above.
	   
 1. For manual deployment, copy the endpoint plugin DLL files to each endpoint bin directory (and restart the endpoint to load the plugin).
 1. By default, the endpoint plugin sends a heartbeat indication to ServiceControl every 30 seconds. If a heartbeat indication is not received within 30 seconds, ServicePulse raises an event that indicates the issue.
