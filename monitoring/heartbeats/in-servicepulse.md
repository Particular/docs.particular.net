---
title: Managing endpoint heartbeats in ServicePulse
summary: Describes how ServicePulse monitors endpoints activity and availability using heartbeat messages
reviewed: 2018-01-05
component: Heartbeats
versions: 'Heartbeats:*'
---

ServicePulse relies on heartbeat messages sent from the heartbeat plugin to indicate whether an endpoint is active or inactive. The main dashboard shows a heartbeats icon which will indicate if there are any inactive endpoints.

IMAGE: Main Dashboard with Heartbeats icon showing red with 2 or 3 inactive endpoints

Click this icon to go to the endpoints overview. This page shows a list of active and inactive endpoint instances. Each endpoint instance shows when the most recent heartbeat from that instance was received.

IMAGE: Endpoints overview


## Hiding endpoints

By default, each new endpoint discovered by ServicePulse is monitored for heartbeats. If the endpoint is not sending heartbeats (either because it is offline or because it does not have the heartbeats plugin installed) it will immediately be marked as inactive. 

Whether or not an endpoint instance is monitored can be configured in the ServicePulse configuration page. 

IMAGE: ServicePulse configuration page

Setting the endpoint to "Off" will prevent it from appearing on the endpoints overview screen and from affecting the dashboard.

NOTE: This is a configuration setting only and the endpoint itself may still be configured to send heartbeat messages.

NOTE: At this time, it is not possible to permanently remove an endpoint from the ServicePulse configuration screen.
