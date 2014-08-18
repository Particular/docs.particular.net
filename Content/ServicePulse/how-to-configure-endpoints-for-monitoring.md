---
title: How to configure endpoints for monitoring by ServicePulse
summary: Steps to configure endpoints to be monitored by ServicePulse
tags:
- ServicePulse
- HowTo
- Endpoint Configuration
---

**ServicePulse monitors NServiceBus endpoints for:**

1. Endpoint availability (using heartbeat signals sent from the endpoint)
1. Failed messages (by monitoring the error queue defined for the endpoints)
1. Custom checks (defined and developed per application needs)
1. And more (see [An Introduction to ServicePulse for NServiceBus](http://particular.net/blog/an-introduction-to-servicepulse-for-nservicebus) for additional upcoming monitoring features)

![ServicePulse dashboard](../images/ServicePulse/dashboard.png)

**Prerequisites for ServicePulse monitoring of endpoints:**

1. An endpoint plugin DLL must be deployed in the binaries directory of each NServiceBus endpoint (required for endpoint availability and custom checks monitoring).
1. Supported NServiceBus Endpoints:
    * NServiceBus V4.0.0 or higher;
    * NServiceBus V3.0.4 to V3.3.8;
1. Auditing must be enabled for all monitored endpoints (see [Auditing With NServiceBus](/NServiceBus/auditing-with-nservicebus)).
1. All endpoints must forward audited data to a single audit and error queue that is monitored by a ServiceControl instance.

**Deploying Endpoint Plugins in each Endpoint**

1. The endpoint plugin consists of two NuGet packages:
    * NServiceBus V4.x: 
        * [`ServiceControl.Plugin.Heartbeat`](https://www.nuget.org/packages/ServiceControl.Plugin.Heartbeat/)
        * [`ServiceControl.Plugin.CustomChecks`](https://www.nuget.org/packages/ServiceControl.Plugin.CustomChecks/)
    * NServiceBus V3.0.4 to V3.3.8: 
        * [`ServiceControl.Plugin.Nsb3.Heartbeat`](https://www.myget.org/feed/particular-prerelease/package/ServiceControl.Plugin.Nsb3.Heartbeat)
        * [`ServiceControl.Plugin.Nsb3.CustomChecks`](https://www.myget.org/feed/particular-prerelease/package/ServiceControl.Plugin.Nsb3.CustomChecks)

1. Get the Endpoint Heartbeat and CustomChecks plugins using the NuGet console: 
     * `install-package ServiceControl.Plugin.Heartbeat`
     * `install-package ServiceControl.Plugin.CustomChecks`
     * or use the appropriate V3 package if your endpoint still targets NServiceBus V3:
	     * `install-package ServiceControl.Plugin.Nsb3.Heartbeat`
	     * `install-package ServiceControl.Plugin.Nsb3.CustomChecks`
1. For manual deployment, copy the endpoint plugin DLL files to each endpoint bin directory (and restart the endpoint to load the plugin).
1. By default, the endpoint plugin sends a heartbeat indication to ServiceControl every 30 seconds. If a heartbeat indication is not received within 30 seconds, ServicePulse raises an event that indicates the issue.

**NOTE**: NServiceBus V3 Endpoint Plugins are a pre-release version hosted on the Particular MyGet public feed. To install the pre-release packages add the `https://www.myget.org/F/particular-prerelease/` package source to Visual Studio or issue the following command using the Package Manager Console:

    Install-Package DiscoverPackageSources
    Discover-PackageSources -Url "https://www.myget.org/F/particular-prerelease/"

#### Related articles

* [ServiceControl Endpoint Plugins](/ServiceControl/Plugins)
