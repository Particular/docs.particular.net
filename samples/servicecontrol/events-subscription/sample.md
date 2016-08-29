---
title: Monitoring NServiceBus endpoints with ServiceControl events.
component: ServiceControlContracts
reviewed: 2016-03-26
tags:
- ServiceControl
related:
- servicecontrol/contracts
---

## Prerequisistes

1. [Install ServiceControl](/servicecontrol/installation).
1. Using [ServiceControl Management Utility](/servicecontrol/license#licensing-servicecontrol-servicecontrol-management-utility) tool, set up ServiceControl to monitor endpoints using MSMQ transport.


## Running the project

The project presents how to handle two kinds of events:


### MessageFailed event

A `MessageFailed` event is emitted whenever processing a message fails and message is moved to the error queue.

In order to observe that, press `Enter` in the `EndpointsMonitoring.NServiceBusEndpoint` console window. That will send a new `SimpleMessage`. Processing of the message fails every time.

When a `MessageFailed` event is received, the `EndpointsMonitor` prints a red `Received ServiceControl 'MessageFailed' event for a SimpleMessage.` text in its console window.

![Failed message event](images/failedmessage-event.png)


### HeartbeatStopped and HeartbeatRestored events

The `HeartbeatStopped` event is emitted whenever an endpoint fails to send a control message at expected interval. The `HeartbeatRestored` event is emitted whenever the endpoint successfully sends a control message again. 

Note that Monitor needs to receive at least one control message, before it can observe that endpoint stopped responding, and that `HeartbeatStopped` event is emitted only following the `HeartbeatStopped` event. 

In order to observe that, stop the `EndpointsMonitoring.NServiceBusEndpoint` application and wait for up to 30 seconds. When a `HeartbeatStopped` event is received, the `EndpointsMonitor` prints a yellow `Heartbeat from EndpointsMonitoring.NServiceBusEndpoint stopped.` text in its console window.

Then restart the `EndpointsMonitoring.NServiceBusEndpoint` application and wait for up to 30 seconds. When a `HeartbeatRestored` event is received, the `EndpointsMonitor` prints a green `Heartbeat from EndpointsMonitoring.NServiceBusEndpoint restored.` text in its console window.

![Heartbeat events](images/heartbeats_events.png)


## Code walk-through 
