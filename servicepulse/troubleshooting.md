---
title: Troubleshooting
summary: ServicePulse installation and common issues troubleshooting
reviewed: 2020-01-28
component: ServicePulse
---


### ServicePulse is unable to connect to ServiceControl

 * See the [ServiceControl release notes](https://github.com/Particular/ServiceControl/releases/) troubleshooting section for guidance on detecting ServiceControl HTTP API accessibility.
 * Verify that ServicePulse is trying to access the correct ServiceControl URI (based on ServiceControl instance URI defined in ServicePulse installation settings).
 * Check that ServicePulse is not blocked from accessing the ServiceControl URI by firewall settings.


### ServicePulse reports that 0 endpoints are active, although endpoint plugins were deployed

 * Follow the guidance in [How to configure endpoints for monitoring by ServicePulse](how-to-configure-endpoints-for-monitoring.md).
 * Restart the endpoint after copying the endpoint plugin files into the endpoint's `bin` directory.
 * Ensure [auditing](/nservicebus/operations/auditing.md) is enabled for the endpoint, and the audited messages are forwarded to the correct audit and error queues monitored by ServiceControl.
 * Ensure relevant ServiceControl assemblies are not in the list of assemblies to exclude from scanning. For more details refer to [Assembly scanning](/nservicebus/hosting/assembly-scanning.md).
 * Ensure the endpoint references NServiceBus version 4.0.0 or later.


### ServicePulse reports empty failed message groups

RavenDB index could be disabled. This typically happens when disk space runs out. To fix this:

 1. Put ServiceControl in [maintenance mode](/servicecontrol/maintenance-mode.md).
 1. Open the [Raven Studio browser](http://localhost:33334/studio/index.html#databases/documents?&database=%3Csystem%3E)
 1. Navigate to the Indexes tab
 1. For each disabled index, set it's state to Normal.
 
This assumes ServiceControl is using the default port and host name; adjust the url accordingly if this is not the case.


### Heartbeat failure in ASP.NET applications


#### Scenario

After a period of inactivity, a web application endpoint is failing with the message:

```
Endpoint has failed to send expected heartbeat to ServiceControl. It is possible that the endpoint could be down or is unresponsive. If this condition persists restart the endpoint.
```

When accessed, the web application is operating as expected. However shortly after accessing the web application, the heartbeat message is restored and indicates the endpoint status as active.


#### Causes and solutions

The issue is due to the way IIS handles application pools. By default after a certain period of inactivity, the application pool is stopped or, under certain configurable conditions, the application pool is recycled. In both cases the ServicePulse heartbeat is not sent anymore until a new web request comes in waking up the web application.

There are two ways to avoid the issue:

 1. Configure IIS to avoid recycling
 1. Use a periodic warm-up HTTP GET to make sure the website is not brought down due to inactivity (the frequency needs to be less than 20 minutes, which is the default IIS recycle-on-idle time)

Starting from IIS 7.5, the above steps can be combined into one by following these steps:

 1. Enable [AlwaysRunning mode](https://msdn.microsoft.com/en-us/library/ee677285.aspx) for the application pool of the site. Go to the application pool management section, open the Advanced Settings, and in the General settings switch `Start Mode` to `AlwaysRunning`.
 1. Enabled Preload for the site itself. Right click on the site, then Manage Site in Advanced Settings, and in the General settings switch `Enable Preload` to `true`.
 1. Install the [Application Initialization Module](https://docs.microsoft.com/en-us/iis/get-started/whats-new-in-iis-8/iis-80-application-initialization).
 1. Add the following to the web.config in the [system.webServer node](https://msdn.microsoft.com/en-us/library/ms689429.aspx).

```xml
<applicationInitialization doAppInitAfterRestart="true" >
    <add initializationPage="/" />
</applicationInitialization>
```

In some cases, configuring IIS to avoid recycling is not possible. Here, the recommended approach is the second one. It also has the benefit of avoiding the "first user after idle time" wake-up response-time hit.


### Duplicate endpoints appear in ServicePulse after re-deployment

This may occur when an endpoint is re-deployed or updated to a different installation path (a common procedure by deployment managers like Octopus).

The installation path of an endpoint is used by ServiceControl and ServicePulse as the default mechanism for generating the unique Id of an endpoint. Changing the installation path of the endpoint affects the generated Id, and causes the system to identify the endpoint as a new and different endpoint.

To address this issue, see [Override host identifier](/nservicebus/hosting/override-hostid.md).


### After enabling heartbeat plugins for NServiceBus version 3 endpoints, ServicePulse reports that endpoints are inactive

Messages that were forwarded to the audit queue by NServiceBus version 3.x endpoints did not have the `HostId` header available which uniquely identifies the endpoint. Adding the heartbeat plugin for these endpoints automatically enriches the headers with this `HostId` information using a [message mutator](/nservicebus/pipeline/message-mutators.md). Since the original message that was processed from the audit/error queue did not have this identifier, it is hard to correlate the messages received via the heartbeat that these belong to the same endpoint. Therefore there appears to be a discrepancy in the endpoints indicator.

To address this issue:

 * Add the heartbeat plugin to all NServiceBus version 3 endpoints, which will add the required header with the host information.
 * Restart ServiceControl to clear the endpoint counter.
