---
title: Performance Metrics in ServicePulse
summary: Describes how to use ServicePulse to review endpoint performance metrics
reviewed: 2018-01-12
---

ServicePulse collects displays performance monitoring data about running endpoints on the Monitoring tab.

![ServicePulse monitoring tab](servicepulse-monitoring-tab.png)

NOTE: ServicePulse will only display the monitoring tab if it is configured to point to a ServiceControl monitoring instance.

The main monitoring tab shows a list of logical endpoints as well as the performance data collected about those endpoints. If a logical endpoint is running more than one physical instance, a badge will show a count of physical instances.

![ServicePulse monitoring tab instance count](servicepulse-instance-count.png)

Clicking on an endpoint in the main monitoring tab will open the endpoint details page.

![ServicePulse monitoring details page](servicepulse-monitoring-details.png)

The endpoint details page shows larger graphs for each of the performance metrics gathered for the logical endpoint. It also contains a number of breakdown views. 

The default breakdown view is by Message Type.

![ServicePulse details breakdown by message type](servicepulse-messagetype-breakdown.png)

This shows a breakdown of performance metrics for each type of message that the endpoint processes.

The second breakdown view is by Instance.

![ServicePulse details breakdown by physical instance](servicepulse-physicalinstance-breakdown.png)

Each logical endpoint can be running one or more physical instances. This view shows a breakdown of performance metrics for each physical instance. 

NOTE: Each physical instance should be configured with it's own instance id, which is shown here. See [installing the plugin](install-plugin.md) for more information about instance ids.


## Reporting period

The main monitoring tab and the endpoint details page both contain an option to change the reporting period. 

![ServicePulse reporting period](servicepulse-reportingperiod.png)

All graphs present the data collected during this reporting period. Where an average is shown, it is the average over the selected reporting period.

NOTE: The screen is refreshed more frequently when a shorter reporting period is selected.


## Data retention

All performance metric data is retained in memory in the ServiceControl Monitoring instance. Restarting the ServiceControl Monitoring instance will cause all performance metric data to be lost.

Performance metric data is only kept by the ServiceControl Monitoring instance long enough to support the longest reporting period (1 hour).


## Disconnected endpoints

If an endpoint instance stops sending metric data it will appear with a warning indicator.

![ServicePulse disconnected endpoint warning indicator](servicepulse-warningindicator.png)

This warning indicator will show in the instances break down on the endpoint details page for each instance that is not sending metric data.


## Endpoints with failed messages

If ServicePulse is aware of failed messages to be processed by an endpoint, it will show a badge indicating how many failed messages there are.

Click this badge to review the failed messages for this endpoint. See [Failed Message Monitoring](/servicepulse/intro-failed-messages.md) for more details.