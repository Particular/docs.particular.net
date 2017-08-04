---
title: Custom Check Monitoring
summary: Describes how ServicePulse uses custom checks to monitor and detect problem that are unique to the solution or endpoint(s) monitored
component: ServicePulse
reviewed: 2016-10-06
related:
- samples/servicecontrol/monitoring3rdparty
- servicepulse/how-to-configure-endpoints-for-monitoring
- servicecontrol/plugins
redirects:
- servicepulse/how-to-develop-custom-checks
---

ServicePulse monitors the health and activity of NServiceBus endpoints using heartbeat messages and activity indications. (See [Introduction to Endpoints and Heartbeats in ServicePulse](intro-endpoints-heartbeats.md).)

However, an endpoint and its hosting process may be fully functional (in the sense that it and its hosting process are able to send, receive, and process messages) but required external conditions may not be met, and required services that the endpoint's business logic relies on may malfunction. As a result, the endpoints may not be able to function as expected.

These external conditions and services are specific for each solution and/or endpoint. ServicePulse Custom Checks can be programmed to monitor such failures by developing Custom and Periodic Checks: customized logic that verifies that these conditions are met and that all services the endpoint and solution rely on are functioning as expected, and raises alerts whenever they are not.


### Common Scenarios

While the specific needs and dependencies of solutions and endpoints may vary significantly, there are common scenarios of required external conditions and services.


#### Connectivity

Most endpoints require connectivity to local intranet or external internet. Some also require VPN connectivity.

It is recommended that to periodically check that the endpoint (and its host) can indeed connect to the resources it requires.

**Examples:**

 * In a store & forward pattern (MSMQ): can an endpoint hosted on one machine connect to another machine, on which another endpoint is located ?
 * In a broker pattern: can the endpoint connect to the broker (e.g., SQL Server)?
 * If required by the endpoint, can the endpoint connect to the local intranet?
 * If required by the endpoint, can the endpoint connect to the internet?
 * If required by the endpoint, can the endpoint connect with the required security settings, credentials, and VPN software?


#### Storage

 * When local or remote storage is required by the endpoint, is that storage location available, accessible, and properly configured (security, permissions, quota, etc.)?
 * When local or remote storage is required by the endpoint, is there enough available storage left for required operations? If there is a requirement for a minimum free space available, is that requirement met?


#### External Services

 * When an endpoint needs to communicate with external services (credit card validation, identity and authentication service, CRM, etc.), is that service available, accessible, and responsive?


### Custom Checks in ServicePulse


#### Custom Check Indicator

The custom check indicator in the ServicePulse dashboard indicates when one or more custom checks fails. The number of failed custom checks is presented on the upper right of the indicator, and a descriptive event is displayed in the Recent Events list.

![Custom Checks](images/custom-checks.png 'width=500')

For example, in the image above, the number 4 indicates that there are four failing custom checks. These custom checks may be all located on the same endpoint or on four separate endpoints. Use the Custom Checks details page to drill down into the details of the failures and their location.


#### Custom Checks Details Page

For a detailed display of the currently failed and failing custom checks per endpoint, click the custom checks indicator in the ServicePulse dashboard, or the related link in the navigation bar.

![Custom Checks Details page](images/custom-checks-details.png 'width=500')

Note there are four failing custom checks located on two endpoints (two failures per endpoint).

If one or more of the failures is expected (for example, it may be caused by a planned maintenance activity that brought down an external service the endpoint relies on) it is possible to mute the specific occurrence of the custom check.

Muting a custom check means that the specific custom check failure event is discarded. If an additional failure occurs later (for example, when the custom check is defined to run periodically), a new failure event will be raised.