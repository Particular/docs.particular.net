---
title: Managing custom checks in ServicePulse
summary: Describes how ServicePulse monitors custom check activity
reviewed: 2018-01-05
component: CustomChecks
versions: 'CustomChecks:*'
---

ServicePulse monitors the health and activity of an NServiceBus endpoint using [Heartbeats](/monitoring/heartbeats/) and [Custom Checks](/monitoring/custom-checks/).

The main dashboard shows a custom checks icon which will indicate if there are any failing custom checks.

IMAGE: Main Dashboard with custom checks icon showing red with 2 or 3 failing custom checks. 

Click this icon to go to the custom checks details page. This page shows a list of all custom checks and their current status.

IMAGE: Custom Checks overview

Each custom check includes information about the endpoint instance that reported the status and how long ago the status was updated.


## Muting custom checks

When a custom check fails it will continue to make the main Custom Checks badge on the dashboard red until it succeeds again. If the custom check has a long period and the problem has been dealt with, it may make sense to mute the custom check. 

Once a custom check has been muted, it will be ignored until it's status is next reported by the endpoint.
