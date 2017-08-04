---
title: Monitor with ServiceControl events
component: ServiceControlContracts
reviewed: 2016-08-31
tags:
 - Notifications
related:
 - servicecontrol
 - servicecontrol/contracts
 - servicecontrol/plugins
 - samples/servicecontrol/monitoring3rdparty
---

## Prerequisistes

 1. [Install ServiceControl](/servicecontrol/installation.md).
 1. Using [ServiceControl Management](/servicecontrol/license.md#servicecontrol-management-app) tool, set up ServiceControl to monitor endpoints using MSMQ transport.
 1. Ensure the `ServiceControl` process is running before running the sample. 


## Running the project

The project presents how to handle two kinds of events:


### MessageFailed event

A `MessageFailed` event is emitted whenever processing a message fails and message is moved to the error queue.

In order to observe that, press `Enter` in the `NServiceBusEndpoint` console window. That will send a new `SimpleMessage`. Processing of the message fails every time.

When a `MessageFailed` event is received, the `EndpointsMonitor` prints the following message in its console window: 

> `Received ServiceControl 'MessageFailed' event for a SimpleMessage.`


### HeartbeatStopped and HeartbeatRestored events

The `HeartbeatStopped` event is emitted whenever an endpoint fails to send a control message at expected interval. The `HeartbeatRestored` event is emitted whenever the endpoint successfully sends a control message again. 

Note: The Monitor needs to receive at least one control message, before it can observe that endpoint stopped responding.

In order to observe that, stop the `NServiceBusEndpoint` application and wait for up to 30 seconds. When a `HeartbeatStopped` event is received, the `EndpointsMonitor` prints the following message in its console window:

> `Heartbeat from NServiceBusEndpoint stopped.`

Then restart the `NServiceBusEndpoint` application and wait for up to 30 seconds. When a `HeartbeatRestored` event is received, the `EndpointsMonitor` prints the following message in its console window:

> `Heartbeat from EndpointsMonitoring.NServiceBusEndpoint restored.`


## Code walk-through 

The solution consists of two projects. The `NServiceBusEndpoint` is a simple endpoint which is monitored by the `EndpointsMonitor`.


### NServiceBusEndpoint

The retries are disabled in the sample for simplicity, therefore the message is immediately moved to the error queue after processing failure:

snippet: DisableRetries

The `MessageFailed` event is published for any standard NServiceBus endpoint that is monitored by ServiceControl.

In order to receive `HeartbeatStopped` and `HeartbeatRestored` events, the endpoint needs to use the [heartbeats plugin](/servicecontrol/plugins/heartbeat.md).

NOTE: Heartbeat control messages are sent [every 30 seconds by default](/servicecontrol/plugins/heartbeat.md#configuration-time-to-live-ttl). That means there might be up to 30 seconds delay before ServiceControl realizes that it lost or restored connection with the endpoint.


### EndpointsMonitor

In order to get notifications when the exposed ServiceControl events occur, create an NServiceBus endpoint. Then reference the `ServiceControl.Contracts` NuGet package and implement a handler which handles ServiceControl events:

snippet: ServiceControlEventsHandlers
